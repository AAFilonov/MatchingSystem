using System;
using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class StudentsAllocatedProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var currentStage = context.Stages.Single(entity => entity.MatchingId == matchingId && entity.IsCurrent == true);
        var query = context.Students //TODO DPL-41 фильтр по isDeleted
            .Include(entity => entity.TutorsChoices)
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList(); 
        
        var studentsInQuota = query.Where(view =>
                view.TutorsChoices
                    .Where(choice => choice.StageId == currentStage.StageId)
                    .Any(choice => choice.IsInQuota && choice.Project.IsClosed))
            .ToList();
        var percentage = Math.Round(studentsInQuota.Count / (double) query.Count, 2);
        return new StatisticsMain
        {
            StatName = "Студентов распределено",
            StatValue = studentsInQuota.Count,
            StatValue_Str = $"{studentsInQuota.Count} / {query.Count}",
            StatPercentage = percentage
        };
    }
}

