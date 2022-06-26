using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingMonitoring;

namespace MatchingSystem.Service.Monitoring;

public class MonitoringService : IMonitoringService
{
    public MatchingMonitoringData getMonitoringData(int matchingId)
    {
        //временная заглушка
        return new MatchingMonitoringData()
        {
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
                            quota = null,
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
                            quota = null,
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