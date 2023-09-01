#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace MatchingSystem.Data.Migrations
{
    public partial class initialBaseline : Migration
    {
     
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {   
            /*
            migrationBuilder.CreateTable(
                name: "ChoosingTypes",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeCode = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TypeName_ru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoosingTypes", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Request = table.Column<string>(type: "text", nullable: true),
                    Endpoint = table.Column<string>(type: "text", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MatchingType",
                columns: table => new
                {
                    MatchingTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchingTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MatchingTypeName_ru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MatchingTypeCode = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingType", x => x.MatchingTypeID);
                });

            migrationBuilder.CreateTable(
                name: "QuotasStates",
                columns: table => new
                {
                    QuotaStateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotaStateCode = table.Column<int>(type: "int", nullable: false),
                    QuotaStateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuotaStateName_ru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotasStates", x => x.QuotaStateID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleCode = table.Column<int>(type: "int", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RoleType = table.Column<short>(type: "smallint", nullable: true),
                    RoleName_ru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "StagesTypes",
                columns: table => new
                {
                    StageTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageTypeCode = table.Column<int>(type: "int", nullable: false),
                    StageTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StageTypeName_ru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagesTypes", x => x.StageTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Technologies",
                columns: table => new
                {
                    TechnologyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnologyName_ru = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TechnologyCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.TechnologyID);
                });

            migrationBuilder.CreateTable(
                name: "Tutors",
                columns: table => new
                {
                    TutorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorBK = table.Column<int>(type: "int", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    CloseIterationNumber = table.Column<short>(type: "smallint", nullable: true),
                    IsReadyToStart = table.Column<bool>(type: "bit", nullable: true),
                    MatchingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutors", x => x.TutorID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastVisitDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserBK = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Patronimic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "VersionInfo",
                columns: table => new
                {
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    AppliedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "WorkDirections",
                columns: table => new
                {
                    DirectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectionName_ru = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DirectionCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directions", x => x.DirectionID);
                });

            migrationBuilder.CreateTable(
                name: "Matching",
                columns: table => new
                {
                    MatchingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchingName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatchingTypeID = table.Column<int>(type: "int", nullable: true),
                    CreatorUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matching", x => x.MatchingID);
                    table.ForeignKey(
                        name: "FK_Matching_MatchingType",
                        column: x => x.MatchingTypeID,
                        principalTable: "MatchingType",
                        principalColumn: "MatchingTypeID");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TutorID = table.Column<int>(type: "int", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true),
                    MatchingID = table.Column<int>(type: "int", nullable: true),
                    ProjectQuotaQty = table.Column<short>(type: "smallint", nullable: true),
                    CloseStage = table.Column<int>(type: "int", nullable: true),
                    CloseDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProjectQuotaDelta = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectID);
                    table.ForeignKey(
                        name: "FK_Projects_Tutors",
                        column: x => x.TutorID,
                        principalTable: "Tutors",
                        principalColumn: "TutorID");
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupBK = table.Column<int>(type: "int", nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatchingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupID);
                    table.ForeignKey(
                        name: "FK_Groups_Matching",
                        column: x => x.MatchingID,
                        principalTable: "Matching",
                        principalColumn: "MatchingID");
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    StageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageTypeID = table.Column<int>(type: "int", nullable: false),
                    StageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IterationNumber = table.Column<short>(type: "smallint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    EndPlanDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    MatchingID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.StageID);
                    table.ForeignKey(
                        name: "FK_Stages_Matching",
                        column: x => x.MatchingID,
                        principalTable: "Matching",
                        principalColumn: "MatchingID");
                    table.ForeignKey(
                        name: "FK_Stages_StagesTypes",
                        column: x => x.StageTypeID,
                        principalTable: "StagesTypes",
                        principalColumn: "StageTypeID");
                });

            migrationBuilder.CreateTable(
                name: "Projects_Technologies",
                columns: table => new
                {
                    ProjectTechnologyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    TechnologyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsTechnologies", x => x.ProjectTechnologyID);
                    table.ForeignKey(
                        name: "FK_Projects_Technologies_Projects",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID");
                    table.ForeignKey(
                        name: "FK_Projects_Technologies_Technologies",
                        column: x => x.TechnologyID,
                        principalTable: "Technologies",
                        principalColumn: "TechnologyID");
                });

            migrationBuilder.CreateTable(
                name: "Projects_WorkDirections",
                columns: table => new
                {
                    ProjectDirectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    DirectionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects_Directions", x => x.ProjectDirectionID);
                    table.ForeignKey(
                        name: "FK_Projects_WorkDirections_Projects",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID");
                    table.ForeignKey(
                        name: "FK_Projects_WorkDirections_WorkDirections",
                        column: x => x.DirectionID,
                        principalTable: "WorkDirections",
                        principalColumn: "DirectionID");
                });

            migrationBuilder.CreateTable(
                name: "Projects_Groups",
                columns: table => new
                {
                    ProjectGroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    GroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Projects__17125E7ED62BC05D", x => x.ProjectGroupID);
                    table.ForeignKey(
                        name: "FK_Projects_Groups_Groups",
                        column: x => x.GroupID,
                        principalTable: "Groups",
                        principalColumn: "GroupID");
                    table.ForeignKey(
                        name: "FK_Projects_Groups_Projects",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentBK = table.Column<int>(type: "int", nullable: true),
                    GroupID = table.Column<int>(type: "int", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchingID = table.Column<int>(type: "int", nullable: true),
                    Info2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_Students_Groups",
                        column: x => x.GroupID,
                        principalTable: "Groups",
                        principalColumn: "GroupID");
                });

            migrationBuilder.CreateTable(
                name: "Tutors_Groups",
                columns: table => new
                {
                    TutorGroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorID = table.Column<int>(type: "int", nullable: false),
                    GroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutors_Groups", x => x.TutorGroupID);
                    table.ForeignKey(
                        name: "FK_Tutors_Groups_Groups",
                        column: x => x.GroupID,
                        principalTable: "Groups",
                        principalColumn: "GroupID");
                    table.ForeignKey(
                        name: "FK_Tutors_Groups_Tutors",
                        column: x => x.TutorID,
                        principalTable: "Tutors",
                        principalColumn: "TutorID");
                });

            migrationBuilder.CreateTable(
                name: "CommonQuotas",
                columns: table => new
                {
                    CommonQuotaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorID = table.Column<int>(type: "int", nullable: true),
                    Qty = table.Column<short>(type: "smallint", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    QuotaStateID = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsNotification = table.Column<bool>(type: "bit", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StageID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonQuotas", x => x.CommonQuotaID);
                    table.ForeignKey(
                        name: "FK_CommonQuotas_QuotasStates",
                        column: x => x.QuotaStateID,
                        principalTable: "QuotasStates",
                        principalColumn: "QuotaStateID");
                    table.ForeignKey(
                        name: "FK_CommonQuotas_Stages",
                        column: x => x.StageID,
                        principalTable: "Stages",
                        principalColumn: "StageID");
                    table.ForeignKey(
                        name: "FK_CommonQuotas_Tutors",
                        column: x => x.TutorID,
                        principalTable: "Tutors",
                        principalColumn: "TutorID");
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageID = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_Documents_Stages",
                        column: x => x.StageID,
                        principalTable: "Stages",
                        principalColumn: "StageID");
                });

            migrationBuilder.CreateTable(
                name: "Students_Technologies",
                columns: table => new
                {
                    StudentTechnologyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    TechnologyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsTechnologies", x => x.StudentTechnologyID);
                    table.ForeignKey(
                        name: "FK_Students_Technologies_Students",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK_Students_Technologies_Technologies",
                        column: x => x.TechnologyID,
                        principalTable: "Technologies",
                        principalColumn: "TechnologyID");
                });

            migrationBuilder.CreateTable(
                name: "Students_WorkDirections",
                columns: table => new
                {
                    StudentDirectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    DirectionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsDirections", x => x.StudentDirectionID);
                    table.ForeignKey(
                        name: "FK_Students_WorkDirections_Students",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK_Students_WorkDirections_WorkDirections",
                        column: x => x.DirectionID,
                        principalTable: "WorkDirections",
                        principalColumn: "DirectionID");
                });

            migrationBuilder.CreateTable(
                name: "StudentsPreferences",
                columns: table => new
                {
                    PreferenceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<short>(type: "smallint", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    TypeID = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((1))"),
                    IsInUse = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    IsUsed = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsPreferences", x => x.PreferenceID);
                    table.ForeignKey(
                        name: "FK_StudentsPreferences_ChoosingTypes",
                        column: x => x.TypeID,
                        principalTable: "ChoosingTypes",
                        principalColumn: "TypeID");
                    table.ForeignKey(
                        name: "FK_StudentsPreferences_Projects",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID");
                    table.ForeignKey(
                        name: "FK_StudentsPreferences_Students",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "TutorsChoice",
                columns: table => new
                {
                    ChoiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    SortOrderNumber = table.Column<short>(type: "smallint", nullable: true, defaultValueSql: "((32767))"),
                    IsInQuota = table.Column<bool>(type: "bit", nullable: false),
                    IsChangeble = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    TypeID = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((2))"),
                    PreferenceID = table.Column<int>(type: "int", nullable: true),
                    IterationNumber = table.Column<short>(type: "smallint", nullable: true),
                    StageID = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsFromPreviousIteration = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorsMatching", x => x.ChoiceID);
                    table.ForeignKey(
                        name: "FK_TutorsChoice_ChoosingTypes",
                        column: x => x.TypeID,
                        principalTable: "ChoosingTypes",
                        principalColumn: "TypeID");
                    table.ForeignKey(
                        name: "FK_TutorsChoice_Projects",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID");
                    table.ForeignKey(
                        name: "FK_TutorsChoice_Stages",
                        column: x => x.StageID,
                        principalTable: "Stages",
                        principalColumn: "StageID");
                    table.ForeignKey(
                        name: "FK_TutorsChoice_Students",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "Users_Roles",
                columns: table => new
                {
                    UserRoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    MatchingID = table.Column<int>(type: "int", nullable: true),
                    LastVisitDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    TutorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_Roles", x => x.UserRoleID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Matching",
                        column: x => x.MatchingID,
                        principalTable: "Matching",
                        principalColumn: "MatchingID");
                    table.ForeignKey(
                        name: "FK_Users_Roles_Roles",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID");
                    table.ForeignKey(
                        name: "FK_Users_Roles_Students",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK_Users_Roles_Tutors",
                        column: x => x.TutorID,
                        principalTable: "Tutors",
                        principalColumn: "TutorID");
                    table.ForeignKey(
                        name: "FK_Users_Roles_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonQuotas_QuotaStateID",
                table: "CommonQuotas",
                column: "QuotaStateID");

            migrationBuilder.CreateIndex(
                name: "IX_CommonQuotas_StageID",
                table: "CommonQuotas",
                column: "StageID");

            migrationBuilder.CreateIndex(
                name: "IX_CommonQuotas_TutorID",
                table: "CommonQuotas",
                column: "TutorID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StageID",
                table: "Documents",
                column: "StageID");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_MatchingID",
                table: "Groups",
                column: "MatchingID");

            migrationBuilder.CreateIndex(
                name: "IX_Matching_MatchingTypeID",
                table: "Matching",
                column: "MatchingTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TutorID",
                table: "Projects",
                column: "TutorID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Groups_GroupID",
                table: "Projects_Groups",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Groups_ProjectID",
                table: "Projects_Groups",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Technologies_ProjectID",
                table: "Projects_Technologies",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Technologies_TechnologyID",
                table: "Projects_Technologies",
                column: "TechnologyID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_WorkDirections_DirectionID",
                table: "Projects_WorkDirections",
                column: "DirectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_WorkDirections_ProjectID",
                table: "Projects_WorkDirections",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_MatchingID",
                table: "Stages",
                column: "MatchingID");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_StageTypeID",
                table: "Stages",
                column: "StageTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_GroupID",
                table: "Students",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Technologies_StudentID",
                table: "Students_Technologies",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Technologies_TechnologyID",
                table: "Students_Technologies",
                column: "TechnologyID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_WorkDirections_DirectionID",
                table: "Students_WorkDirections",
                column: "DirectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_WorkDirections_StudentID",
                table: "Students_WorkDirections",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsPreferences_ProjectID",
                table: "StudentsPreferences",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsPreferences_StudentID",
                table: "StudentsPreferences",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsPreferences_TypeID",
                table: "StudentsPreferences",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Tutors_Groups_GroupID",
                table: "Tutors_Groups",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Tutors_Groups_TutorID",
                table: "Tutors_Groups",
                column: "TutorID");

            migrationBuilder.CreateIndex(
                name: "IX_TutorsChoice_ProjectID",
                table: "TutorsChoice",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_TutorsChoice_StageID",
                table: "TutorsChoice",
                column: "StageID");

            migrationBuilder.CreateIndex(
                name: "IX_TutorsChoice_StudentID",
                table: "TutorsChoice",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_TutorsChoice_TypeID",
                table: "TutorsChoice",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Roles_MatchingID",
                table: "Users_Roles",
                column: "MatchingID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Roles_RoleID",
                table: "Users_Roles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Roles_StudentID",
                table: "Users_Roles",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Roles_TutorID",
                table: "Users_Roles",
                column: "TutorID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Roles_UserID",
                table: "Users_Roles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "UC_Version",
                table: "VersionInfo",
                column: "Version",
                unique: true)
                .Annotation("SqlServer:Clustered", true);
                */
        }
     
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommonQuotas");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Projects_Groups");

            migrationBuilder.DropTable(
                name: "Projects_Technologies");

            migrationBuilder.DropTable(
                name: "Projects_WorkDirections");

            migrationBuilder.DropTable(
                name: "Students_Technologies");

            migrationBuilder.DropTable(
                name: "Students_WorkDirections");

            migrationBuilder.DropTable(
                name: "StudentsPreferences");

            migrationBuilder.DropTable(
                name: "Tutors_Groups");

            migrationBuilder.DropTable(
                name: "TutorsChoice");

            migrationBuilder.DropTable(
                name: "Users_Roles");

            migrationBuilder.DropTable(
                name: "VersionInfo");

            migrationBuilder.DropTable(
                name: "QuotasStates");

            migrationBuilder.DropTable(
                name: "Technologies");

            migrationBuilder.DropTable(
                name: "WorkDirections");

            migrationBuilder.DropTable(
                name: "ChoosingTypes");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tutors");

            migrationBuilder.DropTable(
                name: "StagesTypes");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Matching");

            migrationBuilder.DropTable(
                name: "MatchingType");
        }
    }
}
     