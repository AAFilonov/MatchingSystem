using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class QuotaAvgProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var stages = context.Stages.Where(entity => entity.MatchingId == matchingId).ToList();
        var query = context.ActiveCommonQuotas
            .Where(entity => stages.Select(stage => stage.StageId)
                .ToList().Contains<int>((int) entity.StageId))
            .ToList();

        var avg = query.Average(view => view.Qty);

        return new StatisticsMain()
        {
            StatName = "Квота: средняя",
            StatValue_Str = avg.ToString()
        };
    }
}