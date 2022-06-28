using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.MatchingMonitoring;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;


namespace MatchingSystem.Service.Monitoring;

public class MonitoringService : IMonitoringService
{
    private readonly IStudentRepository studentRepository;
    private readonly ITutorRepository tutorRepository;
    private readonly IProjectRepository projectRepository;
    private readonly IGroupRepository groupRepository;

    public MonitoringService(IStudentRepository studentRepository
                            ,ITutorRepository tutorRepository
                            ,IProjectRepository projectRepository
                            ,IGroupRepository groupRepository
                            )
    {
        this.studentRepository = studentRepository;
        this.tutorRepository = tutorRepository;
        this.projectRepository = projectRepository;
        this.groupRepository = groupRepository;
    }

    List<StudentMonitoringDto> getMonitoringDataStudents(
        IEnumerable<Group> useGroups
        ,IEnumerable<DataLayer.Entities.Student> useStuds
        ,IEnumerable<StudentPreferences> usePrefs
        ,IEnumerable<Project> useProjs
        ,IEnumerable<ProjectGroupDTO> projsGroups
        ,IEnumerable<TutorChoice> tutorsChoices
        ,IEnumerable<TutorFullDTO> useTuts
        )
    {
        List<ProjectMonitoringDto> AssignedStudent = new List<ProjectMonitoringDto>();
        foreach (var tutorChoice in tutorsChoices)
        {
            foreach (var stud in useStuds.Where(x => x.StudentID == tutorChoice.StudentID))
            {
                foreach (var proj in useProjs.Where(x => x.ProjectID == tutorChoice.ProjectID))
                {
                    foreach (var pref in usePrefs.Where(x => x.StudentID == tutorChoice.StudentID && proj.ProjectID == tutorChoice.ProjectID))
                    {
                        var tutchoice = tutorsChoices.Where(x => x.StudentID == stud.StudentID
                                                                 &&
                                                                 x.ProjectID == proj.ProjectID
                        );
                        var tutor = useTuts.Where(x => x.TutorID == proj.TutorID).FirstOrDefault();
                        AssignedStudent.Add( new ProjectMonitoringDto()
                        {
                            assignmentStudentId = stud.StudentID.Value,
                            projectId = proj.ProjectID.Value,
                            projectName = tutor.NameAbbreviation +" : " + proj.ProjectName,
                            quota = ((proj.Qty == null) ? null : proj.Qty.Value),
                            info = proj.Info,
                            availableGroupsNameList = proj.AvailableGroupsName_List,
                            technologiesNameList = proj.TechnologiesName_List,
                            workDirectionsNameList = proj.WorkDirectionsName_List,
                            tutorId = proj.TutorID.Value,
                            orderInStudentPrefs = pref.OrderNumber,
                            isActive = ((tutchoice.Count()==0 || !(tutchoice.Select(x=>x.IsInQuota).Contains(true)))? false:true)
                        });
                    }
                }
            }
        }
        var studs = new List<StudentMonitoringDto>();
        foreach (var group in useGroups)
        {
            foreach (var student in useStuds.Where(x => x.GroupID == group.GroupID))
            {
                var studPrefs = new List<ProjectMonitoringDto>();
                foreach (var pref in usePrefs.Where(x => x.StudentID == student.StudentID))
                {
                    foreach (var proj in useProjs.Where(x => x.ProjectID == pref.ProjectID))
                    {
                        var tutor = useTuts.Where(x => x.TutorID == proj.TutorID).FirstOrDefault();
                        var tutchoice = tutorsChoices.Where(x => x.StudentID == pref.StudentID
                                                                 &&
                                                                 x.ProjectID == proj.ProjectID
                        );
                        studPrefs.Add(
                            new ProjectMonitoringDto()
                            {
                                projectId = proj.ProjectID.Value,
                                projectName = tutor.NameAbbreviation +" : "+ proj.ProjectName,
                                quota = ((proj.Qty == null) ? null : proj.Qty.Value),
                                info = proj.Info,
                                availableGroupsNameList = proj.AvailableGroupsName_List,
                                technologiesNameList = proj.TechnologiesName_List,
                                workDirectionsNameList = proj.WorkDirectionsName_List,
                                tutorId = proj.TutorID.Value,
                                orderInStudentPrefs = pref.OrderNumber,
                                isActive = ((tutchoice.Count()==0 || !(tutchoice.Select(x=>x.IsInQuota).Contains(true)))? false:true)  
                            });
                    }
                }
                
                studs.Add(new()
                {
                    nameAbbreviation = student.Surname + ". " + student.Name.Substring(0, 1) + ". " +
                                       student.Patronimic.Substring(0, 1) + ". ",
                    groupName = group.GroupName, orderInTutorPrefs = null,
                    preferences = studPrefs,
                    assignedProject = AssignedStudent.Where(x=>x.assignmentStudentId == student.StudentID).FirstOrDefault()
                });
            }
        }

        return studs;
    }
    
