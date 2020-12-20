using Microsoft.AspNetCore.Mvc;

namespace MatchingSystem.UI.RequestModels
{
    public class ProjectRequest
    {
        public int? projectId { get; set; }
        public int tutorId { get; set; }
        public string name { get; set; }
        public string info { get; set; }
        public string[] technologyList { get; set; }
        public string[] workDirection { get; set; }
        public string quota { get; set; }
        public string[] aviableGroups { get; set; }
    }
}