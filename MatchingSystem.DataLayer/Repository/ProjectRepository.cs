using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Base;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;


namespace MatchingSystem.DataLayer.Repository
{
    public class ProjectRepository : ConnectionBase, IProjectRepository
    {
        public ProjectRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<Project>> GetProjectsByTutorAsync(int tutorId)
        {
            return await Connection.QueryAsync<Project>(
                "select * from napp.get_Projects_ByTutor(@TutorID)",
                new { TutorID = tutorId }
            );
        }

        public IEnumerable<Project> GetProjectsByTutor(int tutorId)
        {
            return Connection.Query<Project>(
                "select * from napp.get_Projects_ByTutor(@TutorID)",
                new { TutorID = tutorId }
            );
        }

        public IEnumerable<Project> GetProjectsByMatching(int MatchingId)
        {
            return Connection.Query<Project>(
                "SELECT " +
                "ProjectID " +
                ",ProjectName " +
                ",Info " +
                ",IsClosed " +
                ",ProjectQuotaQty " +
                ",null,null,null " +
                ",IsDefault " +
                ",TutorID " +
                "FROM Projects " +
                "WHERE " +
                "MatchingId = @MatchingId",
                new {MatchingId = MatchingId}
            );
        }

        public async Task<IEnumerable<ProjectForStudent>> GetProjectsByStudentAsync(int studentId)
        {
            return await Connection.QueryAsync<ProjectForStudent>(
                "select " +
                "ProjectID, TutorNameAbbreviation, ProjectName, Info, TechnologiesName_List, WorkDirectionsName_List, IsSelectedByStudent, OrderNumber " +
                "from napp.get_Projects_ByStudent(@StudentID)",
                new { StudentID = studentId }
            );
        }

        public IEnumerable<ProjectForStudent> GetProjectsByStudent(int studentId)
        {
            return Connection.Query<ProjectForStudent>(
                "select " +
                "ProjectID," +
                " TutorNameAbbreviation," +
                " ProjectName," +
                " Info," +
                " TechnologiesName_List," +
                " WorkDirectionsName_List," +
                " IsSelectedByStudent," +
                " OrderNumber " +
                "from napp.get_Projects_ByStudent(@StudentID)",
                new { StudentID = studentId }
            );
        }

        public async Task CreateProjectAsync(ProjectParams @params)
        {
            await Connection.QueryAsync(
                  "exec napp.create_Project @TutorID, @ProjectName,@Info, @QuotaQty , @Technology_CodeList + @WorkDirection_CodeList , @Group_IdList",
                  new
                  {
                      TutorID = @params.TutorId,
                      ProjectName = @params.ProjectName,
                      Info = @params.Info,
                      QuotaQty = @params.Quota,
                      Technology_CodeList = @params.CommaSeparatedTechList,
                      WorkDirection_CodeList = @params.CommaSeparatedWorkList,
                      Group_IdList = @params.CommaSeparatedGroupList
                  });
        }

        public IEnumerable<TutorInitDto> SetDefaultProjectsForTutors(List<TutorInitDto> tutors,int matchingId)
        {
            foreach (var tut in tutors)
            {
                tut.DefaultProjectId = Connection.ExecuteScalar<int>(
                    "insert into Projects (ProjectName,TutorID,IsClosed,IsDefault,MatchingID,CreateDate) OUTPUT INSERTED.ProjectID VALUES (@ProjectName,@TutorId,@IsClosed,@IsDefault,@MatchingID,GETDATE())"
                    , new
                    {
                        ProjectName = "Записаться к преподавателю"
                        ,TutorId = tut.TutorId
                        ,IsClosed = 0
                        ,IsDefault = 1
                        ,MatchingID = matchingId
                    });
            }

            return tutors;
        }
        
        public void SetDefaultProjects_Groups(List<TutorInitDto> tuts)
        {
            foreach (var tut in tuts)
            {
                foreach (var tutGroup in tut.groups)
                {
                    Connection.Execute(
                        "insert into Projects_Groups (ProjectID,GroupID) OUTPUT INSERTED.ProjectID VALUES (@ProjectId,@GroupId)", new
                        {
                            @ProjectId = tut.DefaultProjectId
                            ,@GroupId = tutGroup.groupId
                        });
                }
            }
        }

        public void CreateProject(ProjectParams @params)
        {

            Connection.Query(
                    "exec napp.create_Project @TutorID, @ProjectName, @Info, @QuotaQty , @Technology_CodeList ,@WorkDirection_CodeList , @Group_IdList",
                    new
                    {
                        TutorID = @params.TutorId,
                        ProjectName = @params.ProjectName,
                        Info = @params.Info,
                        QuotaQty = @params.Quota,
                        Technology_CodeList = @params.CommaSeparatedTechList,
                        WorkDirection_CodeList = @params.CommaSeparatedWorkList,
                        Group_IdList = @params.CommaSeparatedGroupList
                    });
        }

