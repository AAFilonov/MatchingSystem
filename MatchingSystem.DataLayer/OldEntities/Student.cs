using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.OldEntities
{
    public class Student
    {
        public int? StudentID { get; set; }
        public int? GroupID { get; set; }
        [Display(Name = "Группа")]
        public string GroupName { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        
        public string NameAbbreviation { get; set; }
        public string Patronimic { get; set; }
        [Display(Name = "О себе")]
        public string Info { get; set; }
        [Display(Name = "Группа для магистров")]
        public string Info2 { get; set; }
        [Display(Name = "Интересующие направления")]
        public string WorkDirectionsName_List { get; set; }
        [Display(Name = "Интересующие технологии")]
        public string TechnologiesName_List { get; set; }
    }
}
