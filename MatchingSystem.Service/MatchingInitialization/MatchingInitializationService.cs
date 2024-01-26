using System;
using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.CommonQuota.repo;
using MatchingSystem.DataLayer.Feature.Group.repo;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.Role.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.Feature.User.repo;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
using MatchingSystem.DataLayer.Utils;
using Microsoft.Extensions.Logging;
using Scrypt;

namespace MatchingSystem.Service.MatchingInitialization;

[AllArgsConstructor]
public partial class MatchingInitializationService : Repository, IMatchingInitializationService
{
    private readonly ILogger<MatchingInitializationService> logger;
    private readonly IGroupRepository groupRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly IProjectRepository projectRepository;
    private readonly IStudentRepository studentRepository;
    private readonly ITutorRepository tutorRepository;
    private readonly ICommonQuotaRepository quotaRepository;
    private readonly IUserRepository userRepository;
    private readonly IRoleRepository roleRepository;
    private readonly IStageRepository stageRepository;
    
    public MatchingInitData InitNewMatching(MatchingInitData data)
    {
        using var context = _matchingSystemContext;
        using var transaction = context.Database.BeginTransaction();
        try
        {
            logger.LogInformation("Начат процесс обработки распределения {}", data.matching.name);
            var updatedData = TryToInitNewMatching(context, data);
            context.SaveChanges();
            transaction.Commit();
            logger.LogInformation("Процесс обработки распределения {} успешно завершен", data.matching.name);
            return updatedData;
        }
        catch (System.Exception e)
        {
            transaction.Rollback();
            throw new System.Exception("Matching creation error: " + e.Message, e);
        }
    }

    private MatchingInitData TryToInitNewMatching(DiplomaMatchingContext context, MatchingInitData data)
    {
        data.matching.matchingId = InitMatchingEntity(data, context).MatchingId;
        data.matching.firstStageId = InitFirStageEntity(data, context).StageId;
        InitCreatorRoles(context, data);
        InitGroups(context, data);
        InitTutors(context, data);
        InitTutorDefaultProjects(context, data);
        InitStudents(context, data);
        return data;
    }

    private void InitStudents(DiplomaMatchingContext context, MatchingInitData data)
    {
        PrepareStudentDtos(data.studentRecords);
        data.studentRecords = CreateStudentUserEntities(context, data);
        var studentEntities = data.studentRecords.Select(student => new StudentEntity
        {
            GroupId = student.GroupId,
            MatchingId = data.matching.matchingId
        }).ToList();
        studentRepository.saveRange(studentEntities, context);

        for (var i = 0; i < data.studentRecords.Count; i++)
            data.studentRecords[i].StudentId = studentEntities[i].StudentId;
        logger.LogInformation("Операционные записи студентов созданы успешно");

        roleRepository.AssignStudentsRoles(data.studentRecords, data.matching.matchingId, context);
        logger.LogInformation("Назначение ролей студентам выполнено успешно");
    }

    private void InitTutorDefaultProjects(DiplomaMatchingContext context, MatchingInitData data)
    {
        //TODO - кейс распределения магистров
        //создать проекты по умолчанию для преподавателей
        data.tutorRecords =
            projectRepository.CreateDefaultProjectsForTutors(data.tutorRecords, data.matching.matchingId, context);
        logger.LogInformation("Проекты преподавателей по умолчанию созданы успешно");

        //связать проекты по умолчанию с группами
        projectRepository.AssignProjectsToGroups(data.tutorRecords, context);
        logger.LogInformation("Проекты преподавателей  успешно связаны с группами");
    }
    
    private void InitTutors(DiplomaMatchingContext context, MatchingInitData data)
    {
        //TODO DPL-15 - создать преподу пользака если его у него нет
        //инициализировать UserId у преподавателей
        data.tutorRecords = userRepository.SetUserIdForTutors(data.tutorRecords).ToList();
        //создать TutorEntity
        var tutors = data.tutorRecords.Select(info => new TutorEntity
        {
            MatchingId = data.matching.matchingId,
            IsReadyToStart = false
        }).ToList();
        tutorRepository.saveRange(tutors, context);
        for (var i = 0; i < tutors.Count; i++)  data.tutorRecords[i].TutorId = tutors[i].TutorId;
        logger.LogInformation("Записи о преподавателях созданы успешно");

        //Провязать роли для преподавателей 
        roleRepository.AssignTutorRoles(data.tutorRecords, data.matching.matchingId, context);
        logger.LogInformation("Назначение ролей преподавателям выполнено успешно");
        //задать квоты преподавателям
        quotaRepository.CreateCommonQuotas(data.tutorRecords, data.matching.firstStageId, context);
        logger.LogInformation("Квоты преподавателей созданы успешно");
        //создать провязки преподаватель - группа
        groupRepository.AssignGroupsToTutors(data.tutorRecords, context);
        logger.LogInformation("Назначение групп преподавателям выполнено успешно");
    }

