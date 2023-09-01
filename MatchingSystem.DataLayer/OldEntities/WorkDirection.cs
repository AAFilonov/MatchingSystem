using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.OldEntities
{
    #nullable enable
    public class WorkDirection
    {
        [Key]
        public int DirectionCode { get; set; }
        public string? DirectionName_ru { get; set; }
    }
}
