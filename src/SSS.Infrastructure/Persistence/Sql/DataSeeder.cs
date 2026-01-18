using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSS.Domain.Entities.Content;
using SSS.Domain.Entities.Identity;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Persistence.Sql
{
    public class DataSeeder
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DataSeeder(AppDbContext db, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedAllAsync()
        {
            // 1. Tự động chạy Migration nếu DB chưa có cấu trúc
            await _db.Database.MigrateAsync();

            // 2. Seed Roles
            await SeedRolesAsync();

            // 3. Seed Users (Admin & Test User)
            await SeedUsersAsync();

            // 4. Seed Dữ liệu danh mục học tập (Content)
            await SeedLearningContentAsync();
        }

        private async Task SeedRolesAsync()
        {
            string[] roles = { "Admin", "User" };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            // Tạo Admin mặc định
            var adminEmail = "admin@sss.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private async Task SeedLearningContentAsync()
        {
            if (await _db.LearningCategories.AnyAsync()) return;

            // 1. Seed Categories (Phân loại rộng)
            var categories = new List<LearningCategory>
    {
        new() { Name = "Frontend Development", Description = "UI/UX, HTML, CSS, JavaScript and Frameworks", IsActive = true },
        new() { Name = "Backend Development", Description = "Server-side logic, Databases, APIs", IsActive = true },
        new() { Name = "Mobile Development", Description = "Flutter, React Native, Native Android/iOS", IsActive = true },
        new() { Name = "DevOps & Infrastructure", Description = "CI/CD, Docker, Kubernetes, Cloud", IsActive = true }
    };
            await _db.LearningCategories.AddRangeAsync(categories);
            await _db.SaveChangesAsync();

            // 2. Seed Subject (Môn học cụ thể) - Lấy Backend làm ví dụ
            var backendCat = categories.First(c => c.Name == "Backend Development");
            var dotNetSubject = new LearningSubject
            {
                CategoryId = backendCat.Id,
                Name = ".NET Ecosystem",
                Description = "Comprehensive path to becoming a .NET Expert",
                IsActive = true
            };
            await _db.LearningSubjects.AddAsync(dotNetSubject);
            await _db.SaveChangesAsync();

            // 3. Seed Roadmap (Lộ trình cho môn học đó)
            var roadmap = new Roadmap
            {
                SubjectId = dotNetSubject.Id,
                Title = ".NET Developer Roadmap 2026",
                Description = "From C# Basics to Microservices"
            };
            await _db.Roadmaps.AddAsync(roadmap);
            await _db.SaveChangesAsync();

            // 4. Seed Roadmap Nodes (Các bước trong lộ trình)
            var node1 = new RoadmapNode { RoadmapId = roadmap.Id, Title = "C# Fundamentals", OrderNo = 1, Difficulty = NodeDifficulty.Beginner };
            var node2 = new RoadmapNode { RoadmapId = roadmap.Id, Title = "ASP.NET Core Web API", OrderNo = 2, Difficulty = NodeDifficulty.Intermediate };
            var node3 = new RoadmapNode { RoadmapId = roadmap.Id, Title = "Entity Framework Core", OrderNo = 3, Difficulty = NodeDifficulty.Intermediate };

            await _db.RoadmapNodes.AddRangeAsync(node1, node2, node3);
            await _db.SaveChangesAsync();

            // 5. Seed Roadmap Edges (Mối quan hệ/mũi tên giữa các node)
            var edges = new List<RoadmapEdge>
    {
        // Node 1 -> Node 2
        new() { RoadmapId = roadmap.Id, FromNodeId = node1.Id, ToNodeId = node2.Id, EdgeType = EdgeType.Prerequisite, OrderNo = 1 },
        // Node 2 -> Node 3
        new() { RoadmapId = roadmap.Id, FromNodeId = node2.Id, ToNodeId = node3.Id, EdgeType = EdgeType.Prerequisite, OrderNo = 2 }
    };
            await _db.RoadmapEdges.AddRangeAsync(edges);

            // 6. Seed Node Contents (Tài liệu học tập bên trong mỗi node)
            var contents = new List<NodeContent>
    {
        new()
        {
            NodeId = node1.Id,
            Title = "C# Syntax and Data Types",
            ContentType = NodeContentType.Video,
            Url = "https://youtube.com/example-csharp",
            EstimatedMinutes = 45,
            OrderNo = 1,
            IsRequired = true
        },
        new()
        {
            NodeId = node1.Id,
            Title = "Object-Oriented Programming in C#",
            ContentType = NodeContentType.Article,
            Url = "https://docs.microsoft.com/csharp-oop",
            EstimatedMinutes = 30,
            OrderNo = 2,
            IsRequired = true
        },
        new()
        {
            NodeId = node2.Id,
            Title = "Dependency Injection in ASP.NET Core",
            ContentType = NodeContentType.Video,
            EstimatedMinutes = 20,
            OrderNo = 1,
            IsRequired = true
        }
    };
            await _db.NodeContents.AddRangeAsync(contents);

            await _db.SaveChangesAsync();
        }
    }
}
