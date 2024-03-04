using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class QuotaSumProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var stages = context.Stages.Where(entity => entity.MatchingId == matchingId).ToList();
        var query = context.ActiveCommonQuotas
            .Where(entity => stages.Select(stage => stage.StageId)
                .ToList().Contains<int>((int) entity.StageId))
            .ToList();
        
        var sum = query.Sum(view => view.Qty);
        
        return new StatisticsMain()
        {
            StatName = "Квота: сумма",
            StatValue = sum,
            StatValue_Str = sum.ToString()
        };
    }
}