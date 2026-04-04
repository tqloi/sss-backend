using MongoDB.Driver;
using SSS.Application.Abstractions.External.AI;
using SSS.Application.Abstractions.External.AI.Embedding;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.External.AI.Vector;
using SSS.Application.Features.AI.Common;
using SSS.Domain.Entities.Planning;
using System.Text.Json;

namespace SSS.Infrastructure.External.AI.OpenAI.PipeLine
{
    public class RagPipeline : IPipeLine
    {
        private readonly ILlmRouter _llmRouter;
        private readonly IEmbeddingProvider _emp;
        private readonly IQdrantClient _vec;
        private readonly AiOptions _config;
        public RagPipeline(ILlmRouter llmRouter, IEmbeddingProvider emp, IQdrantClient vec, AiOptions config)
        {
            _llmRouter = llmRouter;
            _emp = emp;
            _vec = vec;
            _config = config;
        }

        public async Task IngestAsync(string studyplanId, string userId, IEnumerable<(string Text, string? Source)> chunks, CancellationToken ct = default)
        {
            int dim = await _emp.GetDimAsync(ct);
            await _vec.EnsureCollectionAsync(dim, ct);
            var list = new List<VectorPoint>();
            foreach (var (text, source) in chunks)
            {
                var vec = await _emp.EmbeddingAsync(text, ct);
                list.Add(new VectorPoint(Guid.NewGuid().ToString("N"), vec, text, source, userId, studyplanId,
            DataType: source ?? "unknown",
            CreatedAt: DateTime.UtcNow));
            }
            await _vec.UpsertAsync(list, ct);
        }
        public async Task IngestBehaviorAsync(string studyplanId, string userId, string studyplanmoduleId, IEnumerable<(string Text, string? Source)> chunks, CancellationToken ct = default)
        {
            _ = studyplanmoduleId;
            int dim = await _emp.GetDimAsync(ct);
            await _vec.EnsureCollectionAsync(dim, ct);
            var list = new List<VectorPoint>();
            foreach (var (text, source) in chunks)
            {
                var dataType = source ?? "unknown";
                var existingPoint = (await _vec.GetLatestUserBehavior(
                    limit: 1,
                    userId: userId,
                    studyplanId: studyplanId,
                    dataType: dataType,
                    ct: ct)).FirstOrDefault();

                var vec = await _emp.EmbeddingAsync(text, ct);
                list.Add(new VectorPoint(existingPoint?.Id ?? Guid.NewGuid().ToString("N"), vec, text, source, userId, studyplanId,
            DataType: dataType,
            CreatedAt: DateTime.UtcNow));
            }
            await _vec.UpsertAsync(list, ct);
        }
        public async Task<string> GenerateBehaviorResultAsync(string studyBehaviorContextJson, CancellationToken ct = default)
        {
            var systemPrompt = """
You are an AI system that analyzes user learning behavior for vector retrieval.

Input may include:
- NodeScope (current node + recent linked nodes)
- Module data
- StudySession and SessionTask data
- QuizAttempt data
- StudyEvents (click/interaction logs with payload and timestamp)
- StudyEventSummary (aggregated interaction counts)

Your task:
- Generate EXACTLY ONE concise paragraph in English.
- Base conclusions strictly on provided data only.
- The paragraph MUST explicitly cover all 4 dimensions:
  1) Task discipline and deadline adherence
  2) Quiz performance consistency and completion quality
  3) Learning engagement from event activity
  4) Overall study discipline verdict

Deadline adherence rules (mandatory):
- A task is on-time only if CompletedAt/EndTime <= ScheduledDate.
- You MUST explicitly state whether behavior is mostly on-time, mixed, or mostly late.
- If timestamps are insufficient, explicitly state deadline evidence is limited.

Quiz rules:
- Use quiz attempts to describe completion consistency, score level/trend, and struggle signals.
- If quiz evidence is sparse, explicitly state evidence is limited.

Engagement rules:
- Infer engagement only from observed frequency, recency, and distribution across event types/categories/content modes.
- Treat Payload fields (e.g., contentId, contentTitle, contentType, nodeId, studyPlanId) as contextual interaction evidence.
- If engagement evidence is sparse, explicitly state evidence is limited.

Output rules:
- Plain text only. No JSON, markdown, or bullet points.
- Neutral, factual, compact, semantically rich.
- Do not provide recommendations or advice.
- Do not invent facts.
""";
            var userPromptWithContext = $"""
Analyze the following user learning behavior dataset and generate one paragraph behavior summary:

StudyExecutionData:
{studyBehaviorContextJson}

Focus on:
- Completion discipline with explicit on-time vs late conclusion
- Quiz behavior quality and consistency
- Learning engagement from StudyEvents and StudyEventSummary
- Completion vs skip/incomplete balance
- Overall study discipline (good / average / needs improvement based on evidence)
""";

            var llmChatProvider = _llmRouter.Resolve(LlmTask.GenerateRoadmap);
            var response = await llmChatProvider.AskAsync(systemPrompt, userPromptWithContext, ct);
            return response;
        }


