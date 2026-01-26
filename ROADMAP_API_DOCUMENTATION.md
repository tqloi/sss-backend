# Roadmap Management API Documentation

## Overview
Complete REST API for managing learning roadmaps with nodes, edges, and content. All endpoints require Admin role authentication.

---

## 📋 Roadmap APIs

### 1. Create Roadmap
**POST** `/roadmaps`

Creates a new roadmap associated with a learning subject.

```bash
curl -X POST https://api.example.com/roadmaps \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "subjectId": 1,
    "title": "Full Stack Web Development Roadmap",
    "description": "Complete path to becoming a full-stack developer"
  }'
```

**Response (201):**
```json
{
  "success": true,
  "message": "Roadmap created successfully.",
  "data": {
    "id": 1,
    "subjectId": 1,
    "title": "Full Stack Web Development Roadmap",
    "description": "Complete path to becoming a full-stack developer"
  }
}
```

---

### 2. Get All Roadmaps (Paginated)
**GET** `/roadmaps?pageIndex=1&pageSize=10&subjectId=1&q=web&status=active`

Lists roadmaps with optional filtering by subject, search query, and status.

```bash
curl -X GET "https://api.example.com/roadmaps?pageIndex=1&pageSize=10&subjectId=1&q=web" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200):**
```json
{
  "roadmaps": {
    "pageIndex": 1,
    "pageSize": 10,
    "totalCount": 1,
    "totalPages": 1,
    "hasPreviousPage": false,
    "hasNextPage": false,
    "items": [
      {
        "id": 1,
        "subjectId": 1,
        "title": "Full Stack Web Development Roadmap",
        "description": "Complete path to becoming a full-stack developer",
        "status": null
      }
    ]
  }
}
```

---

### 3. Get Roadmap Graph
**GET** `/roadmaps/{roadmapId}`

Returns roadmap with all nodes and edges (no content details).

```bash
curl -X GET https://api.example.com/roadmaps/1 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200):**
```json
{
  "success": true,
  "message": "Roadmap retrieved successfully.",
  "data": {
    "roadmap": {
      "id": 1,
      "subjectId": 1,
      "title": "Full Stack Web Development Roadmap",
      "description": "Complete path to becoming a full-stack developer"
    },
    "nodes": [
      {
        "id": 1,
        "roadmapId": 1,
        "title": "HTML & CSS Basics",
        "description": "Learn fundamental web technologies",
        "difficulty": "Beginner",
        "orderNo": 1
      },
      {
        "id": 2,
        "roadmapId": 1,
        "title": "JavaScript Fundamentals",
        "description": "Core JavaScript concepts",
        "difficulty": "Beginner",
        "orderNo": 2
      }
    ],
    "edges": [
      {
        "id": 1,
        "roadmapId": 1,
        "fromNodeId": 1,
        "toNodeId": 2,
        "edgeType": "Prerequisite",
        "orderNo": 1
      }
    ]
  }
}
```

---

### 4. Update Roadmap
**PATCH** `/roadmaps/{id}`

Partial update of roadmap metadata.

```bash
curl -X PATCH https://api.example.com/roadmaps/1 \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Updated Full Stack Roadmap 2026",
    "description": "Updated description"
  }'
```

**Response (200):**
```json
{
  "success": true,
  "message": "Roadmap updated successfully.",
  "data": {
    "id": 1,
    "subjectId": 1,
    "title": "Updated Full Stack Roadmap 2026",
    "description": "Updated description"
  }
}
```

---

### 5. Delete Roadmap
**DELETE** `/roadmaps/{roadmapId}`

Hard deletes roadmap and all related nodes, edges, and content.

```bash
curl -X DELETE https://api.example.com/roadmaps/1 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (204):** No content

---

## 🔷 RoadmapNode APIs

### 6. Create Node
**POST** `/roadmaps/{roadmapId}/nodes`

Creates a new node in the roadmap.

```bash
curl -X POST https://api.example.com/roadmaps/1/nodes \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "roadmapId": 1,
    "title": "React Basics",
    "description": "Introduction to React library",
    "difficulty": "Intermediate",
    "orderNo": 3
  }'
```

**Response (201):**
```json
{
  "success": true,
  "message": "Roadmap node created successfully.",
  "data": {
    "id": 3,
    "roadmapId": 1,
    "title": "React Basics",
    "description": "Introduction to React library",
    "difficulty": "Intermediate",
    "orderNo": 3
  }
}
```

---

### 7. Update Node
**PATCH** `/roadmaps/{roadmapId}/nodes/{nodeId}`

Partial update of node properties.

```bash
curl -X PATCH https://api.example.com/roadmaps/1/nodes/3 \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "roadmapId": 1,
    "nodeId": 3,
    "title": "React Fundamentals",
    "difficulty": "Advanced"
  }'
