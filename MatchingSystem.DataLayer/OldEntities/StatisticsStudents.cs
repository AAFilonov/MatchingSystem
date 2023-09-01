#nullable enable
using System;

namespace MatchingSystem.DataLayer.OldEntities
{
    public class StatisticsStudents
    {
        public int? StudentID { get; set; }
        public string? StudentNameAbbreviation { get; set; }
        public DateTime? StudentLastVisitDate { get; set; }
        public string? StudentGroupName { get; set; }
        //этап 3
        public bool?  IsSetInfo { get; set; }
        public bool?  IsSetTechnologies { get; set; }
        public bool?  IsSetWorkDirections { get; set; }
        public int? ProjectCount { get; set; }
        //этап 4
        public bool? ProjectIsAllocated { get; set; }
        public bool? ChoiceIsInQuota { get; set; }
        public bool? ChoiceIsCantAllocated { get; set; }
        public string? PreferenceTypeName_ru { get; set; }

    }
}