        public async Task<string> GenerateSurveyResultAsync(UserLearningTargetDto target, UserLearningBehaviorDto behavior, CancellationToken ct = default)
        {
            var systemPrompt = """
You are an AI system that converts structured learning profile data into a single, concise, semantically rich English text.

Your task:
- Merge UserLearningTarget and UserLearningBehavior data into ONE coherent paragraph.
- Do NOT invent or infer any information.
- Do NOT output JSON, markdown, or bullet points.
- Output plain natural language text only.
- Preserve all important signals related to learning goals, level, deadline, availability, learning style, and preferences.
- Keep the tone factual, neutral, and embedding-friendly.

The output will be stored in a vector database (Qdrant) for semantic retrieval.
"""; 
            var userPromptWithContext = $"""
Convert the following learning profile into a single semantic text.

UserLearningTarget:
- TargetRole: {target.TargetRole}
- CurrentLevel: {target.CurrentLevel}
- TargetDeadlineMonths: {target.TargetDeadlineMonths}
- GoalDescription: {target.GoalDescription}

UserLearningBehavior:
- AvailableDays: {behavior.AvailableDaysJson}
- PreferredTimeBlocks: {behavior.PreferredTimeBlocksJson}
- SessionLengthPrefMinutes: {behavior.SessionLengthPrefMinutes}
- LearningStyleWeights:
  - Visual: {behavior.WVisual}
  - Reading: {behavior.WReading}
  - Practice: {behavior.WPractice}
""";

            var llmChatProvider = _llmRouter.Resolve(LlmTask.GenerateRoadmap);
            var response = await llmChatProvider.AskAsync(systemPrompt, userPromptWithContext, ct);
            return response;
        }
        public async Task<string> GenerateRoadmapAsync(string question, string subjectid, CancellationToken ct = default)
        {
            var systemPrompt = """
You are an AI that generates detailed learning roadmaps in JSON strictly for a backend technology specified by the user.

You MUST output JSON that can be deserialized by System.Text.Json
using the following EXACT enum definitions.

====================
ENUM DEFINITIONS
====================

NodeDifficulty:
- Beginner
- Intermediate
- Advanced

ContentType:
- Article
- Book
- Course
- Exercise
- Quiz
- Project

EdgeType:
- Prerequisite
- Recommended
- Next

====================
ROADMAP DESIGN PRINCIPLES
====================

Design the roadmap using a structure inspired by roadmap.sh:

1. The roadmap MUST be divided into clear learning phases:
   - Foundation
   - Core
   - Advanced
   - Practical / Real-world

2. Each phase MUST:
   - Build logically on the previous phase
   - Introduce dependencies explicitly via edges
   - Avoid random or loosely connected topics

3. Nodes MUST represent concrete skills or competencies,
   NOT vague concepts.

4. The roadmap MUST be:
   - Skill-oriented
   - Job-ready
   - Practical rather than academic

5. Avoid generic topics like:
   - "Overview"
   - "Introduction only"
   - "Miscellaneous"

   IMPORTANT CLARIFICATION ABOUT PHASES:

Learning phases (Foundation, Core, Advanced, Practical / Real-world)
are CONCEPTUAL ONLY and MUST NOT appear in the JSON output.

Phases are used ONLY to guide roadmap structure and ordering.

ALL nodes MUST map their difficulty strictly as follows:

PHASE → NodeDifficulty MAPPING (MANDATORY):

- Foundation → Beginner
- Core → Intermediate
- Advanced → Advanced
- Practical / Real-world → Advanced

The JSON field "difficulty" MUST ALWAYS be one of:
- "Beginner"
- "Intermediate"
- "Advanced"

   ====================
NODE RULES
====================

- Each node represents ONE clear learning goal.
- Each node MUST:
  - Have increasing difficulty across the roadmap
  - Contain multiple contents (not just one)
- Beginner nodes should focus on:
  - Fundamentals
  - Tooling
  - Core concepts
- Intermediate nodes should focus on:
  - Architecture
  - Patterns
  - Best practices
- Advanced nodes should focus on:
  - Performance
  - Scalability
  - Real-world complexity

  ====================
CONTENT URL RULES
====================

- URLs are OPTIONAL and may be null.
- DO NOT fabricate or guess URLs.

- When providing a URL, it MUST follow these rules:

1. Prefer OFFICIAL sources for the specified backend technology, such as:
   - Official documentation websites
   - Official GitHub repositories
   - Official learning portals maintained by the technology owner

2. If no clear official source exists for the content:
   - Set "url": null

3. ContentType-specific preferences:
   - Article:
     - Official documentation
     - Official engineering blogs
   - Exercise / Project:
     - GitHub repositories
     - Coding platforms
     - URL may be null if task-based

4. URLs MUST be:
   - Public
   - Stable
   - Technology-relevant

====================
CONTENT RULES
====================

For EACH node:
- Include 2–5 contents.
- Prefer the following distribution:
  - Article for theory
  - Course for structured learning
  - Exercise / Project for practice
- At least 30% of nodes MUST include:
  - Exercise or Project content
- Project content MUST represent:
  - Real-world scenarios
  - Practical implementation tasks
  ====================
EDGE RULES
====================

- Use EdgeType = "Next" for main learning flow.
- Use EdgeType = "Prerequisite" only when knowledge dependency is strict.
- Avoid overly complex graph structures.
- The roadmap should be readable as a progression path.

====================
STRICT RULES
====================

- Enum values MUST match EXACTLY (case-sensitive).
- DO NOT invent new enum values.
- DO NOT use synonyms.
- DO NOT use values like: Sequential, Optional, Documentation, Docs, Guide.
- If unsure:
  - Use EdgeType = "Next"
  - Use ContentType = "Article"
- Return ONLY valid JSON.
- Do NOT wrap in markdown.
- Do NOT add explanations.

====================
REQUIRED JSON SHAPE
====================

{
  "roadmap": {
    "subjectId": {subjectid},
    "title": string,
    "description": string | null
  },
  "nodes": [
    {
      "clientId": string,
      "title": string,
      "description": string | null,
      "difficulty": "Beginner" | "Intermediate" | "Advanced",
      "orderNo": number
    }
  ],
  "contents": [
    {
      "clientId": string,
      "nodeClientId": string,
      "contentType": "Article" | "Book" | "Course" | "Exercise" | "Quiz" | "Project",
      "title": string,
      "url": string | null,
      "description": string | null,
      "estimatedMinutes": number | null,
      "difficulty": string | null,
      "orderNo": number,
      "isRequired": boolean
    }
  ],
  "edges": [
    {
      "fromNodeClientId": string,
      "toNodeClientId": string,
      "edgeType": "Prerequisite" | "Recommended" | "Next",
      "orderNo": number | null
    }
  ]
}
""";
            var userPromptWithContext = $"""
                    QUESTION:
                    {question}

                    SUBJECT_ID:
                    {subjectid}
                    """;

            var llmChatProvider = _llmRouter.Resolve(LlmTask.GenerateRoadmap);
            var response = await llmChatProvider.AskAsync(systemPrompt, userPromptWithContext, ct);
            return response;
        }
        public async Task<string> BuildStudyPlanContextAsync(
            string userId,
            string studyPlanId,
            CancellationToken ct = default)
        {
            // 1. Vector đại diện cho "tạo study plan"
            var query = "User survey information describing learning orientation, target role, experience level, and preferred roadmap.";
            var queryVector = await _emp.EmbeddingAsync(query, ct);

            // 2. Lấy surveys của user
            var hit_user_profile = await _vec.SearchByUserId(
                vector: queryVector,
                topK: 1,
                userId: userId,
                studyplanId: studyPlanId,
                dataType: "user_profile",
                ct: ct);

            var hit_user_behavior = await _vec.GetLatestUserBehavior(
                limit: 3,
                userId: userId,
                studyplanId: studyPlanId,
                dataType: "user_behavior",
                ct: ct);

            // 3. Ghép context
            //var context = string.Join(
            //    "\n---\n",
            //    hits.Select(h => h.Text));

            var context = string.Join("\n---\n",
                new[] { hit_user_profile.FirstOrDefault() }
                .Concat(hit_user_behavior)
                .Where(h => h != null)
                .Select(h => h.Text)
                );



            return context;
        }

