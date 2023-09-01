using System;
using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.OldEntities
{
#nullable enable
    public class Stage
    {
        [Key]
        public int StageID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndPlanDate { get; set; }
        public short? IterationNumber { get; set; }
        public int? StageTypeCode { get; set; } 
        public string? StageTypeName_ru { get; set; }
    }
}
