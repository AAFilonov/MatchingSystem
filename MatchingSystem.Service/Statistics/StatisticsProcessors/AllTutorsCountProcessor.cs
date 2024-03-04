using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class AllTutorsCountProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var queryTutors = context.TutorsViews
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList(); //TODO DPL-41 фильтр по isDeleted
            
        return new StatisticsMain()
        {
            StatName = "Участвуют в распределении: преподавателей",
            StatValue = queryTutors.Count(),
            StatValue_Str = queryTutors.Count().ToString()
        };
    }
}