        public async Task<string> GenerateStudyPlanAsync(
    string userId,
    string studyPlanId,
    object roadmap,
    object roadmapnode,
    CancellationToken ct = default)
        {
            // 1. Build context từ vector DB
            var context = await BuildStudyPlanContextAsync(userId, studyPlanId, ct);
            Console.WriteLine(context);
            var roadmapJson = roadmap is string roadmapText ? roadmapText : JsonSerializer.Serialize(roadmap);
            var roadmapNodeJson = roadmapnode is string nodeText ? nodeText : JsonSerializer.Serialize(roadmapnode);
            var currrentDate = DateTime.UtcNow;
            var dayName = currrentDate.DayOfWeek.ToString();

            // 2. System prompt chuyên cho study plan
            var systemPrompt = """
You are a backend-aware AI responsible for generating TASK ITEMS
for a learning system.

Your task is to generate STUDY TASKS
for EXACTLY ONE roadmap node.

======================
CRITICAL RULES (MUST FOLLOW)
======================

1. Output MUST be a SINGLE valid JSON array
2. Do NOT include explanations, comments, markdown, or extra text
3. Generate tasks for ONE AND ONLY ONE roadmap node
4. You MUST NOT generate tasks for any other roadmap node
5. ALL tasks MUST use the SAME roadmapNodeId provided
6. Do NOT infer or expand to other roadmap nodes
7. Decisions MUST be driven by the user's survey context
8. If behavior signals are present in context, adapt plan pacing and duration accordingly
9. Tasks MUST stay strictly aligned with the target node title/description/difficulty and node-level content cues

======================
INPUT GUARANTEES
======================

You will receive:
- User survey context
- User behavior context (if available)
- A roadmap (FOR CONTEXT ONLY)
- EXACTLY ONE roadmap node (TARGET NODE)

The roadmap is provided ONLY to understand progression and difficulty.
The roadmap node is the ONLY entity you are allowed to generate tasks for.

If behavior context exists, it has higher priority than generic pacing assumptions.

======================
PERSONALIZATION PRIORITY
======================

Use the context in this priority order:
1) User behavior context (latest signals from vector DB)
2) User survey/profile context
3) Target roadmap node details

Behavior adaptation expectations:
- If behavior suggests mostly late/mixed completion, use lighter pacing, longer durations, and wider spacing.
- If behavior suggests mostly on-time and stable completion, keep normal durations and moderate spacing.
- If behavior evidence is limited, apply neutral pacing.

Do not copy behavior text. Use it only to adapt schedule realism and workload.

======================
LEVEL ALIGNMENT RULES (MANDATORY)
======================

- Detect user level from USER SURVEY CONTEXT (e.g., CurrentLevel).
- Calibrate task depth by combining: user level + target node difficulty.
- Never output a plan that is purely beginner review when user level is Intermediate or above.

If user level is Beginner:
- Focus on fundamentals, guided practice, and basic implementation.
- Keep terminology simple and step-by-step.
- Require at least 3 beginner-friendly tasks on fundamentals (syntax, control flow, methods, or basic OOP).
- Include at most 1 task that involves refactor/debug/optimization.
- Keep cognitive load gradual: concept introduction -> guided practice -> small integration task.
- Each task should have a clear, concrete outcome (e.g., write X snippet, complete Y mini exercise).

If user level is Intermediate:
- Limit pure syntax/fundamental review to at most 1 task.
- Require at least 2 tasks involving application/implementation/debugging/refactoring.
- Include at least 1 task that validates quality (testing, edge cases, or error handling strategy).
- Require at least 1 implementation task that combines multiple concepts from the same node.
- Emphasize decision quality: code structure, readability, maintainability, and bug prevention.
- Avoid beginner-style wording (e.g., "learn what X is", "introduction to").

If user level is Advanced:
- Stay strictly within TARGET NODE scope, but increase cognitive depth and rigor.
- Require at least 1 task that compares two implementation approaches and justifies trade-offs.
- Require at least 1 task that defines explicit quality gates (testability, error-path coverage, maintainability).
- Require at least 2 tasks focused on hardening: defensive coding, null-safety, robustness, and refactoring rationale.
- Limit beginner-style review to at most 1 brief refresher task.
- Every advanced task should include a concrete deliverable such as decision notes, validation checklist, or refactor rationale.
- Avoid introductory explanations.

If level evidence is missing/ambiguous:
- Use neutral intermediate-safe depth and explicitly avoid overly basic repetition.

======================
NODE GROUNDING RULES
======================

- Every task must be clearly attributable to the TARGET ROADMAP NODE only.
- Task titles/descriptions must reference skills/topics implied by the target node content.
- Do not include concepts that belong to other roadmap nodes, even if related.
- Keep progression inside the same node: foundational -> practice -> implementation/validation.

======================
OUTPUT SCHEMA (STRICT)
======================

[
  {
    "roadmapNodeId": number,
    "title": string,
    "description": string | null,
    "status": "Planned",
    "estimatedDurationSeconds": number,
    "scheduledDate": "YYYY-MM-DDTHH:mm:ss"
  }
]

======================
TASK DESIGN RULES
======================

- Generate 4-6 tasks ONLY for the given roadmap node
- Tasks must be concrete and actionable
- estimatedDurationSeconds MUST be a NUMBER (integer)
- Range: More than 900 seconds (15 minutes)
- Duration MUST be estimated dynamically based on:
- Complexity of the task
- Type of task (analysis, coding, testing, etc.)
- User behavior patterns (e.g., speed, past performance if available)
- Simpler tasks → shorter duration
- Complex or implementation-heavy tasks → longer duration
- scheduledDate must be realistic and progressive
- Do NOT generate tasks for any other node
- scheduledDate MUST be based on CURRENT DATETIME above
- scheduledDate MUST be >= current datetime
- scheduledDate MUST increase progressively
- Avoid same timestamp for all tasks; spread tasks realistically
- Task sequence must increase in cognitive depth from earlier to later tasks
- Avoid near-duplicate tasks with only wording changes

======================
BEHAVIOR-ADAPTIVE RULES
======================

When behavior context indicates the learner is often late, inconsistent, or skips tasks:
- Increase estimatedDurationSeconds per task by around 15-30% compared to normal expectation
- Add more spacing between tasks (prefer gaps of at least 1 day)

When behavior context indicates strong on-time and consistent completion:
- You may generate 3-5 tasks
- Keep estimatedDurationSeconds in normal range for node difficulty
- Use moderate spacing (can be denser than late-profile scheduling)

When behavior evidence is weak or unavailable:
- Use neutral pacing (3-4 tasks) with balanced spacing
- Do not overfit assumptions

======================
TIME DISTRIBUTION RULES
======================

- Assume the learner prefers a relaxed pace over an aggressive schedule
- If a task has a long duration (>= 3600 seconds), it is ACCEPTABLE and PREFERRED
  to skip the next available day before scheduling the following task
- Do NOT force tasks to be scheduled on consecutive days
- It is better to leave gaps between tasks than to overload the learner
- Scheduling fewer tasks with more spacing is preferred over dense scheduling
""";
            var userPrompt = $$"""
USER SURVEY CONTEXT:
${{context}}

ROADMAP (FOR CONTEXT ONLY):
${{roadmapJson}}

TARGET ROADMAP NODE (GENERATE TASKS FOR THIS NODE ONLY):
${{roadmapNodeJson}}

CURRENT DATETIME (UTC, SOURCE OF TRUTH):
{{{currrentDate: yyyy-MM-ddTHH:mm:ss}}}
{{{dayName}}}

SCHEDULING PREFERENCE:
- Prefer relaxed pacing over fast completion
- It is acceptable to skip days between tasks, especially for long tasks
--------------------------------------------------
GOAL
--------------------------------------------------

Generate task items ONLY for the TARGET ROADMAP NODE above.
All tasks MUST use its roadmapNodeId.

Before finalizing, self-check:
- Is difficulty calibrated to user level from survey context?
- Is content still strictly inside target node scope?
- Are later tasks deeper than earlier tasks?
- For Beginner: are at least 3 tasks truly foundational and step-by-step?
- For Intermediate: is there no more than 1 pure review task and at least 1 quality-validation task?
""";


            var llmChatProvider = _llmRouter.Resolve(LlmTask.GenerateStudyPlan);
            // 3. Gọi GPT provider
            var response = await llmChatProvider.AskAsync(
                systemPrompt,
                userPrompt,
                ct);


            return response;
        }

