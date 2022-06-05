using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.Entities
{
    public class StudentPreferences
    {
        public int? StudentID { get; set; }
        public int? ProjectID { get; set; }
        public int? OrderNumber { get; set; }
    }
}
