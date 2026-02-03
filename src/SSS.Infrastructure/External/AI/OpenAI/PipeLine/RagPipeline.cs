using SSS.Application.Abstractions.External.AI;
using SSS.Application.Abstractions.External.AI.Embedding;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.External.AI.Vector;

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

        public async Task IngestAsync(string userId, IEnumerable<(string Text, string? Source)> chunks, CancellationToken ct = default)
        {
            int dim = await _emp.GetDimAsync(ct);
            await _vec.EnsureCollectionAsync(dim, ct);

            var list = new List<VectorPoint>();
            foreach (var (text, source) in chunks)
            {
                var vec = await _emp.EmbeddingAsync(text, ct);
                list.Add(new VectorPoint(Guid.NewGuid().ToString("N"), vec, text, source, userId,
            DataType: source ?? "unknown",
            CreatedAt: DateTime.UtcNow));

            }
            await _vec.UpsertAsync(list, ct);

        }
        //public async Task<string> AskAsync(string question, int? topK, CancellationToken ct = default)
        //{
        //    var dim = await _emp.GetDimAsync(ct);
        //    await _vec.EnsureCollectionAsync(dim, ct);

        //    var qVec = await _emp.EmbeddingAsync(question, ct);

        //    var hits = await _vec.SearchAsync(qVec, topK ?? _config.Rag.TopK, ct);

            
        //}

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
- Video
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
  - Article / Video for theory
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
      "contentType": "Video" | "Article" | "Book" | "Course" | "Exercise" | "Quiz" | "Project",
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
            CancellationToken ct = default)
        {
            // 1. Vector đại diện cho "tạo study plan"
            var query = "User survey information describing learning orientation, target role, experience level, and preferred roadmap.";
            var queryVector = await _emp.EmbeddingAsync(query, ct);

            // 2. Lấy surveys của user
            var hits = await _vec.SearchByUserId(
                vector: queryVector,
                topK: 5,
                userId: userId,
                dataType: "user_surveys",
                ct: ct);

            // 3. Ghép context
            var context = string.Join(
                "\n---\n",
                hits.Select(h => h.Text));

            return context;
        }

        public async Task<string> GenerateStudyPlanAsync(
    string userId,
    object roadmap,
    CancellationToken ct = default)
        {
            // 1. Build context từ vector DB
            var context = await BuildStudyPlanContextAsync(userId, ct);
            Console.WriteLine(context);

            // 2. System prompt chuyên cho study plan
            var systemPrompt = """
You are a senior learning system architect and backend-aware AI.

Your task is to generate a PERSONALIZED STUDY PLAN
that can be DIRECTLY DESERIALIZED into backend domain entities.

CRITICAL RULES (MUST FOLLOW):
1. Output MUST be a SINGLE valid JSON object
2. Do NOT include explanations, comments, markdown, or extra text
3. Do NOT modify, reorder, or create new roadmap nodes
4. Use ONLY roadmap nodes that are provided
5. The roadmap defines WHAT exists — you decide PRIORITY and SCHEDULING only
6. All decisions MUST be driven strictly by the user's survey context
7. If information is missing, make a reasonable inference — DO NOT ask questions

--------------------------------------------------
OUTPUT SCHEMA (STRICT)
--------------------------------------------------

{
  "studyPlan": {
    "roadmapId": number,
    "profileVersion": number,
    "strategy": "Balanced | Intensive | Light",
    "status": "Draft | Active",
    "modules": [
      {
        "roadmapNodeId": number,
        "status": "NotStarted | InProgress | Completed",
        "tasks": [
          {
            "title": string,
            "scheduledDate": "YYYY-MM-DD",
            "status": "Planned"
          }
        ]
      }
    ]
  }
}

--------------------------------------------------
ENTITY MAPPING GUARANTEE
--------------------------------------------------

- studyPlan → StudyPlan
- studyPlan.modules[] → StudyPlanModule
- modules[].tasks[] → TaskItem

Field mapping:
- roadmapId            → StudyPlan.RoadmapId
- profileVersion       → StudyPlan.ProfileVersion
- strategy             → StudyPlan.Strategy
- status               → StudyPlan.Status
- roadmapNodeId        → StudyPlanModule.RoadmapNodeId
- modules[].status     → StudyPlanModule.Status
- tasks[].title        → TaskItem.Title
- tasks[].scheduledDate→ TaskItem.ScheduledDate
- tasks[].status       → TaskItem.Status

--------------------------------------------------
TASK DESIGN RULES
--------------------------------------------------

- Each module MUST have 2–5 tasks
- Tasks must be concrete and actionable (study, build, review, test…)
- Scheduled dates MUST be realistic and progressive
- Earlier roadmap nodes should be scheduled earlier
- Difficulty affects task density and pacing
""";
            var userPrompt = $$"""
USER SURVEY CONTEXT:
${{context}}
ROADMAP:
${{roadmap}}
--------------------------------------------------
GOAL
--------------------------------------------------

Produce a backend-ready personalized study plan
that fits the user's level, goals, and availability
while strictly respecting the provided roadmap.
""";

            var llmChatProvider = _llmRouter.Resolve(LlmTask.GenerateStudyPlan);
            // 3. Gọi GPT provider
            var response = await llmChatProvider.AskAsync(
                systemPrompt,
                userPrompt,
                ct);

            return response;
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
