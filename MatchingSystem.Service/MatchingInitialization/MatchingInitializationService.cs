using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.Service.Exception;
using Scrypt;

namespace MatchingSystem.Service.MatchingInitialization;

public class MatchingInitializationService : IMatchingInitializationService
{
    private readonly IGroupRepository groupRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly IProjectRepository projectRepository;
    private readonly IStudentRepository studentRepository;
    private readonly ITutorRepository tutorRepository;
    private readonly IUserRepository userRepository;

    public MatchingInitializationService(IMatchingRepository matchingRepository
        , IUserRepository userRepository
        , IStudentRepository studentRepository
        , IGroupRepository groupRepository
        , ITutorRepository tutorRepository
        , IProjectRepository projectRepository
    )
    {
        this.matchingRepository = matchingRepository;
        this.userRepository = userRepository;
        this.studentRepository = studentRepository;
        this.groupRepository = groupRepository;
        this.tutorRepository = tutorRepository;
        this.projectRepository = projectRepository;
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
            //Транзации не работают но распределение ссоздать можно
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
        //просетить userId преподавателей по NameAbbreviation
        data.tutorRecords = userRepository.SetUserIdForTutors(data.tutorRecords).ToList();

        //создать Mathching, Stage
        var MatchingId = matchingRepository.CreateMatching(data.matching);
        var StageId = matchingRepository.SetNewFirstStageInMatching(MatchingId);
        userRepository.AssignRoleForUser(creatorUserId, MatchingId);

        //создать группы на базе, получить id записей
        foreach (var group in data.groupRecords) group.groupId = groupRepository.CreateGroup(group.name, MatchingId);


        //записать полученные ID Group в набор студентов
        foreach (var stud in data.studentRecords)
            stud.GroupId = data.groupRecords.Where(x => x.name == stud.groupName).Select(x => x.groupId)
                .FirstOrDefault();

        //записать полученные ID Group в набор преподавателей
        foreach (var tut in data.tutorRecords)
            //tut.groups.Remove(tut.groups.FirstOrDefault(x => x.name == null));
        foreach (var tutGroup in tut.groups)
            tutGroup.groupId = data.groupRecords.Where(x => x.name == tutGroup.name).Select(x => x.groupId)
                .FirstOrDefault();

        //создать учетные записи для студентов
        data.studentRecords = ctreateUsersForStudents(data);

        //Создать Tutors на данный Matching
        data.tutorRecords = tutorRepository.CreateTutors(data.tutorRecords, MatchingId).ToList();

        //Провязать роли для преподавателей 
        tutorRepository.AssignTutorRole(data.tutorRecords, MatchingId);

        //задать квоты преподавателям
        tutorRepository.SetCommonQuotasForTutors(data.tutorRecords, StageId);

        //создать провязки преподаватель - группа
        groupRepository.AssignGroupsToTutors(data.tutorRecords, MatchingId);


        // projectRepository.CreateDefaultTutorsProjects(data.tutorRecords);


        //создать проекты по умолчанию для преподавателей
        data.tutorRecords = projectRepository.CreateDefaultProjectsForTutors(data.tutorRecords, MatchingId)
            .ToList();
        //связать проекты по умолчанию с группами
        projectRepository.SetDefaultProjects_Groups(data.tutorRecords);

        //создать на базе Студентов
        data.studentRecords = studentRepository.CreateStudents(data.studentRecords, MatchingId).ToList();

        //Провязать роли для студентов 
        studentRepository.CreateUserRoles_Students(data.studentRecords, MatchingId);

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
            dto.login = Transliterate(dto.lastName + dto.firstName.Substring(0, 1) +
                                      (dto.middleName != null ? dto.middleName.Substring(0, 1) : ""));
            if (userRepository.GetUser(dto.login) != null)
            {
                dto.login += "2023";
            }

            dto.userBK = dto.UserId; 
            dto.password = CreatePassword(6);
            
            var encoder = new ScryptEncoder();

            dto.passwordHash = encoder.Encode(dto.password);
        }

        return userRepository.CreateUsersForStudents(data.studentRecords).ToList();
    }

    public static string Transliterate(string str)
    {
        string[] lat_up =
        {
            "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U",
            "F", "Kh", "Ts", "Ch", "Sh", "Shch", "", "Y", "", "E", "Yu", "Ya"
        };
        string[] lat_low =
        {
            "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u",
            "f", "kh", "ts", "ch", "sh", "shch", "", "y", "", "e", "yu", "ya"
        };
        string[] rus_up =
        {
            "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У",
            "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я"
        };
        string[] rus_low =
        {
            "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у",
            "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я"
        };
        for (var i = 0; i <= 32; i++)
        {
            str = str.Replace(rus_up[i], lat_up[i]);
            str = str.Replace(rus_low[i], lat_low[i]);
        }

        return str;
    }


    public string CreatePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var res = new StringBuilder();
        var rnd = new Random();
        while (0 < length--) res.Append(valid[rnd.Next(valid.Length)]);
        return res.ToString();
    }
}