```

**Response (200):**
```json
{
  "success": true,
  "message": "Roadmap node updated successfully.",
  "data": {
    "id": 3,
    "roadmapId": 1,
    "title": "React Fundamentals",
    "description": "Introduction to React library",
    "difficulty": "Advanced",
    "orderNo": 3
  }
}
```

---

### 8. Delete Node
**DELETE** `/roadmaps/{roadmapId}/nodes/{nodeId}`

Hard deletes node and all related content and edges.

```bash
curl -X DELETE https://api.example.com/roadmaps/1/nodes/3 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (204):** No content

---

## 🔗 RoadmapEdge APIs

### 9. Create Edge
**POST** `/roadmaps/{roadmapId}/edges`

Creates a new edge with validations:
- No self-loops (fromNodeId ≠ toNodeId)
- Both nodes must exist in the same roadmap
- No duplicates (same fromNodeId, toNodeId, edgeType)

```bash
curl -X POST https://api.example.com/roadmaps/1/edges \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "roadmapId": 1,
    "fromNodeId": 2,
    "toNodeId": 3,
    "edgeType": "Prerequisite",
    "orderNo": 1
  }'
```

**EdgeType values:** `Prerequisite`, `Recommended`, `Next`

**Response (201):**
```json
{
  "success": true,
  "message": "Roadmap edge created successfully.",
  "data": {
    "id": 2,
    "roadmapId": 1,
    "fromNodeId": 2,
    "toNodeId": 3,
    "edgeType": "Prerequisite",
    "orderNo": 1
  }
}
```

---

### 10. Update Edge
**PATCH** `/roadmaps/{roadmapId}/edges/{edgeId}`

Updates edge type or order number.

```bash
curl -X PATCH https://api.example.com/roadmaps/1/edges/2 \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "roadmapId": 1,
    "edgeId": 2,
    "edgeType": "Recommended",
    "orderNo": 2
  }'
```

**Response (200):**
```json
{
  "success": true,
  "message": "Roadmap edge updated successfully.",
  "data": {
    "id": 2,
    "roadmapId": 1,
    "fromNodeId": 2,
    "toNodeId": 3,
    "edgeType": "Recommended",
    "orderNo": 2
  }
}
```

---

### 11. Delete Edge
**DELETE** `/roadmaps/{roadmapId}/edges/{edgeId}`

Hard deletes edge.

```bash
curl -X DELETE https://api.example.com/roadmaps/1/edges/2 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (204):** No content

---

### 12. Bulk Sync Edges ⭐
**PUT** `/roadmaps/{roadmapId}/edges/bulk`

Synchronizes edges list: adds new, updates existing, deletes removed.

**Strategy:** Uses (fromNodeId, toNodeId, edgeType) as identity for matching.

```bash
curl -X PUT https://api.example.com/roadmaps/1/edges/bulk \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "roadmapId": 1,
    "edges": [
      {
        "fromNodeId": 1,
        "toNodeId": 2,
        "edgeType": "Prerequisite",
        "orderNo": 1
      },
      {
        "fromNodeId": 2,
        "toNodeId": 3,
        "edgeType": "Next",
        "orderNo": 2
      },
      {
        "fromNodeId": 1,
        "toNodeId": 3,
        "edgeType": "Recommended",
        "orderNo": 3
      }
    ]
  }'
```

**Response (200):**
```json
{
  "success": true,
  "message": "Bulk sync completed: 2 added, 1 updated, 1 deleted.",
  "data": [
    {
      "id": 1,
      "roadmapId": 1,
      "fromNodeId": 1,
      "toNodeId": 2,
      "edgeType": "Prerequisite",
      "orderNo": 1
    },
    {
      "id": 3,
      "roadmapId": 1,
      "fromNodeId": 2,
      "toNodeId": 3,
      "edgeType": "Next",
      "orderNo": 2
    },
    {
      "id": 4,
      "roadmapId": 1,
      "fromNodeId": 1,
      "toNodeId": 3,
      "edgeType": "Recommended",
      "orderNo": 3
    }
  ]
}
```

---

## 📄 NodeContent APIs

### 13. Create Content
**POST** `/roadmaps/{roadmapId}/nodes/{nodeId}/contents`

Adds content to a node.

```bash
curl -X POST https://api.example.com/roadmaps/1/nodes/1/contents \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "roadmapId": 1,
    "nodeId": 1,
    "contentType": "Video",
    "title": "HTML Crash Course",
    "url": "https://youtube.com/watch?v=abc123",
    "description": "Quick 30-minute intro to HTML",
    "estimatedMinutes": 30,
    "difficulty": "Beginner",
    "orderNo": 1,
    "isRequired": true
  }'
