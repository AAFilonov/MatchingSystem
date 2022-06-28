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
    //слишком много аргументов
    List<StudentMonitoringDto> getMonitoringDataStudents(
        IEnumerable<Group> useGroups
        ,IEnumerable<DataLayer.Entities.Student> students
        ,IEnumerable<StudentPreferences> studentPreferences
        ,IEnumerable<Project> projects
        ,IEnumerable<ProjectGroupDTO> projectGroups
        ,IEnumerable<TutorChoice> tutorsChoices
        ,IEnumerable<TutorFullDTO> tutors
        )
    {
        List<ProjectMonitoringDto> assignedStudent = new List<ProjectMonitoringDto>();
        foreach (var tutorChoice in tutorsChoices)
        {
            foreach (var stud in students.Where(x => x.StudentID == tutorChoice.StudentID))
            {
                foreach (var proj in projects.Where(x => x.ProjectID == tutorChoice.ProjectID))
                {
                    foreach (var pref in studentPreferences.Where(x => x.StudentID == tutorChoice.StudentID && proj.ProjectID == tutorChoice.ProjectID))
                    {
                        var tutchoice = tutorsChoices.Where(x => x.StudentID == stud.StudentID
                                                                 &&
                                                                 x.ProjectID == proj.ProjectID
                        );
                        var tutor = tutors.Where(x => x.TutorID == proj.TutorID).FirstOrDefault();
                        assignedStudent.Add( new ProjectMonitoringDto()
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
            foreach (var student in students.Where(x => x.GroupID == group.GroupID))
            {
                var studPrefs = new List<ProjectMonitoringDto>();
                foreach (var pref in studentPreferences.Where(x => x.StudentID == student.StudentID))
                {
                    foreach (var proj in projects.Where(x => x.ProjectID == pref.ProjectID))
                    {
                        var tutor = tutors.Where(x => x.TutorID == proj.TutorID).FirstOrDefault();
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
                    nameAbbreviation = student.NameAbbreviation,
                    groupName = group.GroupName, orderInTutorPrefs = null,
                    preferences = studPrefs,
                    assignedProject = assignedStudent.Where(x=>x.assignmentStudentId == student.StudentID).FirstOrDefault()
                });
            }
        }

        return studs;
    }
    
    public MatchingMonitoringData getMonitoringData(int matchingId)
    {
        var studentPreferences = studentRepository.GetStudentPreferencesByMatching(matchingId);
        var students = studentRepository.GetStudentsByMatching(matchingId);
        var groups = groupRepository.getGroupsByMatching(matchingId)
           .Where(x=>students.Select(x=>x.GroupID).Contains(x.GroupID));
        var projects = projectRepository.GetProjectsByMatching(matchingId)
            .Where(x=>studentPreferences.Select(x=>x.ProjectID).Contains(x.ProjectID));
        var tutors = tutorRepository.GetFullInfoTutorByMatching(matchingId).Where(x=>projects.Select(x=>x.TutorID).Contains(x.TutorID));

        var projsGroups = projectRepository.GetProjectsGroupsBtMatching(matchingId);

        var tutorsChoices = tutorRepository.getChoicesByMatchingCurrentStage(matchingId);

        var tutorRecord = new List<TutorMonitoringDto>(); 
        
        
        foreach(var tutorDto in tutors)
        {
            TutorMonitoringDto projMon = new TutorMonitoringDto()
            {
                tutorId = tutorDto.TutorID.Value,
                nameAbbreviation = tutorDto.NameAbbreviation,
                quota = tutorDto.Quota.Value,
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
            
            foreach (var proj in projects.Where(x=>x.TutorID == tutorDto.TutorID))
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
            );
        }
        
        
        return new MatchingMonitoringData()
        {
            studentRecords = getMonitoringDataStudents(groups
                ,students
                ,studentPreferences
                , projects
                ,projsGroups
                ,tutorsChoices
                ,tutors
                ),
           
            tutorRecords = tutorRecord
          
        };
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