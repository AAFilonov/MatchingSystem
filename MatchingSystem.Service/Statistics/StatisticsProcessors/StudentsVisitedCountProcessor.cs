using System;
using System.Linq;
using Dapper;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class StudentsVisitedCountProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.Students
            .Include(StudentEntity => StudentEntity.UsersRoles)
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList(); //TODO DPL-41 фильтр по isDeleted
        var currentStage = context.Stages.Single(entity => entity.MatchingId == matchingId && entity.IsCurrent == true);
        var visited = query.AsList().Where(student => student.UsersRoles.FirstOrDefault().LastVisitDate > currentStage.StartDate).ToList();
        var percentage = Math.Round(visited.Count / (double) query.Count, 2);
        return new StatisticsMain
        {
            StatName = "Посетили систему на текущем этапе: Студентов",
            StatValue = visited.Count,
            StatValue_Str = $"{visited.Count} / {query.Count}",
            StatPercentage = percentage
        };
    }
}