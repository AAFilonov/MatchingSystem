// ReSharper disable once CheckNamespace
namespace MatchingSystem.Data.Model;

public partial class Matching
{
    public Matching()
    {
        Groups = new HashSet<Group>();
        Stages = new HashSet<Model.Stage>();
        UsersRoles = new HashSet<UsersRole>();
    }

    public int MatchingId { get; set; }
    public string MatchingName { get; set; } = null!;
    public int? MatchingTypeId { get; set; }
    public int? CreatorUserId { get; set; }

    public virtual MatchingType? MatchingType { get; set; }
    public virtual ICollection<Group> Groups { get; set; }
    public virtual ICollection<Model.Stage> Stages { get; set; }
    public virtual ICollection<UsersRole> UsersRoles { get; set; }
    
}