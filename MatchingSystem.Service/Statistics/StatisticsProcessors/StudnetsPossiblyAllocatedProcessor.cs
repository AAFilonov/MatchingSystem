using System;
using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class StudentsPossiblyAllocatedProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var currentStage = context.Stages.Single(entity => entity.MatchingId == matchingId && entity.IsCurrent == true);
        var query = context.Students
            .Include(entity => entity.TutorsChoices)
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList();  //TODO DPL-41 фильтр по isDeleted
        
        var studentsInQuota = query.Where(view =>
                view.TutorsChoices
                    .Where(choice => choice.StageId == currentStage.StageId)
                    .Any(choice => choice.IsInQuota && !choice.Project.IsClosed))
            .ToList();
        var percentage = Math.Round(studentsInQuota.Count / (double) query.Count, 2);
        return new StatisticsMain
        {
            StatName = "Студентов потенциально распределено",
            StatValue = studentsInQuota.Count,
            StatValue_Str = $"{studentsInQuota.Count} / {query.Count}",
            StatPercentage = percentage
        };
    }
}

