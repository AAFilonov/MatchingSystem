namespace MatchingSystem.DataLayer.Entities
{
#nullable enable
    public class Matching
    {
        public int? MatchingID { get; set; }
        public int? MatchingTypeID { get; set; }
        public string? MatchingName { get; set; }
        public string? MatchingTypeCode { get; set; }
        public string? MatchingTypeName { get; set; }
        public string? MatchingTypeName_ru { get; set; }
    }
}