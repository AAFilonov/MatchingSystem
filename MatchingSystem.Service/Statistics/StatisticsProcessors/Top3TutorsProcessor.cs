using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class Top3TutorsProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.Tutors
            .Where(entity => entity.MatchingId == matchingId)
            .Include(entity => entity.UsersRoles)
            .ThenInclude(entity => entity.User)
            .Include(entity => entity.Projects)
            .ThenInclude(entity => entity.StudentsPreferences)
            .OrderByDescending(entity => entity.Projects.SelectMany(project => project.StudentsPreferences).Count())
            .ThenBy(entity => entity.UsersRoles.FirstOrDefault().User.Surname)
            .ToList();
        string ShortNameAbbreviation(UserEntity user) => $"{user.Surname} {user.Name[0]}. {user.Patronimic[0]}.";

        var value = query
            .Take(3)
            .Select(entity => $"{ShortNameAbbreviation(entity.UsersRoles.FirstOrDefault().User)} " +
                              $"({entity.Projects.SelectMany(project => project.StudentsPreferences).Count()})")
            .ToList();

        return new StatisticsMain
        {
            StatName = "Топ 3: Преподаватели",
            StatValue_Str = string.Join(", ", value),
        };
    }
}