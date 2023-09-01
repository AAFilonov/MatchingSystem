using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.OldEntities
{
    #nullable enable
    public class Tutor
    {
        [Key]
        public int TutorID { get; set; }
        public int UserID { get; set; }
         //Костыльное имя из хранимки
        public string? TutorNameAbbreviation { get; set; }
        public string? NameAbbreviation { get; set; }
    }
}