    public MatchingMonitoringData getMonitoringData(int matchingId)
    {
        var usePrefs = studentRepository.GetStudentPreferencesByMatching(matchingId);
        var useStuds = studentRepository.GetStudentsByMatching(matchingId);
        var useGroups = groupRepository.getGroupsByMatching(matchingId).Result
           .Where(x=>useStuds.Result.Select(x=>x.GroupID).Contains(x.GroupID));
        var useProjs = projectRepository.GetProjectsByMatching(matchingId)
            .Where(x=>usePrefs.Select(x=>x.ProjectID).Contains(x.ProjectID));
        var useTuts = tutorRepository.GetFullInfoTutorByMatching(matchingId).Where(x=>useProjs.Select(x=>x.TutorID).Contains(x.TutorID));

        var projsGroups = projectRepository.GetProjectsGroupsBtMatching(matchingId);

        var tutorsChoices = tutorRepository.getChoicesByMatchingCurrentStage(matchingId);

        var tutorRecord = new List<TutorMonitoringDto>(); 
        
        
        foreach(var tut in useTuts)
        {
            TutorMonitoringDto projMon = new TutorMonitoringDto()
            {
                tutorId = tut.TutorID.Value,
                nameAbbreviation = tut.NameAbbreviation,
                quota = tut.qty.Value,
                projects = new List<ProjectMonitoringDto>(),
                waitingList = new List<StudentMonitoringDto>()
                    {
                        new()
                        {
                            nameAbbreviation = "Иванов И. И",
                            groupName = "19-ИВТ-1",
                            assignedProject = null,
                            orderInTutorPrefs = 1,
                            preferences = new List<ProjectMonitoringDto>()
                            {
                                new()
                                {
                                    projectId = 11,
                                    projectName = "Проект Лагерева 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 1,
                                    orderInStudentPrefs = 1,
                                    isActive = false
                                },
                                new()
                                {
                                    projectId = 13,
                                    projectName = "Проект Дергачева",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 3,
                                    isActive = true
                                },
                            }
                        },
                        new()
                        {
                            nameAbbreviation = "Петров П. П",
                            groupName = "19-ИВТ-2",
                            assignedProject =
                                new()
                                {
                                    projectId = 12,
                                    projectName = "Проект Подвесовского 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 3,
                                    isActive = true
                                },
                            orderInTutorPrefs = 2,
                            preferences = new List<ProjectMonitoringDto>()
                            {
                                new()
                                {
                                    projectId = 11,
                                    projectName = "Проект Лагерева 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 1,
                                    orderInStudentPrefs = 1,
                                    isActive = false
                                }
                            }
                        }
                    }
            };
            
            foreach (var proj in useProjs.Where(x=>x.TutorID == tut.TutorID))
            {
                projMon.projects.Add(
                    new ProjectMonitoringDto()
                    {
                        projectId = proj.ProjectID.Value,
                        projectName = proj.ProjectName,
                        quota = proj.Qty,
                        info = proj.Info,
                        availableGroupsNameList = proj.AvailableGroupsName_List,
                        technologiesNameList = proj.TechnologiesName_List,
                        workDirectionsNameList = proj.WorkDirectionsName_List,
                        tutorId = proj.TutorID.Value,
                        orderInStudentPrefs = null,
                        isActive = null
                    }
                );
            }
            
            tutorRecord.Add(
                projMon
                /*new()
                {
                    tutorId = tut.TutorID.Value,
                    nameAbbreviation = tut.NameAbbreviation,
                    quota = tut.qty.Value,
                    projects = new()
                    {
                        new()
                        {
                            projectId = 12,
                            projectName = "Проект Лагерева 2",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 1,
                            orderInStudentPrefs = null,
                            isActive = null
                        },
                    },
                    waitingList = new List<StudentMonitoringDto>()
                    {
                        new()
                        {
                            nameAbbreviation = "Иванов И. И",
                            groupName = "19-ИВТ-1",
                            assignedProject = null,
                            orderInTutorPrefs = 1,
                            preferences = new List<ProjectMonitoringDto>()
                            {
                                new()
                                {
                                    projectId = 11,
                                    projectName = "Проект Лагерева 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 1,
                                    orderInStudentPrefs = 1,
                                    isActive = false
                                },
                                new()
                                {
                                    projectId = 13,
                                    projectName = "Проект Дергачева",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 3,
                                    isActive = true
                                },
                            }
                        },
                        new()
                        {
                            nameAbbreviation = "Петров П. П",
                            groupName = "19-ИВТ-2",
                            assignedProject =
                                new()
                                {
                                    projectId = 12,
                                    projectName = "Проект Подвесовского 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 3,
                                    isActive = true
                                },
                            orderInTutorPrefs = 2,
                            preferences = new List<ProjectMonitoringDto>()
                            {
                                new()
                                {
                                    projectId = 11,
                                    projectName = "Проект Лагерева 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 1,
                                    orderInStudentPrefs = 1,
                                    isActive = false
                                }
                            }
                        }
                    }
                }*/
                );
        }
        
        
        return new MatchingMonitoringData()
        {
            studentRecords = getMonitoringDataStudents(useGroups
                ,useStuds.Result
                ,usePrefs
                , useProjs
                ,projsGroups.Result
                ,tutorsChoices
                ,useTuts
                ),
            /*
            studentRecords = new List<StudentMonitoringDto>()
            {
                new()
                {
                    nameAbbreviation = "Иванов И. И",
                    groupName = "19-ИВТ-1",
                    assignedProject = null,
                    orderInTutorPrefs = null,
                    preferences = new List<ProjectMonitoringDto>()
                    {
                        new()
                        {
                            projectId = 11,
                            projectName = "Проект Лагерева 1",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 1,
                            orderInStudentPrefs = 1,
                            isActive = false
                        },
                        new()
                        {
                            projectId = 12,
                            projectName = "Проект Подвесовского 1",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 2,
                            orderInStudentPrefs = 2,
                            isActive = true
                        },
                        new()
                        {
                            projectId = 13,
                            projectName = "Проект Дергачева",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 2,
                            orderInStudentPrefs = 3,
                            isActive = true
                        },
                    }
                },
                new()
                {
                    nameAbbreviation = "Петров П. П",
                    groupName = "19-ИВТ-2",
                    assignedProject =
                        new()
                        {
                            projectId = 12,
                            projectName = "Проект Подвесовского 1",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 2,
                            orderInStudentPrefs = 3,
                            isActive = true
                        },
                    orderInTutorPrefs = null,
                    preferences = new List<ProjectMonitoringDto>()
                    {
                        new()
                        {
                            projectId = 11,
                            projectName = "Проект Лагерева 1",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 1,
                            orderInStudentPrefs = 1,
                            isActive = false
                        },
                        new()
                        {
                            projectId = 12,
                            projectName = "Проект Подвесовского 1",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 2,
                            orderInStudentPrefs = 3,
                            isActive = true
                        },
                        new()
                        {
                            projectId = 13,
                            projectName = "Проект Дергачева",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 2,
                            orderInStudentPrefs = 2,
                            isActive = false
                        },
                    }
                }
            },
            */
            tutorRecords = tutorRecord
            /*
            tutorRecords = new List<TutorMonitoringDto>()
            {
                new()
                {
                    tutorId = 1,
                    nameAbbreviation = "Лагерев Д.Г",
                    quota = "3",
                    projects = new()
                    {
                        new()
                        {
                            projectId = 11,
                            projectName = "Проект Лагерева 1",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 1,
                            orderInStudentPrefs = null,
                            isActive = null
                        },
                        new()
                        {
                            projectId = 12,
                            projectName = "Проект Лагерева 2",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 1,
                            orderInStudentPrefs = null,
                            isActive = null
                        },
                    },
                    waitingList = new List<StudentMonitoringDto>()
                    {
                        new()
                        {
                            nameAbbreviation = "Иванов И. И",
                            groupName = "19-ИВТ-1",
                            assignedProject = null,
                            orderInTutorPrefs = 1,
                            preferences = new List<ProjectMonitoringDto>()
                            {
                                new()
                                {
                                    projectId = 11,
                                    projectName = "Проект Лагерева 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 1,
                                    orderInStudentPrefs = 1,
                                    isActive = false
                                },
                                new()
                                {
                                    projectId = 12,
                                    projectName = "Проект Подвесовского 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 2,
                                    isActive = true
                                },
                                new()
                                {
                                    projectId = 13,
                                    projectName = "Проект Дергачева",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 3,
                                    isActive = true
                                },
                            }
                        },
                        new()
                        {
                            nameAbbreviation = "Петров П. П",
                            groupName = "19-ИВТ-2",
                            assignedProject =
                                new()
                                {
                                    projectId = 12,
                                    projectName = "Проект Подвесовского 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 3,
                                    isActive = true
                                },
                            orderInTutorPrefs = 2,
                            preferences = new List<ProjectMonitoringDto>()
                            {
                                new()
                                {
                                    projectId = 11,
                                    projectName = "Проект Лагерева 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 1,
                                    orderInStudentPrefs = 1,
                                    isActive = false
                                },
                                new()
                                {
                                    projectId = 12,
                                    projectName = "Проект Подвесовского 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 3,
                                    isActive = true
                                },
                                new()
                                {
                                    projectId = 13,
                                    projectName = "Проект Дергачева",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 1,
                                    isActive = false
                                },
                            }
                        }
                    }
                },
                 new()
                {
                    tutorId = 1,
                    nameAbbreviation = "Лагерев Д.Г",
                    quota = "3",
                    projects = new()
                    {
                        new()
                        {
                            projectId = 11,
                            projectName = "Проект Лагерева 1",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 1,
                            orderInStudentPrefs = null,
                            isActive = null
                        },
                        new()
                        {
                            projectId = 12,
                            projectName = "Проект Лагерева 2",
                            quota = 2,
                            info = "Проект об разработке на C#",
                            availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                            technologiesNameList = "С#, SQL",
                            workDirectionsNameList = "Разработка, Базы данных",
                            tutorId = 1,
                            orderInStudentPrefs = null,
                            isActive = null
                        },
                    },
                    waitingList = new List<StudentMonitoringDto>()
                    {
                        new()
                        {
                            nameAbbreviation = "Иванов И. И",
                            groupName = "19-ИВТ-1",
                            assignedProject = null,
                            orderInTutorPrefs = 2,
                            preferences = new List<ProjectMonitoringDto>()
                            {
                                new()
                                {
                                    projectId = 11,
                                    projectName = "Проект Лагерева 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 1,
                                    orderInStudentPrefs = 1,
                                    isActive = false
                                },
                                new()
                                {
                                    projectId = 12,
                                    projectName = "Проект Подвесовского 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 2,
                                    isActive = true
                                },
                                new()
                                {
                                    projectId = 13,
                                    projectName = "Проект Дергачева",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 3,
                                    isActive = true
                                },
                            }
                        },
                        new()
                        {
                            nameAbbreviation = "Петров П. П",
                            groupName = "19-ИВТ-2",
                            assignedProject =
                                new()
                                {
                                    projectId = 12,
                                    projectName = "Проект Подвесовского 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 3,
                                    isActive = true
                                },
                            orderInTutorPrefs = 1,
                            preferences = new List<ProjectMonitoringDto>()
                            {
                                new()
                                {
                                    projectId = 11,
                                    projectName = "Проект Лагерева 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 1,
                                    orderInStudentPrefs = 1,
                                    isActive = false
                                },
                                new()
                                {
                                    projectId = 12,
                                    projectName = "Проект Подвесовского 1",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 3,
                                    isActive = true
                                },
                                new()
                                {
                                    projectId = 13,
                                    projectName = "Проект Дергачева",
                                    quota = 2,
                                    info = "Проект об разработке на C#",
                                    availableGroupsNameList = "19-ИВТ-1, 19-ИВТ-2",
                                    technologiesNameList = "С#, SQL",
                                    workDirectionsNameList = "Разработка, Базы данных",
                                    tutorId = 2,
                                    orderInStudentPrefs = 1,
                                    isActive = false
                                },
                            }
                        }
                    }
                }
            }
            */
        };

        //return new MatchingMonitoringData()
        //{
        //    studentRecords = getStudentsMonitoringData(matchingId),
        //    tutorRecords = getTutorsMonitoringData(matchingId),
        //};
    }

    public List<StudentMonitoringDto> getStudentsMonitoringData(int matchingId)
    {
        throw new System.NotImplementedException();
    }

    public List<TutorMonitoringDto> getTutorsMonitoringData(int matchingId)
    {
        throw new System.NotImplementedException();
    }
}