// ReSharper disable once CheckNamespace

using System.Collections.Generic;
using MatchingSystem.DataLayer.Model;

namespace MatchingSystem.Data.Model;

public class Matching
{
    public Matching()
    {
        Groups = new HashSet<Group>();
        Stages = new HashSet<Stage>();
        UsersRoles = new HashSet<UsersRole>();
    }

    public int MatchingId { get; set; }
    public string MatchingName { get; set; } = null!;
    public int? MatchingTypeId { get; set; }
    public int? CreatorUserId { get; set; }

    public virtual MatchingType? MatchingType { get; set; }
    public virtual ICollection<Group> Groups { get; set; }
    public virtual ICollection<Stage> Stages { get; set; }
    public virtual ICollection<UsersRole> UsersRoles { get; set; }
    
}