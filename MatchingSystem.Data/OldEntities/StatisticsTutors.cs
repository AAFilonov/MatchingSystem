#nullable enable
using System;

namespace MatchingSystem.DataLayer.OldEntities
{
    public class StatisticsTutors
    {
        public int? TutorID { get; set; }
        public string? TutorNameAbbreviation { get; set; }
        public bool?  TutorIsReadyToStart { get; set; }
        public int? TutorQuotaQty { get; set; }
        public DateTime? TutorLastVisitDate { get; set; }
        //stage 4
        public int? TutorProjectsAllCount { get; set; }
        public int? TutorProjectsClosedCount { get; set; }
        public bool?  TutorIsSelfChoice { get; set; }
        public bool?  TutorIsAvailableChoice { get; set; }
        public int? TutorStudentsInQuotaCount { get; set; }
        public int? TutorStudentsOutQuotaCount { get; set; }
    }
}