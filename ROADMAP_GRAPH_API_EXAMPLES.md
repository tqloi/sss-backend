# Roadmap Graph API - Sample curl Examples

## POST /api/roadmaps/graph - Create Full Graph

Creates a complete roadmap graph from scratch using clientId references.

```bash
curl -X POST "https://your-api.com/api/roadmaps/graph" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
  "roadmap": {
    "subjectId": 1,
    "title": "Complete React Learning Path",
    "description": "Master React from basics to advanced concepts"
  },
  "nodes": [
    {
      "clientId": "node-1",
      "title": "JavaScript Fundamentals",
      "description": "Learn core JavaScript concepts",
      "difficulty": "Beginner",
      "orderNo": 1
    },
    {
      "clientId": "node-2",
      "title": "React Basics",
      "description": "Introduction to React components and props",
      "difficulty": "Intermediate",
      "orderNo": 2
    },
    {
      "clientId": "node-3",
      "title": "React Hooks",
      "description": "Master useState, useEffect, and custom hooks",
      "difficulty": "Intermediate",
      "orderNo": 3
    },
    {
      "clientId": "node-4",
      "title": "Advanced React Patterns",
      "description": "Learn compound components, render props, HOCs",
      "difficulty": "Advanced",
      "orderNo": 4
    }
  ],
  "contents": [
    {
      "clientId": "content-1",
      "nodeClientId": "node-1",
      "contentType": "Video",
      "title": "JavaScript ES6+ Features",
      "url": "https://youtube.com/watch?v=example1",
      "description": "Learn modern JavaScript syntax",
      "estimatedMinutes": 120,
      "difficulty": "Beginner",
      "orderNo": 1,
      "isRequired": true
    },
    {
      "clientId": "content-2",
      "nodeClientId": "node-1",
      "contentType": "Article",
      "title": "Async/Await Deep Dive",
      "url": "https://blog.example.com/async-await",
      "description": "Understanding asynchronous JavaScript",
      "estimatedMinutes": 30,
      "orderNo": 2,
      "isRequired": false
    },
    {
      "clientId": "content-3",
      "nodeClientId": "node-2",
      "contentType": "Video",
      "title": "React Components Tutorial",
      "url": "https://youtube.com/watch?v=example2",
      "description": "Build your first React components",
      "estimatedMinutes": 90,
      "difficulty": "Beginner",
      "orderNo": 1,
      "isRequired": true
    },
    {
      "clientId": "content-4",
      "nodeClientId": "node-3",
      "contentType": "Document",
      "title": "React Hooks Official Docs",
      "url": "https://react.dev/hooks",
      "description": "Official React hooks documentation",
      "estimatedMinutes": 60,
      "orderNo": 1,
      "isRequired": true
    }
  ],
  "edges": [
    {
      "fromNodeClientId": "node-1",
      "toNodeClientId": "node-2",
      "edgeType": "Prerequisite",
      "orderNo": 1
    },
    {
      "fromNodeClientId": "node-2",
      "toNodeClientId": "node-3",
      "edgeType": "Next",
      "orderNo": 2
    },
    {
      "fromNodeClientId": "node-3",
      "toNodeClientId": "node-4",
      "edgeType": "Next",
      "orderNo": 3
    },
    {
      "fromNodeClientId": "node-1",
      "toNodeClientId": "node-3",
      "edgeType": "Recommended",
      "orderNo": 4
    }
  ]
}'
```

### Response Example:
```json
{
  "success": true,
  "data": {
    "roadmapId": 42,
    "nodeIdMap": {
      "node-1": 101,
      "node-2": 102,
      "node-3": 103,
      "node-4": 104
    },
    "contentIdMap": {
      "content-1": 201,
      "content-2": 202,
      "content-3": 203,
      "content-4": 204
    },
    "summary": {
      "nodesCount": 4,
      "edgesCount": 4,
      "contentsCount": 4
    }
  }
}
```

---

## PUT /api/roadmaps/{id}/graph - Sync/Update Full Graph

Updates an existing roadmap graph. Payload represents the FINAL desired state.
Server will add/update/delete to match the payload (SYNC semantics).

### Scenario: Update existing graph with mixed operations
- Update existing nodes (using id)
- Add new node (using clientId)
- Remove a node (by omitting it from payload)
- Update content and add new content
- Modify edges

