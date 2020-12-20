namespace MatchingSystem.DataLayer.Entities
{
    #nullable enable
    public class QuotaHistoryExecutive
    {
        public string? NameAbbreviation { get; set; }
        public string? StageTypeName_ru { get; set; }
        public short? RequestedQuotaQty { get; set; }
        public string? QuotaStateName_ru { get; set; }
        public string? Message { get; set; }
        public short? IterationNumber { get; set; }
    }
}
