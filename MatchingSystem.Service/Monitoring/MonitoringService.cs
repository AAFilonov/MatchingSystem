using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingSystem.DataLayer.Feature;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.TutorChoice;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.OldDto.MatchingMonitoring;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Service.Monitoring;

[AllArgsConstructor]
public partial class MonitoringService : Repository, IMonitoringService
{
    private readonly IMatchingRepository matchingRepository;

    public MatchingMonitoringData getMonitoringData(int matchingId)
    {
        var data = new MatchingMonitoringData();
        data.projectRecords = getProjectsMonitoringData(matchingId);
        data.studentRecords = getStudentsMonitoringData(matchingId);
        data.tutorRecords = getTutorsMonitoringData(matchingId);
        return data;
    }

    private List<ProjectMonitoringDto> getProjectsMonitoringData(int matchingId)
    {
        StageEntity currentStage = matchingRepository.GetCurrentStage(matchingId);
        return CallInContext(context =>
        {
            var students = context.Projects
                .Where(entity => entity.MatchingId == matchingId)
                .Include(entity => entity.TutorsChoices)
                .Include(entity => entity.ProjectsWorkDirections)
                .ThenInclude(entity => entity.directionEntity)
                .Include(entity => entity.ProjectsTechnologies)
                .ThenInclude(entity => entity.Technology)
                .Include(entity => entity.ProjectsGroups)
                .ThenInclude(entity => entity.groupEntity)
                .ToList();
            return students.Select(entity =>
            {
                var dto = new ProjectMonitoringDto();
                dto.projectId = entity.ProjectId;
                dto.projectName = entity.ProjectName;
                dto.quota = entity.ProjectQuotaQty?.ToString() ?? "Не важно";
                dto.info = entity.Info;
                dto.isClosed = entity.IsClosed;
                dto.tutorId = entity.TutorId;
                var tutorChoises = entity.TutorsChoices
                    .Where(choiceEntity => choiceEntity.StageId == currentStage.StageId)
                    .ToList();
                dto.assignedStudentsIds = tutorChoises.Select(choiceEntity => choiceEntity.StudentId).ToList();
                dto.assignedStudentsIdsInQuota = tutorChoises
                    .Where(entity => entity.IsInQuota)
                    .Select(choiceEntity => choiceEntity.StudentId).ToList();
                var availableGroupsNameList =
                    entity.ProjectsGroups.Select(linkRecod => linkRecod.groupEntity.GroupName).ToList();
                dto.availableGroupsNameList = string.Join(",", availableGroupsNameList);
                var technologiesNameList = entity.ProjectsTechnologies
                    .Select(linkRecod => linkRecod.Technology.TechnologyNameRu).ToList();
                dto.technologiesNameList = string.Join(",", technologiesNameList);
                var workDirectionsNameList = entity.ProjectsWorkDirections
                    .Select(linkRecod => linkRecod.directionEntity.DirectionNameRu).ToList();
                dto.workDirectionsNameList = string.Join(",", workDirectionsNameList);
                return dto;
            }).ToList();
        });
    }


    public List<StudentMonitoringDto> getStudentsMonitoringData(int matchingId)
    {
        StageEntity currentStage = matchingRepository.GetCurrentStage(matchingId);
        return CallInContext(context =>
        {
            var students = context.Students
                .Where(entity => entity.MatchingId == matchingId)
                .Include(student => student.StudentsPreferences)
                .Include(student => student.Group)
                .Include(student => student.UsersRoles)
                .ThenInclude(entity => entity.User)
                .Include(student => student.TutorsChoices)
                .ToList();
            return students.Select(entity =>
            {
                var dto = new StudentMonitoringDto();
                dto.studentId = entity.StudentId;
                dto.groupName = entity.Group.GroupName;
                dto.nameAbbreviation = entity.UsersRoles.FirstOrDefault().User.NameAbbreviation;
                dto.preferenceProjectIds = entity.StudentsPreferences
                    .OrderBy(preferenceEntity => preferenceEntity.OrderNumber)
                    .Select(project => project.ProjectId)
                    .ToList();
                dto.alreadyTryedProjectIds = entity.StudentsPreferences
                    .Where(entity => (bool)entity.IsUsed)
                    .OrderBy(preferenceEntity => preferenceEntity.OrderNumber)
                    .Select(project => project.ProjectId)
                    .ToList();
                TutorsChoiceEntity tutorsChoice =
                    entity.TutorsChoices.FirstOrDefault(choiceEntity => choiceEntity.StageId == currentStage.StageId);
                dto.assignedProjectId = tutorsChoice?.ProjectId;
                var tutorPrefsOnCurrentStage = tutorsChoice?.Project?.TutorsChoices
                    .Where(choiceEntity => choiceEntity.StageId == currentStage.StageId).ToList();
                dto.orderInTutorPrefs = tutorsChoice?.SortOrderNumber == 32767
                    ? null
                    : tutorPrefsOnCurrentStage?.FindIndex(choice => choice.StudentId == dto.studentId) + 1;
                return dto;
            }).ToList();
        });
    }

    public List<TutorMonitoringDto> getTutorsMonitoringData(int matchingId)
    {
        StageEntity currentStage = matchingRepository.GetCurrentStage(matchingId);
        return CallInContext(context =>
        {
            var tutors = context.Tutors
                .Where(tutor => tutor.MatchingId == matchingId)
                .Include(tutor => tutor.Projects)
                .ThenInclude(project => project.TutorsChoices)
                .Include(tutor => tutor.UsersRoles)
                .ThenInclude(tutor => tutor.User)
                .Include(tutor => tutor.CommonQuota)
                .ToList();
            return tutors.Select(entity =>
            {
                var dto = new TutorMonitoringDto();
                dto.tutorId = entity.TutorId;
                UserEntity user = entity.UsersRoles.FirstOrDefault().User;

                dto.nameAbbreviation = $"{user.Surname} {user.Name[0]}.{user.Patronimic[0]}.";
                dto.quota = entity.CommonQuota
                    .SingleOrDefault(quota => quota.QuotaStateId == (int)CommonQuotaStateEnum.ACTIVE)?.Qty;
                dto.projects = entity.Projects.Select(projectEntity => projectEntity.ProjectId).ToList();
                dto.waitingList = entity.Projects
                    .SelectMany(project => project.TutorsChoices)
                    .Where(choice => choice.StageId == currentStage.StageId)
                    .Select(choice => choice.StudentId).ToList();
                return dto;
            }).ToList();
        });
    }
}