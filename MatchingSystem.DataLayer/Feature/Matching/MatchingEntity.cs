using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Role;

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

    public override string ToString()
    {
        return $"{nameof(MatchingId)}: {MatchingId}, {nameof(MatchingName)}: {MatchingName}, {nameof(MatchingTypeId)}: {MatchingTypeId}";
    }
    
    protected bool Equals(MatchingEntity other)
    {
        return MatchingId == other.MatchingId && MatchingName == other.MatchingName && MatchingTypeId == other.MatchingTypeId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MatchingEntity)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(MatchingId, MatchingName, MatchingTypeId);
    }

}