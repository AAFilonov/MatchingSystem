#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<RoleMatching> RolesAndMatchings { get; set; }
        public DbSet<Project> TutorProjects { get; set; }
        public DbSet<ProjectForStudent> StudentProjects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<WorkDirection> WorkDirections { get; set; }
        public DbSet<QuotaHistoryTutor> HistoryByTutor { get; set; }
        public DbSet<QuotaHistoryExecutive> HistoryByExecutive { get; set; }
        public DbSet<QuotaRequest> QuotaRequests { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<TutorChoice> Choice { get; set; }
        public DbSet<StringModel> GetString { get; set; }
        public DbSet<IntModel> GetInt { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Matching> Matchings { get; set; }
        public DbSet<MainStatistics> MainStatistics { get; set; }
        public DbSet<Allocation> AllocationByExecutive { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<AllocatedByStudent> AllocationByStudent { get; set; }
        public DbSet<MatchingInfo> MatchingInfo { get; set; }

        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RoleMatching>().HasNoKey();
            builder.Entity<IntModel>().HasNoKey();
            builder.Entity<StringModel>().HasNoKey();
            builder.Entity<QuotaHistoryExecutive>().HasNoKey();
            builder.Entity<BoolModel>().HasNoKey();
            builder.Entity<QuotaHistoryTutor>().HasNoKey();
            builder.Entity<TutorChoice>().HasNoKey();
            builder.Entity<Role>().HasNoKey();
            builder.Entity<Matching>().HasNoKey();
            builder.Entity<MainStatistics>().HasNoKey();
            builder.Entity<Allocation>().HasNoKey();
            builder.Entity<AllocatedByStudent>().HasNoKey();
        }

        #region Stored rocedures query

        public async Task<bool> AddProject(int tutorId, string projectName, string info, string techList, string workList, string groupList, string quota)
        {
            SqlParameter pTutorID = new SqlParameter("@TutorID", tutorId);
            SqlParameter pName = new SqlParameter("@ProjectName", projectName);
            SqlParameter pInfo = new SqlParameter("@Info", info);

            SqlParameter pTechList = new SqlParameter("@Technology_CodeList", techList);
            SqlParameter pWorkDirections = new SqlParameter("@WorkDirection_CodeList", workList);
            SqlParameter pGroupList = new SqlParameter("@Group_IdList", groupList);
            SqlParameter pQuotaQty;

            if (quota == "Не важно")
            {
                pQuotaQty = new SqlParameter("@QuotaQty", DBNull.Value);
            }
            else
            {
                pQuotaQty = new SqlParameter("@QuotaQty", Convert.ToInt32(quota));
            }

            try
            {
                await Database.ExecuteSqlRawAsync(
                    "exec napp.create_Project @TutorID, " +
                    "@ProjectName, " +
                    "@Info, " +
                    "@QuotaQty, " +
                    "@Technology_CodeList, " +
                    "@WorkDirection_CodeList, " +
                    "@Group_IdList", pTutorID, pName, pInfo, pQuotaQty, pTechList,
                    pWorkDirections, pGroupList
                );
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> EditProject(int projectId, int tutorId, string projectName, string info, string techList, string workList, string groupList, string quota)
        {
            SqlParameter pTutorID = new SqlParameter("@TutorID", tutorId);
            SqlParameter pName = new SqlParameter("@ProjectName", projectName);
            SqlParameter pInfo = new SqlParameter("@Info", info);
            SqlParameter pProjID = new SqlParameter("@ProjectID", projectId);
            SqlParameter pTechList = new SqlParameter("@Technology_CodeList", techList);
            SqlParameter pWorkDirections = new SqlParameter("@WorkDirection_CodeList", workList);
            SqlParameter pGroupList = new SqlParameter("@Group_IdList", groupList);
            SqlParameter pQuotaQty;
            if (quota == "Не важно")
            {
                pQuotaQty = new SqlParameter("@QuotaQty", DBNull.Value);
            }
            else
            {
                pQuotaQty = new SqlParameter("@QuotaQty", Convert.ToInt32(quota));
            }

            try
            {
                await Database.ExecuteSqlRawAsync(
                    "exec napp.upd_Project @TutorID, @ProjectID, " +
                    "@ProjectName, " +
                    "@Info, " +
                    "@QuotaQty, " +
                    "@Technology_CodeList, " +
                    "@WorkDirection_CodeList, " +
                    "@Group_IdList", pTutorID, pProjID, pName, pInfo, pQuotaQty,
                    pTechList, pWorkDirections, pGroupList);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetEndDate(DateTime? time, int? matchingId)
        {
            await Database.ExecuteSqlRawAsync("exec napp.upd_CurrentStage_EndPlanDate @MatchingID, @EndDate",
            new SqlParameter("@MatchingID", matchingId),
            new SqlParameter("@EndDate", time));
        }
        public async Task SetNextStage(int? matchingId)
        {
            await Database.ExecuteSqlRawAsync("exec napp.goto_NextStage @MatchingID",
                new SqlParameter("@MatchingID", matchingId));
        }

        public async Task SetAdjustmentAsync(int? userId, int? matchingId, int? studentId, int? projectId)
        {
            await Database.ExecuteSqlRawAsync(
                    "exec napp.create_ExecutiveChoice @UserID, @MatchingID, @ProjectID, @StudentID",
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@MatchingID", matchingId),
                    new SqlParameter("@ProjectID", projectId),
                    new SqlParameter("@StudentID", studentId));
        }

        public async Task SetLastVisitDate(int userId, string roleName, int? matchingId)
        {
            SqlParameter userID = new SqlParameter("@UserID", userId);
            SqlParameter role_name = new SqlParameter("@RoleName", roleName);
            SqlParameter matchingID = new SqlParameter("@MatchingID", matchingId);
            
            await Database.ExecuteSqlRawAsync(
                "exec napp.upd_User_LastVisitDate " +
                "@UserID, " +
                "null, " +
                "@RoleName, " +
                "@MatchingID",
            userID, role_name, matchingID
            );
        }

        public void ReadNotifications(int? userId, int? matchingId, int? tutorId = null)
        {
            SqlParameter uid = new SqlParameter("@UserID", userId);
            SqlParameter matching = new SqlParameter("@MatchingID", matchingId);
            SqlParameter tutor;
            if (tutorId.HasValue) tutor = new SqlParameter("@TutorID", tutorId);
            else tutor = new SqlParameter("@TutorID", DBNull.Value);

            Database.ExecuteSqlRaw(
                "exec napp.upd_CommonQuota_Request_ReadNotifications @UserID, @MatchingID, @TutorID",
                uid, matching, tutor
            );
        }

        public async Task<bool> CreateCommonQuotaRequestForIterationsAsync(
            int? tutorId,
            int? newQuota,
            string? message,
            DataTable table
        )
        {
            var tid = new SqlParameter("@TutorID", tutorId);
            var nq = new SqlParameter("@NewQuotaQty", newQuota);
            var msg = new SqlParameter("@Message", message ?? string.Empty);
            var pq = new SqlParameter("@ProjectQuota", table) { TypeName = ProjectQuota.DbTypeName };

            await Database.ExecuteSqlRawAsync(
                "exec napp.create_CommonQuota_Request " +
                "@TutorID," +
                "@NewQuotaQty, " +
                "@Message, " +
                "@ProjectQuota", tid, nq, msg, pq
            );
            return true;
        }
        #endregion
        #region Functions query

        public async Task<int> GetReady(int? tutorId)
        {
            return (await GetInt
                .FromSqlRaw(
                    "select napp.get_IsReadyToStart_ByTutor (@TutorID) as Value",
                    new SqlParameter("@TutorID", tutorId)
                ).FirstOrDefaultAsync()).Value;
        }

        public async Task<int> GetNotificationsCountByTutor(int? tutorId)
        {
            return (await GetInt.FromSqlRaw(
                "select count(*) as Value from napp.get_CommonQuota_Request_Notification_ByTutor(@TutorID)",
                new SqlParameter("@TutorID", tutorId)).FirstOrDefaultAsync()).Value;
        }

        public async Task<int> GetNotificationsCountByExecutive(int? userId, int? matchingId)
        {
            return (
                await GetInt.FromSqlRaw("select " +
                    "napp.get_CommonQuota_Requests_NotificationCount_ByExecutive(@UserID, @MatchingID) as Value",
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@MatchingID", matchingId)).FirstOrDefaultAsync()
                ).Value;
        }

        public async Task<List<MatchingInfo>> GetMatchingsInfoAsync()
        {
            return await MatchingInfo
                .FromSqlRaw(
                    "select * from napp.get_FinishedMatchings()"
                ).ToListAsync();
        }

        public async Task<AllocatedByStudent> GetAllocationByStudentAsync(int? studentId)
        {
            return await AllocationByStudent
                .FromSqlRaw(
                    "select TutorNameAbbreviation, ProjectName from napp.get_AllocatedProject_ByStudent(@StudentID)",
                    new SqlParameter("@StudentID", studentId)
                ).FirstOrDefaultAsync();
        }

        public async Task<List<Allocation>> GetFinalAllocation()
        {
            var result = await AllocationByExecutive
                .FromSqlRaw(
                    "select MatchingID, " +
                    "ProjectID, StudentID, TutorID, GroupID, " +
                    "StudentNameAbbreviation, GroupName, " +
                    "ProjectName, TutorNameAbbreviation, IsAllocated " +
                    "from napp.get_FinishedAllocations()"
                    ).ToListAsync();
            return result;
        }
        public async Task<List<Allocation>> GetAllocationsByExecutiveAsync(int? userId, int? matchingId)
        {
            var result = await AllocationByExecutive
                .FromSqlRaw(
                    "select MatchingID, " +
                    "ProjectID, StudentID, TutorID, GroupID, " +
                    "StudentNameAbbreviation, GroupName, " +
                    "ProjectName, TutorNameAbbreviation, IsAllocated " +
                    "from napp.get_Allocation_ByExecutive(@UserID, @MatchingID)",
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@MatchingID", matchingId)
                ).ToListAsync();
            return result;
        }

        public async Task<List<Tutor>> GetTutorsByMatchingAsync(int? matchingId)
        {
            return await Tutors
                .FromSqlRaw(
                    "select TutorID, TutorNameAbbreviation from napp.get_Tutors_ByMatching(@MatchingID)",
                    new SqlParameter("@MatchingID", matchingId)
                ).ToListAsync();
        }

        public async Task<List<Group>> GetGroupsByTutorAsync(int? tutorId)
        {
            return await Groups
                .FromSqlRaw(
                    "select * from napp.get_Groups_ByTutor(@TutorID)",
                new SqlParameter("@TutorID", tutorId)
                ).ToListAsync();
        }

        public async Task<List<QuotaRequest>> GetQuotaRequestAsync(int? userId, int? matchingId)
        {
            return await QuotaRequests
                .FromSqlRaw(
                    "select QuotaID, " +
                    "TutorID, " +
                    "NameAbbreviation, " +
                    "RequestedQuotaQty, " +
                    "Message, " +
                    "CurrentQuotaQty " +
                    "from napp.get_CommonQuota_Requests_ByExecutive(@UserID, @MatchingID)",
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@MatchingID", matchingId)
                ).ToListAsync();
        }

        public async Task<List<Matching>> GetMatchingsForUserAsync(int userId)
        {
            return await Matchings
                .FromSqlRaw(
                "select MatchingID, MatchingName, MatchingTypeName_ru from napp.get_UserMatchings(@UserID, null)",
                new SqlParameter("@UserID", userId)
                ).ToListAsync();
        }

        public async Task<List<Role>> GetRolesForUserAsync(int userId, int matchingId)
        {
            return await Roles.FromSqlRaw(
                "select RoleCode, RoleName, RoleName_ru from napp.get_UserRoles_ByMatching(@UserID, @MatchingID)",
                new SqlParameter("@UserID", userId),
                new SqlParameter("@MatchingID", matchingId)
                ).ToListAsync();
        }

        public async Task<Stage> GetCurrentStageAsync(int? matchingId)
        {
            return await Stages
                .FromSqlRaw(
                    "select StageID, " +
                    "StartDate, " +
                    "EndPlanDate, " +
                    "IterationNumber, " +
                    "StageTypeCode, " +
                    "StageTypeName_ru from napp.get_CurrentStage_ByMatching (@MatchingID)",
                    new SqlParameter("@MatchingID", matchingId)
                ).FirstOrDefaultAsync();
        }

        public async Task<List<TutorChoice>> GetTutorChoiceAsync(int? tutorId)
        {
            var resutl = await Choice
                .FromSqlRaw("select ChoiceID, " +
                    "StudentNameAbbreviation, " +
                    "GroupName, " +
                    "ProjectID, " +
                    "StudentID, " +
                    "ProjectName, " +
                    "IsInQuota, " +
                    "Qty, " +
                    "SortOrderNumber, " +
                    "ProjectIsClosed, " +
                    "TypeCode " +
                    "from napp.get_TutorsChoice_ByTutor(@TutorID)",
                    new SqlParameter("@TutorID", tutorId)
                ).ToListAsync();
            return resutl;
        }

        public async Task<int> GetCommonQuotaAsync(int? tutorId)
        {
            return (await GetInt
                .FromSqlRaw(
                    "select napp.get_CommonQuota_ByTutor(@TutorID) as Value",
                    new SqlParameter("@TutorID", tutorId)).FirstOrDefaultAsync()
                ).Value;
        }

        public async Task<List<QuotaHistoryTutor>> GetTutorQuotaHistoryAsync(int? tutorId)
        {
            return await HistoryByTutor
                .FromSqlRaw(
                    "select CreateDate, " +
                    "Qty, " +
                    "StageTypeName_ru, " +
                    "QuotaStateName_ru, " +
                    "QuotaStateCode, " +
                    "IterationNumber " +
                    "from napp.get_CommonQuota_History_ByTutor(@TutorID) order by CreateDate desc",
                    new SqlParameter("@TutorID", tutorId)
                ).ToListAsync();
        }

        public async Task<List<QuotaHistoryExecutive>> GetCommonQuotaHistoryByExecutiveAsync(int? userId, int? matchingId)
        {
            return await HistoryByExecutive
                .FromSqlRaw(
                    "select NameAbbreviation, " +
                    "StageTypeName_ru, " +
                    "RequestedQuotaQty, " +
                    "QuotaStateName_ru, " +
                    "Message, " +
                    "IterationNumber " +
                    "from napp.get_CommonQuota_History_ByExecutive(@UserID, @MatchingID) order by CreateDate asc",
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@MatchingID", matchingId)).ToListAsync();
        }

        public async Task<List<Project>> GetTutorProjectsAsync(int? tutorId)
        {
            return await TutorProjects
                .FromSqlRaw(
                    "select * from napp.get_Projects_ByTutor(@TutorID)",
                    new SqlParameter("@TutorID", tutorId)
                ).ToListAsync();
        }

        public Student GetStudent(int? studentId)
        {
            var result = Students
                .FromSqlRaw(
                    "select StudentID, " +
                    "GroupID, " +
                    "GroupName, " +
                    "Surname, " +
                    "Name, " +
                    "Patronimic, " +
                    "Info, " +
                    "WorkDirectionsName_List, " +
                    "TechnologiesName_List " +
                    "from napp.get_StudentInfo(@StudentID)",
                    new SqlParameter("@StudentID", studentId)
                ).FirstOrDefault();
            return result ?? new Student();
        }

        public List<MainStatistics> GetMainStatisticsAsync(int? matchingId, int? currentStage)
        {
            List<MainStatistics> result = new List<MainStatistics>();

            if (currentStage == 2)
            {
                result = MainStatistics
                    .FromSqlRaw(
                        "select StatName, StatValue_Str from napp.get_StatisticStage2_Main(@MatchingID)",
                        new SqlParameter("@MatchingID", matchingId)
                    ).ToList();
            }
            else if (currentStage == 3)
            {
                result = MainStatistics
                    .FromSqlRaw(
                        "select StatName, StatValue_Str from napp.get_StatisticStage3_Main(@MatchingID)",
                        new SqlParameter("@MatchingID", matchingId)
                    ).ToList();
            }
            else if (currentStage == 4)
            {
                result = MainStatistics
                    .FromSqlRaw(
                        "select StatName, StatValue_Str from napp.get_StatisticStage4_Main(@MatchingID)",
                        new SqlParameter("@MatchingID", matchingId)
                    ).ToList();
            }

            return result;
        }
        #endregion

        #region Tools
        public DataTable FillProjectQuota(List<ProjectQuota> arg)
        {
            var table = new DataTable();
            table.Columns.Add("ProjectID", typeof(int));
            table.Columns.Add("Quota", typeof(short));

            foreach (var projectQuota in arg)
            {
                var row = table.NewRow();
                row.SetField("ProjectID", projectQuota.ProjectID);
                row.SetField("Quota", projectQuota.Quota);
                table.Rows.Add(row);
            }
            return table;
        }
        #endregion
    }
}