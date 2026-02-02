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

        public async Task<string> GenerateRoadmapAsync(string question, CancellationToken ct = default)
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
    "subjectId": 3,
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
            var userPromptWithContext = $"QUESTION:\n{question}";

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
{context}
ROADMAP:
{
  "roadmap": {
    "subjectId": 1,
    "title": "Mobile Developer Roadmap",
    "description": "A comprehensive learning path for aspiring mobile developers to gain essential skills and practical experience within 3 months."
  },
  "nodes": [
    {
      "id": -1,
      "title": "Introduction to Mobile Development",
      "description": "Understanding the basics of mobile development, including platforms and languages.",
      "difficulty": "Beginner",
      "orderNo": 1,
      "resources": [
        {
          "title": "Mobile App Development Overview",
          "url": "https://www.freecodecamp.org/news/mobile-app-development-overview/",
          "type": "article"
        }
      ]
    },
    {
      "id": -2,
      "title": "Learn Dart and Flutter",
      "description": "Get started with Dart programming language and Flutter framework for building mobile applications.",
      "difficulty": "Beginner",
      "orderNo": 2,
      "resources": [
        {
          "title": "Flutter Official Documentation",
          "url": "https://flutter.dev/docs",
          "type": "documentation"
        },
        {
          "title": "Dart Programming Language",
          "url": "https://dart.dev/",
          "type": "documentation"
        },
        {
          "title": "Flutter & Dart - The Complete Guide",
          "url": "https://www.udemy.com/course/flutter-dart-the-complete-guide/",
          "type": "course"
        }
      ]
    },
    {
      "id": -3,
      "title": "Building Your First Flutter App",
      "description": "Hands-on project to build a simple mobile app using Flutter.",
      "difficulty": "Beginner",
      "orderNo": 3,
      "resources": [
        {
          "title": "Creating Your First Flutter App",
          "url": "https://flutter.dev/docs/get-started/codelab",
          "type": "documentation"
        }
      ]
    },
    {
      "id": -4,
      "title": "Understanding Mobile App Architecture",
      "description": "Learn about MVC, MVVM, and other architectures used in mobile app development.",
      "difficulty": "Intermediate",
      "orderNo": 4,
      "resources": [
        {
          "title": "Intro to Mobile App Architecture",
          "url": "https://medium.com/swlh/a-beginners-guide-to-mobile-architecture-patterns-26b6334c3e32",
          "type": "article"
        }
      ]
    },
    {
      "id": -5,
      "title": "State Management in Flutter",
      "description": "Learn different state management techniques in Flutter applications.",
      "difficulty": "Intermediate",
      "orderNo": 5,
      "resources": [
        {
          "title": "State Management in Flutter",
          "url": "https://flutter.dev/docs/development/data-and-backend/state-mgmt/intro",
          "type": "documentation"
        }
      ]
    },
    {
      "id": -6,
      "title": "Working with APIs",
      "description": "Learn to connect your Flutter app to REST APIs for dynamic data.",
      "difficulty": "Intermediate",
      "orderNo": 6,
      "resources": [
        {
          "title": "Consume a RESTful API",
          "url": "https://flutter.dev/docs/cookbook/networking/fetch-data",
          "type": "documentation"
        }
      ]
    },
    {
      "id": -7,
      "title": "Testing Flutter Applications",
      "description": "Learn to write unit and integration tests for your mobile applications.",
      "difficulty": "Advanced",
      "orderNo": 7,
      "resources": [
        {
          "title": "Testing Flutter Apps",
          "url": "https://flutter.dev/docs/cookbook/testing/integration/introduction",
          "type": "documentation"
        }
      ]
    },
    {
      "id": -8,
      "title": "Publishing Your App",
      "description": "Learn the steps to publish your Flutter app on Google Play Store and Apple App Store.",
      "difficulty": "Advanced",
      "orderNo": 8,
      "resources": [
        {
          "title": "Building and Releasing an Android App",
          "url": "https://flutter.dev/docs/deployment/android",
          "type": "documentation"
        },
        {
          "title": "Building and Releasing iOS Apps",
          "url": "https://flutter.dev/docs/deployment/ios",
          "type": "documentation"
        }
      ]
    },
    {
      "id": -9,
      "title": "Portfolio Project",
      "description": "Complete a significant mobile app project to showcase your skills.",
      "difficulty": "Advanced",
      "orderNo": 9,
      "resources": [
        {
          "title": "Creating a Personal Project",
          "url": "https://medium.com/@benny.6497/building-a-personal-project-thats-not-a-tutorial-544c3d2763e0",
          "type": "article"
        }
      ]
    }
  ],
  "edges": [
    {
      "fromNodeId": -1,
      "toNodeId": -2,
      "edgeType": "Prerequisite",
      "orderNo": 1
    },
    {
      "fromNodeId": -2,
      "toNodeId": -3,
      "edgeType": "Prerequisite",
      "orderNo": 2
    },
    {
      "fromNodeId": -3,
      "toNodeId": -4,
      "edgeType": "Prerequisite",
      "orderNo": 3
    },
    {
      "fromNodeId": -4,
      "toNodeId": -5,
      "edgeType": "Prerequisite",
      "orderNo": 4
    },
    {
      "fromNodeId": -5,
      "toNodeId": -6,
      "edgeType": "Prerequisite",
      "orderNo": 5
    },
    {
      "fromNodeId": -6,
      "toNodeId": -7,
      "edgeType": "Prerequisite",
      "orderNo": 6
    },
    {
      "fromNodeId": -7,
      "toNodeId": -8,
      "edgeType": "Prerequisite",
      "orderNo": 7
    },
    {
      "fromNodeId": -8,
      "toNodeId": -9,
      "edgeType": "Prerequisite",
      "orderNo": 8
    }
  ]
}

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
