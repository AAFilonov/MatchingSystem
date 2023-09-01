using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Model;

public class Tutor
{
    public Tutor()
    {
        CommonQuota = new HashSet<CommonQuota>();
        Projects = new HashSet<Project>();
        TutorsGroups = new HashSet<TutorsGroup>();
        UsersRoles = new HashSet<UsersRole>();
    }

    public int TutorId { get; set; }
    public int? TutorBk { get; set; }
    public bool IsClosed { get; set; }
    public short? CloseIterationNumber { get; set; }
    public bool? IsReadyToStart { get; set; }
    public int? MatchingId { get; set; }

    public virtual ICollection<CommonQuota> CommonQuota { get; set; }
    public virtual ICollection<Project> Projects { get; set; }
    public virtual ICollection<TutorsGroup> TutorsGroups { get; set; }
    public virtual ICollection<UsersRole> UsersRoles { get; set; }
}