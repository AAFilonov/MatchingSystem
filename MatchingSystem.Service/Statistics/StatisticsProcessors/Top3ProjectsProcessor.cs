using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class Top3ProjectsProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.Projects
            .Where(entity => entity.MatchingId == matchingId)
            .Include(entity => entity.StudentsPreferences)
            .Include(entity => entity.Tutor)
            .ThenInclude(entity =>entity.UsersRoles )
            .ThenInclude(entity =>entity.User )
            .OrderByDescending(entity =>entity.StudentsPreferences.Count )
            .ThenBy(entity=>entity.Tutor.UsersRoles.FirstOrDefault().User.Surname)
            .ToList();
        string ShortNameAbbreviation(UserEntity user) => $"{user.Surname} {user.Name[0]}. {user.Patronimic[0]}.";
        
        var value = query
            .Take(3)
            .Select(entity => $"{entity.ProjectName} : " +
                              $"{ShortNameAbbreviation(entity.Tutor.UsersRoles.FirstOrDefault().User)} " +
                              $"({entity.StudentsPreferences.Count()})")
            .ToList();
        
        return new StatisticsMain
        {
            StatName = "Топ 3: Проекты",
            StatValue_Str = string.Join(", ", value),
        };
    }
}

