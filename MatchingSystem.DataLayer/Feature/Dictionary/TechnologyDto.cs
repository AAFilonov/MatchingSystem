using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.OldEntities
{
    #nullable enable
    public class TechnologyDto
    {
        [Key]
        public int TechnologyCode { get; set; }
        public string? TechnologyName_ru { get; set; }
    }
}