```bash
curl -X PUT "https://your-api.com/api/roadmaps/42/graph" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
  "roadmap": {
    "title": "Complete React Learning Path - Updated",
    "description": "Master React from basics to advanced concepts with Redux"
  },
  "nodes": [
    {
      "id": 101,
      "title": "JavaScript Fundamentals - Enhanced",
      "description": "Learn core JavaScript concepts with ES2023 features",
      "difficulty": "Beginner",
      "orderNo": 1
    },
    {
      "id": 102,
      "title": "React Basics",
      "description": "Introduction to React components and props",
      "difficulty": "Intermediate",
      "orderNo": 2
    },
    {
      "id": 103,
      "title": "React Hooks",
      "description": "Master useState, useEffect, and custom hooks",
      "difficulty": "Intermediate",
      "orderNo": 3
    },
    {
      "clientId": "node-new-1",
      "title": "Redux State Management",
      "description": "Learn Redux for complex state management",
      "difficulty": "Advanced",
      "orderNo": 5
    }
  ],
  "contents": [
    {
      "id": 201,
      "nodeId": 101,
      "contentType": "Video",
      "title": "JavaScript ES6+ Features - Updated",
      "url": "https://youtube.com/watch?v=example1-v2",
      "description": "Learn modern JavaScript syntax including ES2023",
      "estimatedMinutes": 150,
      "difficulty": "Beginner",
      "orderNo": 1,
      "isRequired": true
    },
    {
      "id": 203,
      "nodeId": 102,
      "contentType": "Video",
      "title": "React Components Tutorial",
      "url": "https://youtube.com/watch?v=example2",
      "description": "Build your first React components",
      "estimatedMinutes": 90,
      "difficulty": "Beginner",
      "orderNo": 1,
      "isRequired": true
    },
    {
      "id": 204,
      "nodeId": 103,
      "contentType": "Document",
      "title": "React Hooks Official Docs",
      "url": "https://react.dev/hooks",
      "description": "Official React hooks documentation",
      "estimatedMinutes": 60,
      "orderNo": 1,
      "isRequired": true
    },
    {
      "clientId": "content-new-1",
      "nodeClientId": "node-new-1",
      "contentType": "Video",
      "title": "Redux Fundamentals",
      "url": "https://youtube.com/watch?v=redux-basics",
      "description": "Learn Redux basics and middleware",
      "estimatedMinutes": 120,
      "difficulty": "Advanced",
      "orderNo": 1,
      "isRequired": true
    }
  ],
  "edges": [
    {
      "fromNodeId": 101,
      "toNodeId": 102,
      "edgeType": "Prerequisite",
      "orderNo": 1
    },
    {
      "fromNodeId": 102,
      "toNodeId": 103,
      "edgeType": "Next",
      "orderNo": 2
    },
    {
      "fromNodeId": 103,
      "fromNodeClientId": "node-new-1",
      "edgeType": "Next",
      "orderNo": 3
    }
  ]
}'
```

### Response Example:
```json
{
  "success": true,
  "data": {
    "roadmapId": 42,
    "nodeIdMap": {
      "node-new-1": 105
    },
    "contentIdMap": {
      "content-new-1": 205
    },
    "summary": {
      "nodesCount": 4,
      "edgesCount": 3,
      "contentsCount": 4
    }
  }
}
```

### What happened in this sync:
1. **Updated**: Node 101 title and description changed
2. **Kept**: Nodes 102, 103 unchanged (still in payload)
3. **Added**: New node 105 (from "node-new-1" clientId)
4. **Deleted**: Node 104 (not in payload anymore)
5. **Updated**: Content 201 with new video URL and duration
6. **Deleted**: Content 202 (not in payload)
7. **Added**: New content 205 for Redux node
8. **Edges**: Recalculated based on new node structure

---

## Edge Reference Resolution Examples

### Using existing node IDs:
```json
{
  "fromNodeId": 101,
  "toNodeId": 102,
  "edgeType": "Prerequisite"
}
```

### Using clientIds for new nodes:
```json
{
  "fromNodeClientId": "node-1",
  "toNodeClientId": "node-2",
  "edgeType": "Next"
}
```

### Mixed: existing node to new node:
```json
{
  "fromNodeId": 101,
  "toNodeClientId": "node-new-1",
  "edgeType": "Recommended"
}
```

---

## Edge Types

- **Prerequisite**: Must complete FROM before TO (hard requirement)
- **Recommended**: Suggested to complete FROM before TO (soft requirement)
- **Next**: Natural progression from FROM to TO

---

## Content Types

Available content types in the system:
- `Video`
- `Article`
- `Document`
- `Exercise`
- `Quiz`
- `Project`

---

## Node Difficulty Levels

- `Beginner`
- `Intermediate`
- `Advanced`
- `Expert`

---

## Important Notes

1. **ClientId Usage**: 
   - Use clientIds for NEW items (without database ID yet)
   - ClientIds must be unique within each collection in payload
   - Server returns mapping of clientId -> dbId in response

2. **Sync Semantics (PUT)**:
   - Payload = final desired state
   - Items in DB but NOT in payload = DELETED
   - Items with ID = UPDATED
   - Items with clientId only = CREATED

3. **Validation**:
   - No self-loop edges allowed
   - No duplicate edges (same from/to/type combination)
   - Content (node, orderNo) must be unique
   - All node/content references must resolve

4. **Transaction Safety**:
   - All operations in one transaction
   - Rollback on any error
   - Maintain referential integrity

5. **Delete Order** (for sync):
   - Edges deleted first
   - Then contents
   - Then nodes (to satisfy FK constraints)