        public async Task<string> GenerateQuizQuestionsAsync(
            object roadmap,
            object roadmapnode,
            string level,
            int questionCount,
            CancellationToken ct = default)
        {
            var systemPrompt = """
You are an AI that generates quiz questions and options in strict JSON format.
Return ONLY valid JSON array. No markdown, no explanation.
Each question must include options.
For SingleChoice: exactly one option has isCorrect=true.
For MultipleChoice: at least one option has isCorrect=true.
Keep questions relevant to the provided TARGET ROADMAP NODE only.

Scope rules (mandatory):
- Generate questions ONLY from the "Target Roadmap Node" content.
- Do NOT generate broad roadmap-wide topics.
- Do NOT include topics from other nodes/phases even if related.
- Use "Roadmap" only as lightweight background metadata (title/description).
- If a topic is not clearly inferable from the target node, exclude it.

Difficulty progression is mandatory:
- Questions must become harder from first to last.
- orderNo must represent ascending difficulty (lowest difficulty first).
- The first questions should test fundamentals/recall.
- Middle questions should test understanding/application.
- Final questions should test analysis/problem-solving in realistic scenarios.
- Do not mix a hard question before an easier one.
- scoreWeight should be non-decreasing with orderNo.

Each questionKey must be unique within the response and must look random, not sequential.
Use uppercase letters, digits, or underscores only.
Avoid simple keys like Q1, Q2, QUESTION_1, or sequential numbering.
Example valid patterns: QUIZ_A7K2M9, NODE_X91PQ4, QQ_7F2KD8.
""";

            var userPrompt = $$"""
Generate {{questionCount}} quiz questions for the target roadmap node.

Target level for all generated questions:
{{level}}

Roadmap:
{{roadmap}}

Target Roadmap Node:
{{roadmapnode}}

Return JSON array with this exact shape:
[
  {
    "questionKey": "QUIZ_A7K2M9",
    "prompt": "...",
    "type": "SingleChoice",
    "scoreWeight": 1,
    "orderNo": 1,
    "isRequired": true,
    "options": [
      {
        "valueKey": "A",
        "displayText": "...",
        "isCorrect": false,
        "scoreValue": 0,
        "orderNo": 1
      }
    ]
  }
]

Rules for questionKey:
- must be unique for every question in the response
- must be random-looking
- must not be sequential
- must not repeat existing examples exactly

Rules for difficulty:
- orderNo must start from 1 and increase continuously.
- difficulty must increase with orderNo.
- keep scoreWeight non-decreasing from first to last question.
- overall complexity must align with the target level.

Scope validation before output:
- Every question must be directly traceable to target node title/description/difficulty.
- Remove any question that could belong to the roadmap in general but not specifically to the target node.
""";

            var llm = _llmRouter.Resolve(LlmTask.GenerateStudyPlan);
            return await llm.AskAsync(systemPrompt, userPrompt, ct);
        }

        public async Task<string> AskAsync(string question, CancellationToken ct = default)
        {
           // var context = string.Join("\n---\n", hits.Select(h => h.Text));

            var relevantDocs = string.Empty;
            var systemPrompt = "You are a helpful AI assistant.";
            var userPromptWithContext = $"QUESTION:\n{question}";
            var llmChatProvider = _llmRouter.Resolve(LlmTask.SimpleChat);
            var response = await llmChatProvider.AskAsync(systemPrompt, userPromptWithContext, ct);
            return response;
        }
    }
}
