using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using MatchingSystem.Data.Model;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml.ConditionalFormatting;

namespace MatchingSystem.Service.MatchingInitialization;

public class MatchingInitializationService   : IMatchingInitializationService
{
    private readonly IMatchingRepository matchingRepository;
    private readonly IUserRepository userRepository;
    private readonly IStudentRepository studentRepository;
    private readonly ITutorRepository tutorRepository;
    private readonly IGroupRepository groupRepository;
    private readonly IProjectRepository projectRepository;

    public MatchingInitializationService(IMatchingRepository matchingRepository
            ,IUserRepository userRepository
            ,IStudentRepository studentRepository
            ,IGroupRepository groupRepository
            ,ITutorRepository tutorRepository
            ,IProjectRepository projectRepository
            )
    {
        this.matchingRepository = matchingRepository;
        this.userRepository = userRepository;
        this.studentRepository = studentRepository;
        this.groupRepository = groupRepository;
        this.tutorRepository = tutorRepository;
        this.projectRepository = projectRepository;
    }
    public void createMatching(MatchingInitData data,int creatorUserId)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        
        //создать Mathching, Stage
        var MatchingId = matchingRepository.SetNewMatching(data.matching).Result;
        var StageId = matchingRepository.SetNewFirstStageInMatching(MatchingId).Result;
        //установить ответственным запустившего создание нового распределения
        userRepository.SetUser_Role(creatorUserId, MatchingId);
        
        //создать группы на базе, получить id записей
        foreach (var group in data.groupRecords)
        {
            group.groupId = groupRepository.SetNewGroup(group.name,MatchingId).Result;
        }
        
        //записать полученные ID Group в набор студентов
        foreach (var stud in data.studentRecords)
        {
            stud.GroupId = data.groupRecords.Where(x => x.name == stud.groupName).Select(x => x.groupId).FirstOrDefault();
        }
        //записать полученные ID Group в набор преподавателей
        foreach (var tut in data.tutorRecords)
        {
            //tut.groups.Remove(tut.groups.FirstOrDefault(x => x.name == null));
            foreach (var tutGroup in tut.groups)
            {
                tutGroup.groupId = data.groupRecords.Where(x => x.name == tutGroup.name).Select(x => x.groupId).FirstOrDefault();    
            }
        }

        //создать учетные записи для студентов
        data.studentRecords = userRepository.SetNewUsersForStudents(data.studentRecords).ToList();
        
        //создать на базе Студентов
        data.studentRecords = studentRepository.SetNewStudents(data.studentRecords,MatchingId).ToList();
        
        //Провязать роли для студентов 
        studentRepository.setNewUserRoles_Students(data.studentRecords,MatchingId);

        //найти Id преподавателей по NameAbbreviation
        data.tutorRecords = userRepository.GetTutorUsers(data.tutorRecords).ToList();
        
        //Создать Tutors на данный Matching
        data.tutorRecords = tutorRepository.SetNewTutors(data.tutorRecords,MatchingId).ToList();
        //Провязать роли для преподавателей 
        tutorRepository.setNewUserRoles_Tutors(data.tutorRecords, MatchingId);
        
        //задать квоты преподавателям
        tutorRepository.SetCommonQuotasForTutors(data.tutorRecords, StageId);
        
        //создать провязки преподаватель - группа
        groupRepository.SetNew_Tutors_Groups(data.tutorRecords, MatchingId);
        
        //создать проекты по умолчанию для преподавателей
        data.tutorRecords = projectRepository.SetDefaultProjectsForTutors(data.tutorRecords, MatchingId).ToList();

        //связать проекты по умолчанию с группами
        projectRepository.SetDefaultProjects_Groups(data.tutorRecords);

        //перейти к следующему шагу
        //переключиться в новое распределение
        //SetNewFirstStageInMatching(MatchingId);
    }
}