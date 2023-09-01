using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchingSystem.DataLayer.Dto.MatchingMonitoring;

public class MatchingMonitoringData
{
    public List<StudentMonitoringDto> studentRecords { get; set; } = new();
    public List<TutorMonitoringDto> tutorRecords { get; set; } = new();

    public override string ToString()
    {
        string students = String.Join("\n",studentRecords.Select(student => student.ToString()).ToList());
        string tutors = String.Join("\n",tutorRecords.Select(student => student.ToString()).ToList());
        
        return $"studentRecords: {students},tutorRecords: {tutors}";
    }
}