    private void InitCreatorRoles(DiplomaMatchingContext context, MatchingInitData data)
    {
        roleRepository.AssignRoleForUser(data.creatorId, data.matching.matchingId, (int)RolesEnum.EXECUTIVE, context);
        roleRepository.AssignRoleForUser(data.creatorId, data.matching.matchingId, (int)RolesEnum.ADMIN, context);
        logger.LogInformation("Роли администратора и отвественного назначены успешно");
    }

    private void InitGroups(DiplomaMatchingContext context, MatchingInitData data)
    {
        var groups = groupRepository.CreateGroups(data, context);
        for (var i = 0; i < data.tutorRecords.Count(); i++)
            data.tutorRecords[i].groups.ForEach(tutorGroup =>
            {
                tutorGroup.groupId = groups.Find(g => g.GroupName == tutorGroup.name).GroupId;
            });
        //записать полученные ID Group в набор студентов
        foreach (var stud in data.studentRecords)
            stud.GroupId = data.groupRecords
                .Where(x => x.name == stud.groupName)
                .Select(x => x.groupId)
                .FirstOrDefault();
        logger.LogInformation("Группы студентов созданы успешно");
    }

    private StageEntity InitFirStageEntity(MatchingInitData data, DiplomaMatchingContext context)
    {
        var firstStage = new StageEntity
        {
            StageTypeId = 1,
            StartDate = DateTime.Now.Add(TimeSpan.FromHours(1)),
            IsCurrent = true,
            MatchingId = data.matching.matchingId
        };
        firstStage = stageRepository.save(firstStage, context);
        logger.LogInformation("Начальная стадия распределения созадана успешно, StageId ={}", firstStage.StageId);
        return firstStage;
    }

    private MatchingEntity InitMatchingEntity(MatchingInitData data, DiplomaMatchingContext context)
    {
        var matching = new MatchingEntity();
        matching.MatchingName = data.matching.name;
        matching.MatchingTypeId = 1;
        matching = matchingRepository.save(matching, context);
        logger.LogInformation("Матчинг созадан успешно, matchingId ={}", data.matching.matchingId);
        return matching;
    }

    private List<StudentInitDto> CreateStudentUserEntities(DiplomaMatchingContext context, MatchingInitData data)
    {
        var views = studentRepository.getStundentsView();
        var studentsToCreate = new List<UserEntity>();

        foreach (var student in data.studentRecords)
        {
            var existingUser = views.Find(view =>
                view.Name == student.firstName &&
                view.Patronimic == student.middleName &&
                view.Surname == student.lastName
            );

            if (existingUser == null)
                studentsToCreate.Add(new UserEntity
                {
                    Login = student.login,
                    PasswordHash = student.passwordHash,
                    Name = student.firstName,
                    Surname = student.lastName,
                    Patronimic = student.middleName
                });
            else
                student.UserId = existingUser.UserId;
        }

        userRepository.saveRange(studentsToCreate, context);
        data.studentRecords
            .Where(student => student.UserId == 0)
            .ToList().ForEach(student =>
            {
                var user = studentsToCreate.Find(user => user.Login == student.login);
                student.UserId = user.UserId;
            });
        logger.LogInformation("Учетные записи студентов созданы успешно");
        return data.studentRecords;
    }

    private void PrepareStudentDtos(List<StudentInitDto> studentRecords)
    {
        foreach (var dto in studentRecords)
        {
            dto.login = UserUtils.Transliterate(dto.lastName + dto.firstName[..1] +
                                                (dto.middleName != null ? dto.middleName[..1] : ""));
            dto.password = UserUtils.CreatePassword(6);
            var encoder = new ScryptEncoder();
            dto.passwordHash = encoder.Encode(dto.password);
        }
    }
}