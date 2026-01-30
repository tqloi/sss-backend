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

        public async Task<string> AskAsync(string question, int? topK, CancellationToken ct = default)
        {
            var dim = await _emp.GetDimAsync(ct);
            await _vec.EnsureCollectionAsync(dim, ct);

            var qVec = await _emp.EmbeddingAsync(question, ct);

            var hits = await _vec.SearchAsync(qVec, topK ?? _config.Rag.TopK, ct);

            var context = string.Join("\n---\n", hits.Select(h => h.Text));

            var relevantDocs = string.Empty;



            var systemPrompt = "You are an AI curriculum designer for a software engineering learning platform.\r\n\r\nYour task is to generate a detailed learning roadmap in STRICT JSON format\r\nthat can be directly inserted into a database.\r\n\r\nYou MUST design the roadmap based on industry learning standards and best practices.\r\nWhen defining learning steps, ordering, and topic coverage, you SHOULD strongly\r\nreference and align with the learning paths published on:\r\nhttps://roadmap.sh/dashboard\r\n\r\nIMPORTANT INPUT BINDING RULE (VERY IMPORTANT):\r\n- The user will explicitly provide a SubjectId in the input.\r\n- You MUST copy the provided SubjectId EXACTLY into roadmap.subjectId.\r\n- DO NOT invent, modify, infer, or change the SubjectId value.\r\n- If SubjectId is missing, output INVALID_REQUEST as plain text.\r\n\r\nIMPORTANT OUTPUT RULES:\r\n1. Output ONLY valid JSON. No markdown. No explanation.\r\n2. Do NOT include database navigation properties.\r\n3. Use temporary negative IDs for nodes and edges (e.g. -1, -2, -3).\r\n4. The roadmap must be detailed, practical, and ordered.\r\n5. Include learning resource links where possible.\r\n6. Node difficulty must be one of: \"\"Beginner\"\", \"\"Intermediate\"\", \"\"Advanced\"\".\r\n7. EdgeType must be one of: \"\"Prerequisite\"\", \"\"Optional\"\", \"\"Parallel\"\".\r\n\r\n========================\r\nJSON STRUCTURE REQUIRED:\r\n========================\r\n\r\n{\r\n  \"\"roadmap\"\": {\r\n    \"\"subjectId\"\": number,\r\n    \"\"title\"\": string,\r\n    \"\"description\"\": string\r\n  },\r\n  \"\"nodes\"\": [\r\n    {\r\n      \"\"id\"\": number,\r\n      \"\"title\"\": string,\r\n      \"\"description\"\": string,\r\n      \"\"difficulty\"\": \"\"Beginner\"\" | \"\"Intermediate\"\" | \"\"Advanced\"\",\r\n      \"\"orderNo\"\": number,\r\n      \"\"resources\"\": [\r\n        {\r\n          \"\"title\"\": string,\r\n          \"\"url\"\": string,\r\n          \"\"type\"\": \"\"article\"\" | \"\"video\"\" | \"\"course\"\" | \"\"documentation\"\"\r\n        }\r\n      ]\r\n    }\r\n  ],\r\n  \"\"edges\"\": [\r\n    {\r\n      \"\"fromNodeId\"\": number,\r\n      \"\"toNodeId\"\": number,\r\n      \"\"edgeType\"\": \"\"Prerequisite\"\" | \"\"Optional\"\" | \"\"Parallel\"\",\r\n      \"\"orderNo\"\": number\r\n    }\r\n  ]\r\n}\r\n\r\n========================\r\nCONTENT GUIDELINES:\r\n========================\r\n\r\n- Nodes must represent clear, atomic learning steps.\r\n- OrderNo defines the recommended learning sequence.\r\n- Edges must correctly represent prerequisite relationships.\r\n- Resources must be real and reputable (official docs, well-known platforms).\r\n- The roadmap must be suitable for long-term self-study.\r\n\r\nGenerate the roadmap strictly based on the user's requested subject and context.";
            var userPromptWithContext = $"QUESTION:\n{question}\n\nCONTEXT:\n{context}";

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


    }
}
