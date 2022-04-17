using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.Entities
{
    #nullable enable
    public class Tutor
    {
        [Key]
        public int TutorID { get; set; }
        public string? TutorNameAbbreviation { get; set; }
    }
}