        public async Task EditProjectAsync(ProjectParams @params)
        {
           await  Connection.QueryAsync(
               "exec napp.upd_Project " +
               "@TutorID " +
               ",@ProjectID " +
               ",@ProjectName " +
               ",@Info " +
               ",@QuotaQty " +
               ",@Technology_CodeList " +
               ",@WorkDirection_CodeList " +
               ",@Group_IdList ",
                  new
                  {
                      ProjectID = @params.ProjectId,
                      TutorID = @params.TutorId,
                      ProjectName = @params.ProjectName,
                      Info = @params.Info,
                      QuotaQty = @params.Quota,
                      Technology_CodeList = @params.CommaSeparatedTechList,
                      WorkDirection_CodeList = @params.CommaSeparatedWorkList,
                      Group_IdList = @params.CommaSeparatedGroupList
                  });
        }

        public void EditProject(ProjectParams @params)
        {
            Connection.Query(
                      "exec napp.upd_Project " +
                      "@TutorID " +
                      ",@ProjectID " +
                      ",@ProjectName " +
                      ",@Info " +
                      ",@QuotaQty " +
                      ",@Technology_CodeList " +
                      ",@WorkDirection_CodeList " +
                      ",@Group_IdList ",
                   new
                   {
                       ProjectID = @params.ProjectId,
                       TutorID = @params.TutorId,
                       ProjectName = @params.ProjectName,
                       Info = @params.Info,
                       QuotaQty = @params.Quota,
                       Technology_CodeList = @params.CommaSeparatedTechList,
                       WorkDirection_CodeList = @params.CommaSeparatedWorkList,
                       Group_IdList = @params.CommaSeparatedGroupList
                   });
        }

        public async Task DeleteProjectAsync(int projectId)
        {
             await  Connection.QueryAsync("exec napp.del_Project @ProjectID ",
                 new { ProjectID = projectId });
        }

        public void DeleteProject(int projectId)
        {
            Connection.Query("exec napp.del_Project @ProjectID",
                  new { ProjectID = projectId });
        }

        public async Task UpdateProjectQuotaStage3Async(int tutorId, int projectId, int newQty)
        {
            await Connection.QueryAsync("exec napp.upd_ProjectQuota_ForStage3 @TutorID, @ProjectID,@NewQuotaQty",
                   new { TutorID= tutorId, ProjectID = projectId , NewQuotaQty= newQty });
        }

        public void UpdateProjectQuotaStage3(int tutorId, int projectId, int newQty)
        {
             Connection.Query("exec napp.upd_ProjectQuota_ForStage3 @TutorID, @ProjectID,@NewQuotaQty",
                    new { TutorID = tutorId, ProjectID = projectId, NewQuotaQty = newQty });
        }

        public async Task UpdateProjectQuotaStage4Async(int tutorId, int projectId, short projectQuota)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ProjectID", typeof(int));
            dataTable.Columns.Add("Quota", typeof(short));

            var row = dataTable.NewRow();
            row.SetField( "ProjectID", projectId);
            row.SetField( "Quota", projectQuota);
            dataTable.Rows.Add(row);

            //var projectQuotaTable = new SqlParameter("@ProjectQuota", dataTable) {TypeName = "dbo.ProjectQuota"};
            
            await Connection.ExecuteAsync("exec napp.upd_ProjectsQuota_ForStage4 @TutorID, @ProjectQuotaDelta ",
                new { TutorID = tutorId, ProjectQuotaDelta = dataTable.AsTableValuedParameter("[dbo].[ProjectQuota]")  }
            );
        }

        public void UpdateProjectQuotaStage4(int tutorId, int projectId, short projectQuota)
        {
            
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("ProjectID", typeof(int));
            dataTable.Columns.Add("Quota", typeof(short));

            var row = dataTable.NewRow();
            row.SetField( "ProjectID", projectId);
            row.SetField( "Quota", projectQuota);
            dataTable.Rows.Add(row);

           // var projectQuotaTable = new SqlParameter("@ProjectQuota", dataTable) {TypeName = "dbo.ProjectQuota"};
            
            Connection.Execute("exec napp.upd_ProjectsQuota_ForStage4 @TutorID, @ProjectQuota ",
                new { TutorID = tutorId, ProjectQuota = dataTable.AsTableValuedParameter("[dbo].[ProjectQuota]") }
            );
        }

        public async Task SetProjectCloseAsync(int tutorId, int projectId)
        {
            await Connection.ExecuteAsync("exec napp.upd_Project_Close @TutorID, @ProjectID",
                  new { TutorID = tutorId, ProjectID = projectId });
        }

        public void SetProjectClose(int tutorId, int projectId)
        {
            Connection.Execute("exec napp.upd_Project_Close @TutorID, @ProjectID",
                   new { TutorID = tutorId, ProjectID = projectId });
        }
    }
}