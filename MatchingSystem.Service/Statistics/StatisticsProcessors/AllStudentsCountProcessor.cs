using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class AllStudentsCountProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.Students
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList();//TODO DPL-41 фильтр по isDeleted
            
        return new StatisticsMain()
        {
            StatName = "Участвуют в распределении: студентов",
            StatValue = query.Count,
            StatValue_Str = query.Count.ToString()
        };
    }
}