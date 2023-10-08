using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Model;

namespace MatchingSystem.DataLayer.Feature.Matching;

public class MatchingEntity
{
    public MatchingEntity()
    {
        Groups = new HashSet<GroupEntity>();
        Stages = new HashSet<Stage.StageEntity>();
        UsersRoles = new HashSet<UsersRoleEntity>();
    }

    public int MatchingId { get; set; }
    public string MatchingName { get; set; } = null!;
    public int MatchingTypeId { get; set; }
    public int? CreatorUserId { get; set; }
    
    [Required]
    public virtual MatchingType MatchingType { get; set; }
    public virtual ICollection<GroupEntity> Groups { get; set; }
    public virtual ICollection<Stage.StageEntity> Stages { get; set; }
    public virtual ICollection<UsersRoleEntity> UsersRoles { get; set; }
    
}