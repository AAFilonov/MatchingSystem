using System;

namespace MatchingSystem.DataLayer.Entities
{
    #nullable enable
    public class QuotaHistoryTutor
    {
        public DateTime? CreateDate { get; set; }
        public short Qty { get; set; }
        public string? QuotaStateName_ru { get; set; }
        public string? StageTypeName_ru { get; set; }
        public int? QuotaStateCode { get; set; }
        public short? IterationNumber { get; set; }
    }
}
