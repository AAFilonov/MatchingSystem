using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class ProjectsPerTutorAvg : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.Projects
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList();
        
        var queryTutor = context.Tutors
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList();

        var avg = query.Count / queryTutor.Count;
        return new StatisticsMain()
        {
            StatName = "Проектов: в среднем на преподавателя",
            StatValue = avg,
            StatValue_Str = avg.ToString()
        };
    }
}