using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.OldEntities
{
    #nullable enable
    public class GroupDto
    {
        [Key]
        public int GroupID { get; set; }
        public string? GroupName { get; set; }
    }
}