```

**ContentType values:** `Video`, `Article`, `Book`, `Course`, `Exercise`, `Quiz`, `Project`

**Response (201):**
```json
{
  "success": true,
  "message": "Node content created successfully.",
  "data": {
    "id": 1,
    "nodeId": 1,
    "contentType": "Video",
    "title": "HTML Crash Course",
    "url": "https://youtube.com/watch?v=abc123",
    "description": "Quick 30-minute intro to HTML",
    "estimatedMinutes": 30,
    "difficulty": "Beginner",
    "orderNo": 1,
    "isRequired": true
  }
}
```

---

### 14. Get Node Contents
**GET** `/roadmaps/{roadmapId}/nodes/{nodeId}/contents`

Returns all content for a node, ordered by OrderNo.

```bash
curl -X GET https://api.example.com/roadmaps/1/nodes/1/contents \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200):**
```json
{
  "success": true,
  "message": "Node contents retrieved successfully.",
  "data": [
    {
      "id": 1,
      "nodeId": 1,
      "contentType": "Video",
      "title": "HTML Crash Course",
      "url": "https://youtube.com/watch?v=abc123",
      "description": "Quick 30-minute intro to HTML",
      "estimatedMinutes": 30,
      "difficulty": "Beginner",
      "orderNo": 1,
      "isRequired": true
    },
    {
      "id": 2,
      "nodeId": 1,
      "contentType": "Article",
      "title": "HTML Best Practices",
      "url": "https://example.com/html-best-practices",
      "description": null,
      "estimatedMinutes": 15,
      "difficulty": "Beginner",
      "orderNo": 2,
      "isRequired": false
    }
  ]
}
```

---

### 15. Update Content
**PATCH** `/roadmaps/{roadmapId}/nodes/{nodeId}/contents/{contentId}`

Partial update of content properties.

```bash
curl -X PATCH https://api.example.com/roadmaps/1/nodes/1/contents/1 \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "roadmapId": 1,
    "nodeId": 1,
    "contentId": 1,
    "title": "HTML5 Complete Course",
    "estimatedMinutes": 45,
    "isRequired": true
  }'
```

**Response (200):**
```json
{
  "success": true,
  "message": "Node content updated successfully.",
  "data": {
    "id": 1,
    "nodeId": 1,
    "contentType": "Video",
    "title": "HTML5 Complete Course",
    "url": "https://youtube.com/watch?v=abc123",
    "description": "Quick 30-minute intro to HTML",
    "estimatedMinutes": 45,
    "difficulty": "Beginner",
    "orderNo": 1,
    "isRequired": true
  }
}
```

---

### 16. Delete Content
**DELETE** `/roadmaps/{roadmapId}/nodes/{nodeId}/contents/{contentId}`

Hard deletes content.

```bash
curl -X DELETE https://api.example.com/roadmaps/1/nodes/1/contents/1 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (204):** No content

---

## 🔒 Authentication

All endpoints require Admin role. Include JWT token in Authorization header:
```
Authorization: Bearer YOUR_JWT_TOKEN
```

---

## ✅ Validation Rules

### Edge Validations:
- ✓ FromNodeId and ToNodeId must exist
- ✓ Both nodes must belong to the same roadmap
- ✓ No self-loops: FromNodeId ≠ ToNodeId
- ✓ No duplicates: Unique per (RoadmapId, FromNodeId, ToNodeId, EdgeType)
- ✓ EdgeType must be: `Prerequisite`, `Recommended`, or `Next`

### Content Validations:
- ✓ NodeId must belong to specified RoadmapId
- ✓ Unique OrderNo per NodeId (enforced by DB constraint)
- ✓ Title required, max 400 characters
- ✓ URL max 2048 characters
- ✓ ContentType must be valid enum value

### Node Validations:
- ✓ RoadmapId must exist
- ✓ Title required, max 300 characters
- ✓ Description max 2000 characters

---

## 🗑️ Delete Cascade Behavior

- **Delete Roadmap** → Deletes all Nodes, Edges, and Contents
- **Delete Node** → Deletes all Contents and Edges referencing it
- **Delete Edge** → No cascade
- **Delete Content** → No cascade

---

## 📊 Database Constraints

### Unique Indexes:
- `RoadmapEdges`: (RoadmapId, FromNodeId, ToNodeId, EdgeType)
- `NodeContents`: (NodeId, OrderNo)

### Regular Indexes:
- `RoadmapNodes`: (RoadmapId, OrderNo)

---

## 🎯 Use Cases

### 1. Create Complete Roadmap Flow:
```bash
# 1. Create roadmap
POST /roadmaps

# 2. Add nodes
POST /roadmaps/1/nodes
POST /roadmaps/1/nodes
POST /roadmaps/1/nodes

# 3. Define relationships (bulk)
PUT /roadmaps/1/edges/bulk

# 4. Add learning content
POST /roadmaps/1/nodes/1/contents
POST /roadmaps/1/nodes/1/contents
```

### 2. Update Roadmap Structure:
```bash
# Sync edges with new structure
PUT /roadmaps/1/edges/bulk

# Update node details
PATCH /roadmaps/1/nodes/2
```

### 3. View Roadmap:
```bash
# Get graph structure
GET /roadmaps/1

# Get content for specific node
GET /roadmaps/1/nodes/1/contents
```

---

## 📝 Notes

- All timestamps use ISO 8601 format
- IDs are 64-bit integers (long)
- Pagination defaults: page=1, pageSize=10, max=100
- All operations are transactional
- Bulk sync uses (FromNodeId, ToNodeId, EdgeType) as identity
