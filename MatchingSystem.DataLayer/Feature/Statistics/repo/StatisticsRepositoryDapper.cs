using System.Collections.Generic;
using Dapper;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.OldEntities;
using MatchingSystem.DataLayer.Utils;

namespace MatchingSystem.DataLayer.Feature.Statistics.repo
{
    public class StatisticsRepositoryOldDapper : ConnectionBase, IStatisticsRepositoryOld
    {
        public StatisticsRepositoryOldDapper(string connectionString) : base(connectionString)
        {
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


        public IEnumerable<StatisticsMain> GetStatisticsGroups(int matchingId)
        {
            return Connection.Query<StatisticsMain>("select StatName, StatValue_Str ,StatPercentage " +
                                                    "from napp.get_StatisticStage3_Main_ProgressBars(@MatchingID)",
                new {MatchingID = matchingId});
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

        public IEnumerable<ProjectDto> GetStatisticsTutorsProjects(int matchingId, int tutorId)
        {
            return Connection.Query<ProjectDto>("select " +
                                                "ProjectName " +
                                                ",TechnologiesName_List " +
                                                ",WorkDirectionsName_List " +
                                                ",Info " +
                                                ",Qty " +
                                             
                                             
                                                ",AvailableGroupsName_List " +
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
  
   
        public IEnumerable<StudentStatistics> GetStatisticsStudents(int matchingId, int currentStage)
        {
    

            IEnumerable<StudentStatistics> result = new List<StudentStatistics>();


            if (currentStage == 3)
            {
                result = Connection.Query<StudentStatistics>("select " +
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
                result = Connection.Query<StudentStatistics>("select " +
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
