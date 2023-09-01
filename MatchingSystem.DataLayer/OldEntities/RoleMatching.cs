namespace MatchingSystem.DataLayer.OldEntities
{
    public class RoleMatching
    {
        public int UserId { get; set; }
        public int RoleCode { get; set; }
        public string RoleName { get; set; }
        public string RoleName_ru { get; set; }
        public int MatchingId { get; set; }
        public string MatchingName { get; set; }
        public int MatchingTypeCode { get; set; }
        public string MatchingTypeName { get; set; }
        public string MatchingTypeName_ru { get; set; }
        public int TutorID { get; set; }
        public int StudentID { get; set; }
    }
}
