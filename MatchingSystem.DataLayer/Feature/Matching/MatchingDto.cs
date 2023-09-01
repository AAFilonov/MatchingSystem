namespace MatchingSystem.DataLayer.Feature.Matching
{
#nullable enable
    public class MatchingDto
    {
        public MatchingDto(Matching matching)
        {
            MatchingID = matching.MatchingId;
            MatchingName = matching.MatchingName;
            MatchingTypeCode = matching?.MatchingType?.MatchingTypeCode;
            MatchingTypeName = matching?.MatchingType?.MatchingTypeName;
            MatchingTypeName_ru = matching?.MatchingType?.MatchingTypeNameRu;
        }

        public int? MatchingID { get; set; }
        public int? MatchingTypeID { get; set; }
        public string? MatchingName { get; set; }
        public string? MatchingTypeCode { get; set; }
        public string? MatchingTypeName { get; set; }
        public string? MatchingTypeName_ru { get; set; }
    }
}