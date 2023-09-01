namespace MatchingSystem.DataLayer.Dto
{
    public class ProjectRequest
    {
        public int ProjectId { get; set; }
        public int TutorId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string[] TechnologyList { get; set; }
        public string[] WorkDirection { get; set; }
        public string Quota { get; set; }
        public string[] AviableGroups { get; set; }
    }
}