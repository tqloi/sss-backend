using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SSS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_Surveys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_Surveys", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ct_LearningCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_LearningCategories", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Id_Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id_Roles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Id_Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false),
                    LastName = table.Column<string>(type: "longtext", nullable: true),
                    AvatarUrl = table.Column<string>(type: "varchar(512)", nullable: true),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    Dob = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "longtext", nullable: true),
                    SubscriptionType = table.Column<string>(type: "longtext", nullable: true),
                    SubscriptionStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SubscriptionEndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ln_UserLearningBehaviors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    AvailableDaysJson = table.Column<string>(type: "json", nullable: true),
                    PreferredTimeBlocksJson = table.Column<string>(type: "json", nullable: true),
                    SessionLengthPrefMinutes = table.Column<int>(type: "int", nullable: true),
                    WVisual = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false),
                    WReading = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false),
                    WPractice = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false),
                    DisciplineType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CommonDifficultiesJson = table.Column<string>(type: "json", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ln_UserLearningBehaviors", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ln_UserLearningTargets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ProfileVersion = table.Column<int>(type: "int", nullable: false),
                    TargetRole = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CurrentLevel = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TargetDeadlineMonths = table.Column<int>(type: "int", nullable: true),
                    GoalDescription = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ln_UserLearningTargets", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_SurveyQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SurveyId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionKey = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Prompt = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    ScaleMin = table.Column<int>(type: "int", nullable: true),
                    ScaleMax = table.Column<int>(type: "int", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_SurveyQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_SurveyQuestions_As_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "As_Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ct_LearningSubjects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_LearningSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ct_LearningSubjects_Ct_LearningCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Ct_LearningCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Id_RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id_RoleClaims_Id_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Id_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_SurveyResponses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SurveyId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    TriggerReason = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SnapshotJson = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_SurveyResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_SurveyResponses_As_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "As_Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_As_SurveyResponses_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Id_RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    TokenHash = table.Column<string>(type: "char(64)", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByIp = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    RevokedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RevokedByIp = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    ReplacedByTokenId = table.Column<Guid>(type: "char(36)", nullable: true),
                    IsUsed = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id_RefreshTokens_Id_RefreshTokens_ReplacedByTokenId",
                        column: x => x.ReplacedByTokenId,
                        principalTable: "Id_RefreshTokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Id_RefreshTokens_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Id_UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id_UserClaims_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Id_UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_Id_UserLogins_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Id_UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Id_UserRoles_Id_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Id_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Id_UserRoles_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Id_UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_Id_UserTokens_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Nt_UserPushTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    DeviceToken = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false),
                    DeviceType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nt_UserPushTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nt_UserPushTokens_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tr_UserBehaviorWindows",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    WindowStart = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WindowEnd = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AvgFocusScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ActiveRatio = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    AvgSessionLengthMinutes = table.Column<int>(type: "int", nullable: true),
                    CompletionRate = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    ComputedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tr_UserBehaviorWindows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tr_UserBehaviorWindows_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tr_UserGamification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    CurrentStreak = table.Column<int>(type: "int", nullable: true),
                    LongestStreak = table.Column<int>(type: "int", nullable: true),
                    LastActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    TotalExp = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tr_UserGamification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tr_UserGamification_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_SurveyQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    ValueKey = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DisplayText = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(6,4)", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    AllowFreeText = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_SurveyQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_SurveyQuestionOptions_As_SurveyQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "As_SurveyQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ct_Roadmaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_Roadmaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ct_Roadmaps_Ct_LearningSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Ct_LearningSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tr_UserSubjectStats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false),
                    ProficiencyLevel = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    TotalHoursSpent = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    WeakNodeIds = table.Column<string>(type: "json", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tr_UserSubjectStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tr_UserSubjectStats_Ct_LearningSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Ct_LearningSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tr_UserSubjectStats_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_SurveyAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ResponseId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    OptionId = table.Column<long>(type: "bigint", nullable: true),
                    NumberValue = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    TextValue = table.Column<string>(type: "text", nullable: true),
                    AnsweredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_SurveyAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_SurveyAnswers_As_SurveyQuestionOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "As_SurveyQuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_As_SurveyAnswers_As_SurveyQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "As_SurveyQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_As_SurveyAnswers_As_SurveyResponses_ResponseId",
                        column: x => x.ResponseId,
                        principalTable: "As_SurveyResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ct_RoadmapNodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoadmapId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Difficulty = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_RoadmapNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ct_RoadmapNodes_Ct_Roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "Ct_Roadmaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pl_StudyPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoadmapId = table.Column<long>(type: "bigint", nullable: false),
                    ProfileVersion = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Strategy = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pl_StudyPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pl_StudyPlans_Ct_Roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "Ct_Roadmaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pl_StudyPlans_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_Quizzes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoadmapNodeId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TotalScore = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_Quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_Quizzes_Ct_RoadmapNodes_RoadmapNodeId",
                        column: x => x.RoadmapNodeId,
                        principalTable: "Ct_RoadmapNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ct_NodeContents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NodeId = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Title = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false),
                    Url = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EstimatedMinutes = table.Column<int>(type: "int", nullable: true),
                    Difficulty = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_NodeContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ct_NodeContents_Ct_RoadmapNodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Ct_RoadmapNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ct_RoadmapEdges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoadmapId = table.Column<long>(type: "bigint", nullable: false),
                    FromNodeId = table.Column<long>(type: "bigint", nullable: false),
                    ToNodeId = table.Column<long>(type: "bigint", nullable: false),
                    EdgeType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_RoadmapEdges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ct_RoadmapEdges_Ct_RoadmapNodes_FromNodeId",
                        column: x => x.FromNodeId,
                        principalTable: "Ct_RoadmapNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ct_RoadmapEdges_Ct_RoadmapNodes_ToNodeId",
                        column: x => x.ToNodeId,
                        principalTable: "Ct_RoadmapNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ct_RoadmapEdges_Ct_Roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "Ct_Roadmaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pl_StudyPlanModules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StudyPlanId = table.Column<long>(type: "bigint", nullable: false),
                    RoadmapNodeId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pl_StudyPlanModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pl_StudyPlanModules_Ct_RoadmapNodes_RoadmapNodeId",
                        column: x => x.RoadmapNodeId,
                        principalTable: "Ct_RoadmapNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pl_StudyPlanModules_Pl_StudyPlans_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "Pl_StudyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_QuizAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    QuizId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_QuizAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_QuizAttempts_As_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "As_Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_As_QuizAttempts_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    QuizId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionKey = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Prompt = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    ScoreWeight = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 1.0m),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_QuizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_QuizQuestions_As_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "As_Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pl_TaskItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StudyPlanModuleId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    EstimatedDurationSeconds = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "date", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pl_TaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pl_TaskItems_Pl_StudyPlanModules_StudyPlanModuleId",
                        column: x => x.StudyPlanModuleId,
                        principalTable: "Pl_StudyPlanModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_QuizQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    ValueKey = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DisplayText = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    IsCorrect = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ScoreValue = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_QuizQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_QuizQuestionOptions_As_QuizQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "As_QuizQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tr_StudySessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    TaskId = table.Column<long>(type: "bigint", nullable: true),
                    StudyPlanId = table.Column<long>(type: "bigint", nullable: true),
                    ModuleId = table.Column<long>(type: "bigint", nullable: true),
                    NodeId = table.Column<long>(type: "bigint", nullable: true),
                    StartAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndedReason = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    PlannedDurationSeconds = table.Column<int>(type: "int", nullable: true),
                    ActualDurationSeconds = table.Column<int>(type: "int", nullable: true),
                    ActiveSeconds = table.Column<int>(type: "int", nullable: true),
                    IdleSeconds = table.Column<int>(type: "int", nullable: true),
                    PauseCount = table.Column<int>(type: "int", nullable: true),
                    PauseSeconds = table.Column<int>(type: "int", nullable: true),
                    FocusScore = table.Column<int>(type: "int", nullable: true),
                    ConfidenceActiveLearning = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    FatigueScore = table.Column<int>(type: "int", nullable: true),
                    SelfRating = table.Column<int>(type: "int", nullable: true),
                    LocalTimeBlock = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Timezone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tr_StudySessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tr_StudySessions_Ct_RoadmapNodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Ct_RoadmapNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tr_StudySessions_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tr_StudySessions_Pl_StudyPlanModules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Pl_StudyPlanModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tr_StudySessions_Pl_StudyPlans_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "Pl_StudyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tr_StudySessions_Pl_TaskItems_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Pl_TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_QuizAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AttemptId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    OptionId = table.Column<long>(type: "bigint", nullable: true),
                    TextValue = table.Column<string>(type: "text", nullable: true),
                    NumberValue = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    ScoredValue = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    AnsweredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_QuizAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_QuizAnswers_As_QuizAttempts_AttemptId",
                        column: x => x.AttemptId,
                        principalTable: "As_QuizAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_As_QuizAnswers_As_QuizQuestionOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "As_QuizQuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_As_QuizAnswers_As_QuizQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "As_QuizQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Nt_UserNotifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Title = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    RelatedType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    RelatedId = table.Column<long>(type: "bigint", nullable: true),
                    RelatedSessionId = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: true),
                    IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ReadAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nt_UserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nt_UserNotifications_Id_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nt_UserNotifications_Tr_StudySessions_RelatedSessionId",
                        column: x => x.RelatedSessionId,
                        principalTable: "Tr_StudySessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_As_QuizAnswers_AttemptId_QuestionId",
                table: "As_QuizAnswers",
                columns: new[] { "AttemptId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_As_QuizAnswers_OptionId",
                table: "As_QuizAnswers",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_As_QuizAnswers_QuestionId",
                table: "As_QuizAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_As_QuizAttempts_QuizId",
                table: "As_QuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_As_QuizAttempts_UserId_StartedAt",
                table: "As_QuizAttempts",
                columns: new[] { "UserId", "StartedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_As_QuizQuestionOptions_QuestionId_ValueKey",
                table: "As_QuizQuestionOptions",
                columns: new[] { "QuestionId", "ValueKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_As_QuizQuestions_QuizId_QuestionKey",
                table: "As_QuizQuestions",
                columns: new[] { "QuizId", "QuestionKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_As_Quizzes_RoadmapNodeId",
                table: "As_Quizzes",
                column: "RoadmapNodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_As_SurveyAnswers_OptionId",
                table: "As_SurveyAnswers",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_As_SurveyAnswers_QuestionId",
                table: "As_SurveyAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_As_SurveyAnswers_ResponseId",
                table: "As_SurveyAnswers",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_As_SurveyQuestionOptions_QuestionId_ValueKey",
                table: "As_SurveyQuestionOptions",
                columns: new[] { "QuestionId", "ValueKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_As_SurveyQuestions_SurveyId",
                table: "As_SurveyQuestions",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_As_SurveyResponses_SurveyId",
                table: "As_SurveyResponses",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_As_SurveyResponses_UserId_SubmittedAt",
                table: "As_SurveyResponses",
                columns: new[] { "UserId", "SubmittedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Ct_LearningSubjects_CategoryId",
                table: "Ct_LearningSubjects",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ct_NodeContents_NodeId_OrderNo",
                table: "Ct_NodeContents",
                columns: new[] { "NodeId", "OrderNo" });

            migrationBuilder.CreateIndex(
                name: "IX_Ct_RoadmapEdges_FromNodeId",
                table: "Ct_RoadmapEdges",
                column: "FromNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ct_RoadmapEdges_RoadmapId_FromNodeId_ToNodeId_EdgeType",
                table: "Ct_RoadmapEdges",
                columns: new[] { "RoadmapId", "FromNodeId", "ToNodeId", "EdgeType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ct_RoadmapEdges_ToNodeId",
                table: "Ct_RoadmapEdges",
                column: "ToNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ct_RoadmapNodes_RoadmapId_OrderNo",
                table: "Ct_RoadmapNodes",
                columns: new[] { "RoadmapId", "OrderNo" });

            migrationBuilder.CreateIndex(
                name: "IX_Ct_Roadmaps_SubjectId",
                table: "Ct_Roadmaps",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Id_RefreshTokens_ReplacedByTokenId",
                table: "Id_RefreshTokens",
                column: "ReplacedByTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Id_RefreshTokens_TokenHash",
                table: "Id_RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Id_RefreshTokens_UserId",
                table: "Id_RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Id_RoleClaims_RoleId",
                table: "Id_RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Id_Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Id_UserClaims_UserId",
                table: "Id_UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Id_UserLogins_UserId",
                table: "Id_UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Id_UserRoles_RoleId",
                table: "Id_UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Id_Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Id_Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ln_UserLearningBehaviors_UserId",
                table: "Ln_UserLearningBehaviors",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ln_UserLearningTargets_UserId",
                table: "Ln_UserLearningTargets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Nt_UserNotifications_RelatedSessionId",
                table: "Nt_UserNotifications",
                column: "RelatedSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Nt_UserNotifications_UserId_CreatedAt",
                table: "Nt_UserNotifications",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Nt_UserNotifications_UserId_IsRead_CreatedAt",
                table: "Nt_UserNotifications",
                columns: new[] { "UserId", "IsRead", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Nt_UserPushTokens_DeviceToken",
                table: "Nt_UserPushTokens",
                column: "DeviceToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nt_UserPushTokens_UserId_IsActive",
                table: "Nt_UserPushTokens",
                columns: new[] { "UserId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Pl_StudyPlanModules_RoadmapNodeId",
                table: "Pl_StudyPlanModules",
                column: "RoadmapNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pl_StudyPlanModules_StudyPlanId_RoadmapNodeId",
                table: "Pl_StudyPlanModules",
                columns: new[] { "StudyPlanId", "RoadmapNodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pl_StudyPlans_RoadmapId",
                table: "Pl_StudyPlans",
                column: "RoadmapId");

            migrationBuilder.CreateIndex(
                name: "IX_Pl_StudyPlans_UserId",
                table: "Pl_StudyPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pl_TaskItems_StudyPlanModuleId_ScheduledDate",
                table: "Pl_TaskItems",
                columns: new[] { "StudyPlanModuleId", "ScheduledDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Pl_TaskItems_StudyPlanModuleId_Status_ScheduledDate",
                table: "Pl_TaskItems",
                columns: new[] { "StudyPlanModuleId", "Status", "ScheduledDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Tr_StudySessions_ModuleId",
                table: "Tr_StudySessions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tr_StudySessions_NodeId",
                table: "Tr_StudySessions",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tr_StudySessions_StudyPlanId",
                table: "Tr_StudySessions",
                column: "StudyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Tr_StudySessions_TaskId",
                table: "Tr_StudySessions",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tr_StudySessions_UserId_StartAt",
                table: "Tr_StudySessions",
                columns: new[] { "UserId", "StartAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Tr_UserBehaviorWindows_UserId_WindowStart_WindowEnd",
                table: "Tr_UserBehaviorWindows",
                columns: new[] { "UserId", "WindowStart", "WindowEnd" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tr_UserGamification_UserId",
                table: "Tr_UserGamification",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tr_UserSubjectStats_SubjectId",
                table: "Tr_UserSubjectStats",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tr_UserSubjectStats_UserId_SubjectId",
                table: "Tr_UserSubjectStats",
                columns: new[] { "UserId", "SubjectId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "As_QuizAnswers");

            migrationBuilder.DropTable(
                name: "As_SurveyAnswers");

            migrationBuilder.DropTable(
                name: "Ct_NodeContents");

            migrationBuilder.DropTable(
                name: "Ct_RoadmapEdges");

            migrationBuilder.DropTable(
                name: "Id_RefreshTokens");

            migrationBuilder.DropTable(
                name: "Id_RoleClaims");

            migrationBuilder.DropTable(
                name: "Id_UserClaims");

            migrationBuilder.DropTable(
                name: "Id_UserLogins");

            migrationBuilder.DropTable(
                name: "Id_UserRoles");

            migrationBuilder.DropTable(
                name: "Id_UserTokens");

            migrationBuilder.DropTable(
                name: "Ln_UserLearningBehaviors");

            migrationBuilder.DropTable(
                name: "Ln_UserLearningTargets");

            migrationBuilder.DropTable(
                name: "Nt_UserNotifications");

            migrationBuilder.DropTable(
                name: "Nt_UserPushTokens");

            migrationBuilder.DropTable(
                name: "Tr_UserBehaviorWindows");

            migrationBuilder.DropTable(
                name: "Tr_UserGamification");

            migrationBuilder.DropTable(
                name: "Tr_UserSubjectStats");

            migrationBuilder.DropTable(
                name: "As_QuizAttempts");

            migrationBuilder.DropTable(
                name: "As_QuizQuestionOptions");

            migrationBuilder.DropTable(
                name: "As_SurveyQuestionOptions");

            migrationBuilder.DropTable(
                name: "As_SurveyResponses");

            migrationBuilder.DropTable(
                name: "Id_Roles");

            migrationBuilder.DropTable(
                name: "Tr_StudySessions");

            migrationBuilder.DropTable(
                name: "As_QuizQuestions");

            migrationBuilder.DropTable(
                name: "As_SurveyQuestions");

            migrationBuilder.DropTable(
                name: "Pl_TaskItems");

            migrationBuilder.DropTable(
                name: "As_Quizzes");

            migrationBuilder.DropTable(
                name: "As_Surveys");

            migrationBuilder.DropTable(
                name: "Pl_StudyPlanModules");

            migrationBuilder.DropTable(
                name: "Ct_RoadmapNodes");

            migrationBuilder.DropTable(
                name: "Pl_StudyPlans");

            migrationBuilder.DropTable(
                name: "Ct_Roadmaps");

            migrationBuilder.DropTable(
                name: "Id_Users");

            migrationBuilder.DropTable(
                name: "Ct_LearningSubjects");

            migrationBuilder.DropTable(
                name: "Ct_LearningCategories");
        }
    }
}
