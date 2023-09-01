namespace MatchingSystem.DataLayer.Dto.IO.Request
{
    public class ProjectRequest
    {
        public int TutorId { get; set; }
        public string ProjectName { get; set; }
        public string Info { get; set; }
        public string CommaSeparatedTechList { get; set; }
        public string CommaSeparatedWorkList { get; set; }
        public string CommaSeparatedGroupList { get; set; }
        public string Quota { get; set; }
        public int ProjectId { get; set; }
    }
}
