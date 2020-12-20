using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.Entities
{
    #nullable enable
    public class Technology
    {
        [Key]
        public int TechnologyCode { get; set; }
        public string? TechnologyName_ru { get; set; }
    }
}
