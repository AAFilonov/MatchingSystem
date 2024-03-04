using System;
using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class StudentsCantBeAllocatedProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.Students
            .Include(entity => entity.TutorsChoices)
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList();
        var studentsWithoutPrefs = query
            .Where(entity => entity.StudentsPreferences.Count == 0)
            .ToList();
  
        var studentsRejectedOnEveryProjects = query
            .Where(entity => entity.StudentsPreferences.Count > 0 && entity.StudentsPreferences
                .All(pref => 
                    pref.IsUsed == true ||
                    pref.IsInUse == false && 
                    pref.Project.IsClosed)
            ).ToList();
        
        double value = studentsWithoutPrefs.Count + studentsRejectedOnEveryProjects.Count();
        var percentage = Math.Round(value / query.Count, 2);
        return new StatisticsMain
        {
            StatName = "Студентов не может быть распределено",
            StatValue = value,
            StatValue_Str = $"{value} / {query.Count}",
            StatPercentage = percentage
        };
    }
}

