using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.Feature.Group
{
    #nullable enable
    public class GroupDto
    {
        public GroupDto(GroupEntity entity)
        {
            GroupID = entity.GroupId;
            GroupName = entity.GroupName;
        }
        
        [Key]
        public int GroupID { get; set; }
        public string? GroupName { get; set; }
    }
}
