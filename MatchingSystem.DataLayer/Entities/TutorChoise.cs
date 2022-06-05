using System;

namespace MatchingSystem.DataLayer.Entities
{
    #nullable enable
    public class TutorChoice
    {
        public int? ChoiceID { get; set; }
        public int? StudentID { get; set; }
        public string? StudentNameAbbreviation { get; set; }
        public string? GroupName { get; set; }
        public int? ProjectID { get; set; }
        public string? ProjectName { get; set; }
        public bool? IsInQuota { get; set; }
        public short? Qty { get; set; }
        public short? SortOrderNumber { get; set; }
        public bool? ProjectIsClosed { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? TypeCode { get; set; }
    }
}
