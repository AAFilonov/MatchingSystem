using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Group.repo;
using MatchingSystem.DataLayer.Feature.Role.repo;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.Feature.User.repo;
using MatchingSystem.DataLayer.Model;
using MatchingSystem.DataLayer.Utils;
using MatchingSystem.Service.Exception;
using MatchingSystem.Service.Matching;
using Scrypt;

namespace MatchingSystem.Service.MatchingInitialization;

public class MatchingInitializationService : IMatchingInitializationService
{
    private readonly IGroupRepository groupRepository;
    private readonly IMatchingService matchingService;
    private readonly IProjectRepository projectRepository;
    private readonly IStudentRepositoryOld studentRepositoryOld;
    private readonly ITutorRepository tutorRepository;
    private readonly ICommonQuotaRepository quotaRepository;
    //private readonly IUserRepositoryOld userRepositoryOld;
    private readonly IUserRepository userRepository;
    private readonly IRoleRepository roleRepository;

    public MatchingInitializationService(
        IStudentRepositoryOld studentRepositoryOld
        , IGroupRepository groupRepository
        , ITutorRepository tutorRepository
        , IProjectRepository projectRepository
        , IMatchingService matchingService, IUserRepository userRepository, IRoleRepository roleRepository, ICommonQuotaRepository quotaRepository)
    {

        this.studentRepositoryOld = studentRepositoryOld;
        this.groupRepository = groupRepository;
        this.tutorRepository = tutorRepository;
        this.projectRepository = projectRepository;
        this.matchingService = matchingService;
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
        this.quotaRepository = quotaRepository;
    }

    public MatchingInitData createMatching(MatchingInitData data, int creatorUserId)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        MatchingInitData matching;
        try
        {
            matching = TryCreateMatching(data, creatorUserId);
            //            transactionScope.Complete();
            return matching;
            //TODO DPL-17 транзакционность через EF, тк в Dapper она не работает
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
            {
                matching = TryCreateMatching(data, creatorUserId);
                transactionScope.Complete();
                return matching;
            }
        }
        catch (TransactionAbortedException ex)
        {
            return data;
        }
        catch (System.Exception e)
        {
            throw new InputDataException("Unknown matching creation error", e);
        }
    }

    private MatchingInitData TryCreateMatching(MatchingInitData data, int creatorUserId)
    {
        //TODO DPL-16
        //просетить userId преподавателей по NameAbbreviation
        data.tutorRecords = userRepository.SetUserIdForTutors(data.tutorRecords).ToList();

        //создать Mathching, Stage
        var MatchingId = matchingService.CreateMatching(data.matching);
        var StageId = matchingService.createFirstStage(MatchingId);
        roleRepository.AssignRoleForUser(creatorUserId, MatchingId, 3);

        var groups = data.groupRecords.Select(item => new GroupEntity()
        {
            GroupName = item.name,
            MatchingId = MatchingId,
        });
        groupRepository.saveAll(groups);


        //записать полученные ID Group в набор студентов
        foreach (var stud in data.studentRecords)
            stud.GroupId = data.groupRecords.Where(x => x.name == stud.groupName).Select(x => x.groupId)
                .FirstOrDefault();

        //записать полученные ID Group в набор преподавателей
        foreach (var tut in data.tutorRecords)
        {
            tut.groups.ForEach(group =>
            {
                group.groupId = data.groupRecords.Where(x => x.name == group.name).Select(x => x.groupId)
                    .FirstOrDefault();
            });
        }

        //создать учетные записи для студентов 
        data.studentRecords = ctreateUsersForStudents(data);//TODO - этот шаг очень медленный, нужно переписать;

        //Создать Tutors на данный Matching
        data.tutorRecords = tutorRepository.CreateTutors(data.tutorRecords, MatchingId).ToList();

        //Провязать роли для преподавателей 
        roleRepository.AssignTutorRole(data.tutorRecords, MatchingId); //TODO реализваоно по старому

        //задать квоты преподавателям
        quotaRepository.SetCommonQuotasForTutors(data.tutorRecords, StageId); //TODO реализваоно по старому

        //создать провязки преподаватель - группа
        groupRepository.AssignGroupsToTutors(data.tutorRecords); //TODO реализваоно по старому


        projectRepository.CreateDefaultTutorsProjects(data.tutorRecords); //TODO реализваоно по старому


        //создать проекты по умолчанию для преподавателей
        data.tutorRecords = projectRepository.CreateDefaultProjectsForTutors(data.tutorRecords, MatchingId) //TODO реализваоно по старому
            .ToList();
        //связать проекты по умолчанию с группами
        projectRepository.SetDefaultProjects_Groups(data.tutorRecords); //TODO реализваоно по старому

        //создать на базе Студентов
        data.studentRecords = studentRepositoryOld.CreateStudents(data.studentRecords, MatchingId).ToList();

        //Провязать роли для студентов 
        studentRepositoryOld.CreateUserRoles_Students(data.studentRecords, MatchingId); //TODO реализваоно по старому

        //перейти к следующему шагу
        //переключиться в новое распределение
        //SetNewFirstStageInMatching(MatchingId);
        //    transactionScope.Complete();
        //}


        return data;
    }

    private List<StudentInitDto> ctreateUsersForStudents(MatchingInitData data)
    {
        foreach (var dto in data.studentRecords)
        {
            dto.login = UserUtils.Transliterate(dto.lastName + dto.firstName.Substring(0, 1) +
                                                (dto.middleName != null ? dto.middleName.Substring(0, 1) : ""));
            //if (userRepository.GetByLogin(dto.login) != null) dto.login += "2023";

            dto.userBK = dto.UserId;
            dto.password = UserUtils.CreatePassword(6);

            var encoder = new ScryptEncoder();

            dto.passwordHash = encoder.Encode(dto.password);
        }

        return userRepository.SaveStudents(data.studentRecords).ToList();
    }

}