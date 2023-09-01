using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Feature.Interface;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Repository
{
    public class StatisticsRepository : ConnectionBase, IStatisticsRepository
    {
        public StatisticsRepository(string connectionString) : base(connectionString)
        {
        }


        public async Task<IEnumerable<StatisticsMain>> GetStatisticsMainAsync(int matchingId, int currentStage)
        {
            //TODO разделить функцию

            if (currentStage == 2)
            {
                return await Connection.QueryAsync<StatisticsMain>("select StatName, StatValue_Str " +
                                                                   "from napp.get_StatisticStage2_Main(@MatchingID)",
                    new {MatchingID = matchingId});
            }

            if (currentStage == 3)
            {
                return await Connection.QueryAsync<StatisticsMain>("select StatName, StatValue_Str " +
                                                                   "from napp.get_StatisticStage3_Main(@MatchingID)",
                    new {MatchingID = matchingId});
            }

            if (currentStage == 4)
            {
                return await Connection.QueryAsync<StatisticsMain>("select StatName, StatValue_Str " +
                                                                   "from napp.get_StatisticStage4_Main(@MatchingID)",
                    new {MatchingID = matchingId});
            }

            return null;
        }
        public IEnumerable<StatisticsMain> GetStatisticsMain(int matchingId, int currentStage)
        {
        

            IEnumerable<StatisticsMain> result = new List<StatisticsMain>();

            if (currentStage == 2)
            {
                result = Connection.Query<StatisticsMain>("select StatName, StatValue_Str, StatPercentage " +
                                                          "from napp.get_StatisticStage2_Main(@MatchingID)",
                    new {MatchingID = matchingId});
            }
            else if (currentStage == 3)
            {
                result = Connection.Query<StatisticsMain>("select StatName, StatValue_Str, StatPercentage " +
                                                          "from napp.get_StatisticStage3_Main(@MatchingID)",
                    new {MatchingID = matchingId});
            }
            else if (currentStage == 4)
            {
                result = Connection.Query<StatisticsMain>("select StatName, StatValue_Str, StatPercentage " +
                                                          "from napp.get_StatisticStage4_Main(@MatchingID)",
                    new {MatchingID = matchingId});
            }

            return result;
        }

        public async Task<IEnumerable<StatisticsMain>> GetStatisticsGroupsAsync(int matchingId)
        {
            return await Connection.QueryAsync<StatisticsMain>("select StatName, StatValue_Str ,StatPercentage " +
                                                               "from napp.get_StatisticStage3_Main_ProgressBars(@MatchingID)",
                new {MatchingID = matchingId});
        }
        public IEnumerable<StatisticsMain> GetStatisticsGroups(int matchingId)
        {
            return Connection.Query<StatisticsMain>("select StatName, StatValue_Str ,StatPercentage " +
                                                    "from napp.get_StatisticStage3_Main_ProgressBars(@MatchingID)",
                new {MatchingID = matchingId});
        }

        public async Task<IEnumerable<StatisticsTutors>> GetStatisticsTutorsAsync(int matchingId, int currentStage)
        {
            //TODO разделить функцию
            // TODO имена параметров временные потом поменять 
            if (currentStage == 2)
            {
                return await Connection.QueryAsync<StatisticsTutors>("select " +
                                                                     "TutorID " +
                                                                     ",TutorNameAbbreviation " +
                                                                     ",QuotaQty " +
                                                                     ",TutorIsReadyToStart " +
                                                                     ",TutorLastVisitDate " +
                                                                     "from napp.get_StatisticStage2_Tutors(@MatchingID)",
                    new {MatchingID = matchingId});
            }

            if (currentStage == 3)
            {
                return await Connection.QueryAsync<StatisticsTutors>("select " +
                                                                     "TutorID " +
                                                                     ",TutorNameAbbreviation " +
                                                                     ",QuotaQty " +
                                                                     ",TutorIsReadyToStart " +
                                                                     ",TutorLastVisitDate " +
                                                                     "from napp.get_StatisticStage3_Tutors(@MatchingID)",
                    new {MatchingID = matchingId});
            }

            if (currentStage == 4)
            {
                return await Connection.QueryAsync<StatisticsTutors>("select " +
                                                                     "TutorID " +
                                                                     ",TutorNameAbbreviation " +
                                                                     ",QuotaQty " +
                                                                     ",TutorIsReadyToStart " +
                                                                     ",TutorLastVisitDate " +
                                                                     "from napp.get_StatisticStage4_Tutors(@MatchingID)",
                    new {MatchingID = matchingId});
            }

            return null;
        }
        public IEnumerable<StatisticsTutors> GetStatisticsTutors(int matchingId, int currentStage)
        {
        

            IEnumerable<StatisticsTutors> result = new List<StatisticsTutors>();

            if (currentStage == 2)
            {
                result = Connection.Query<StatisticsTutors>("select " +
                                                            "TutorID " +
                                                            ",TutorNameAbbreviation " +
                                                            ",QuotaQty " +
                                                            ",TutorIsReadyToStart " +
                                                            ",TutorLastVisitDate " +
                                                            "from napp.get_StatisticStage2_Tutors(@MatchingID)",
                    new {MatchingID = matchingId});
            }
            else if (currentStage == 3)
            {
                result = Connection.Query<StatisticsTutors>("select " +
                                                            "TutorID " +
                                                            ",TutorNameAbbreviation " +
                                                            ",QuotaQty " +
                                                            ",TutorIsReadyToStart " +
                                                            ",TutorLastVisitDate " +
                                                            "from napp.get_StatisticStage3_Tutors(@MatchingID)",
                    new {MatchingID = matchingId});
            }
            else if (currentStage == 4)
            {
                result = Connection.Query<StatisticsTutors>("select " +
                                                            "TutorID " +
                                                            ",TutorNameAbbreviation " +
                                                            ",TutorQuotaQty " +
                                                            ",TutorLastVisitDate " +
                                                            ",TutorProjectsAllCount " +
                                                            ",TutorProjectsClosedCount " +
                                                            ",TutorIsSelfChoice " +
                                                            ",TutorIsAvailableChoice " +
                                                            ",TutorStudentsInQuotaCount " +
                                                            ",TutorStudentsOutQuotaCount " +
                                                            "from napp.get_StatisticStage4_Tutors(@MatchingID)",
                    new {MatchingID = matchingId});
            }

            return result;
        }

        public IEnumerable<Project> GetStatisticsTutorsProjects(int matchingId, int tutorId)
        {
            return Connection.Query<Project>("select " +
                                             "ProjectName " +
                                             ",TechnologiesName_List " +
                                             ",WorkDirectionsName_List " +
                                             ",Info " +
                                             ",Qty " +
                                             
                                             
                                             ",AvailableGroupsName_List " +
                                             "from napp.get_StatisticStage2_Tutor_Projects(@MatchingID,@TutorId)",
                new {MatchingID = matchingId, TutorId = tutorId});
        }
        public async Task<IEnumerable<Project>> GetStatisticsTutorsProjectsAsync(int matchingId, int tutorId)
        {
            return await Connection.QueryAsync<Project>("select " +
                                                        "ProjectName " +
                                                        ",ProjectTechnologiesName_List " +
                                                        ",ProjectWorkDirectionsName_List " +
                                                        ",ProjectsQty " +
                                                        ",ProjectAvailableGroupsName_List " +
                                                        "from napp.get_StatisticStage2_Tutor_Projects(@MatchingID,@TutorId)",
                new {MatchingID = matchingId, TutorId = tutorId});
        }

        public IEnumerable<StatisticsStudentsProjects> GetStatisticsStudentsProjects(int matchingId, int studentId)
        {
            return Connection.Query<StatisticsStudentsProjects>("select " +
                                                                "StudentsPreferencesProjectID " +
                                                                ",ProjectsProjectName " +
                                                                ",ProjectsTechnologiesName_List " +
                                                                ",ProjectsWorkDirectionsName_List " +
                                                                ",ProjectQty " +
                                                                ",ProjectsAvailableGroupsName_List " +
                                                                ",TutorNameAbbreviation " +
                                                                ",OrderNumber "+
                                                                "from napp.get_StatisticStage3_Student_Projects(@MatchingID,@StudentID)",
                new {MatchingID = matchingId, StudentID = studentId});
        }
        public async Task<IEnumerable<StatisticsStudentsProjects>> GetStatisticsStudentsProjectsAsync(int matchingId, int studentId)
        {
            return await Connection.QueryAsync<StatisticsStudentsProjects>("select " +
                                                                           "StudentsPreferencesProjectID " +
                                                                           ",ProjectsProjectName " +
                                                                           ",ProjectsTechnologiesName_List " +
                                                                           ",ProjectsWorkDirectionsName_List " +
                                                                           ",ProjectQty " +
                                                                           ",ProjectsAvailableGroupsName_List " +
                                                                           "from napp.get_StatisticStage3_Student_Projects(@MatchingID,@StudentID)",
                new {MatchingID = matchingId, StudentID = studentId});
        }

        public async Task<IEnumerable<StatisticsStudents>> GetStatisticsStudentsAsync(int matchingId, int currentStage)
        {
          

            IEnumerable<StatisticsStudents> result = new List<StatisticsStudents>();


            if (currentStage == 3)
            {
                return await Connection.QueryAsync<StatisticsStudents>("select " +
                                                                       "StudentStudentID " +
                                                                       ",StudentNameAbbreviation " +
                                                                       ",StudentLastVisitDate " +
                                                                       ",IsSetInfo " +
                                                                       ",IsSetTechnologies " +
                                                                       ",IsSetWorkDirections " +
                                                                       ",ProjectCount " +
                                                                       ",StudentGroupName" +
                                                                       "from napp.get_StatisticStage3_Students(@MatchingID)",
                    new {MatchingID = matchingId});
            }

            if (currentStage == 4)
            {
                return await Connection.QueryAsync<StatisticsStudents>("select " +
                                                                       "StudentStudentID " +
                                                                       ",StudentNameAbbreviation " +
                                                                       ",StudentLastVisitDate " +
                                                                       ",StudentGroupID " +
                                                                       ",StudentGroupName " +
                                                                       ",PreferenceTypeCode " +
                                                                       ",PreferenceTypeName " +
                                                                       ",PreferenceTypeName_ru " +
                                                                       "from napp.get_StatisticStage4_Students(@MatchingID)",
                    new {MatchingID = matchingId});
            }

            return null;
        }
        public IEnumerable<StatisticsStudents> GetStatisticsStudents(int matchingId, int currentStage)
        {
    

            IEnumerable<StatisticsStudents> result = new List<StatisticsStudents>();


            if (currentStage == 3)
            {
                result = Connection.Query<StatisticsStudents>("select " +
                                                              "StudentID " +
                                                              ",StudentNameAbbreviation " +
                                                              ",StudentLastVisitDate " +
                                                              ",IsSetInfo " +
                                                              ",IsSetTechnologies " +
                                                              ",IsSetWorkDirections " +
                                                              ",ProjectCount " +
                                                              ",StudentGroupName " +
                                                              "from napp.get_StatisticStage3_Students(@MatchingID)",
                    new {MatchingID = matchingId});
            }
            else if (currentStage == 4)
            {
                result = Connection.Query<StatisticsStudents>("select " +
                                                              "StudentID " +
                                                              ",StudentNameAbbreviation " +
                                                              ",StudentLastVisitDate " +
                                                              ",StudentGroupName " +
                                                              ",ProjectIsAllocated " +
                                                              ",ChoiceIsInQuota " +
                                                              ",ChoiceIsCantAllocated " +
                                                              ",PreferenceTypeName_ru " +
                                                              "from napp.get_StatisticStage4_Students(@MatchingID)",
                    new {MatchingID = matchingId});
            }
            else
            {
                result = null;
            }

            return result;
        }
        
        public async Task<IEnumerable<TutorsProjectAllocation>> GetStatisticsTutorProjectAllocatedAsync(int matchingId, int tutorId)
        {
            return await Connection.QueryAsync<TutorsProjectAllocation>("select " +
                                                                        "Allocation_FullInfoStudentNameAbbreviation " +
                                                                        ",Allocation_FullInfoProjectName " +
                                                                        ",Allocation_FullInfoTypeName_ru " +
                                                                        "from napp.get_StatisticStage4_Tutors_Project_Allocated(@MatchingID,@TutorID)",
                new {MatchingID = matchingId, TutorId = tutorId});
        }
        public IEnumerable<TutorsProjectAllocation> GetStatisticsTutorProjectAllocated(int matchingId, int tutorId)
        {
            return Connection.Query<TutorsProjectAllocation>("select " +
                                                             "Allocation_FullInfoStudentNameAbbreviation " +
                                                             ",Allocation_FullInfoProjectName " +
                                                             ",Allocation_FullInfoTypeName_ru " +
                                                             "from napp.get_StatisticStage4_Tutors_Project_Allocated(@MatchingID,@TutorID)",
                new {MatchingID = matchingId, TutorId = tutorId});
        }

    }
}
