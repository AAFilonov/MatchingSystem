using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.TutorChoice;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Statistics.repo;

public class StatisticsRepository : Repository, IStatisticsRepository
{
    public IEnumerable<StatisticsTutors> GetStatisticsTutors(int matchingId, int currentStage)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<ProjectDto> GetStatisticsTutorsProjects(int matchingId, int studentId)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<StudentStatistics> GetStatisticsStudents(int matchingId, int currentStage)
    {
        return CallInContext(context =>
        {
            List<StudentEntity> views = context.Students
                .Where(entity => entity.MatchingId == matchingId)
                //stage 3
                .Include(student => student.StudentsPreferences)
                .Include(student => student.StudentsTechnologies)
                .Include(student => student.StudentsWorkDirections)
                .Include(student => student.Group)
                .Include(student => student.UsersRoles)
                .ThenInclude(entity => entity.User)
                //stage 4
                .Include(student => student.TutorsChoices)
                .ThenInclude(choice => choice.Project)
                .Include(student => student.TutorsChoices)
                .ThenInclude(choice => choice.typeEntity)
                .ToList();

            return views.Select(entity =>
            {
                var statistic = new StudentStatistics();
                //stage 3
                statistic.StudentId = entity.StudentId;
                statistic.StudentNameAbbreviation = entity.UsersRoles.FirstOrDefault().User.NameAbbreviation;
                statistic.StudentLastVisitDate = entity.UsersRoles
                    .FirstOrDefault(ur => ur.StudentId == entity.StudentId)?.LastVisitDate;
                statistic.StudentGroupName = entity.Group.GroupName;
                statistic.IsSetInfo = string.IsNullOrEmpty(entity.Info);
                statistic.IsSetTechnologies = entity.StudentsTechnologies.Any();
                statistic.IsSetWorkDirections = entity.StudentsWorkDirections.Any();
                statistic.ProjectCount = entity.StudentsPreferences.Count;
                //stage 4
                TutorsChoiceEntity? tutorChoice = entity.TutorsChoices.FirstOrDefault(choiceEntity =>
                    choiceEntity.StageId == currentStage
                );
                statistic.ChoiceIsCantAllocated = tutorChoice == null;
                if (tutorChoice != null)
                {
                    statistic.ProjectIsAllocated = tutorChoice.Project.IsClosed;
                    statistic.ChoiceIsInQuota = tutorChoice.IsInQuota;
                    statistic.PreferenceTypeName_ru =
                        tutorChoice.typeEntity.TypeNameRu; //statistic.tutorChoice.typeEntity.TypeNameRu;
                }

                return statistic;
            });
        });
    }

    public IEnumerable<StatisticsStudentsProjects> GetStatisticsStudentsProjects(int matchingId, int studentId)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<StatisticsMain> GetStatisticsGroups(int matchingId)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<TutorsProjectAllocation> GetStatisticsTutorProjectAllocated(int matchingId, int tutorId)
    {
        throw new System.NotImplementedException();
    }
}