namespace MatchingSystem.Data.Model;

public partial class Group
{
    public Group()
    {
        ProjectsGroups = new HashSet<ProjectsGroup>();
        Students = new HashSet<Student>();
        TutorsGroups = new HashSet<TutorsGroup>();
    }

    public int GroupId { get; set; }
    public int? GroupBk { get; set; }
    public string GroupName { get; set; } = null!;
    public int? MatchingId { get; set; }

    public virtual Matching? Matching { get; set; }
    public virtual ICollection<ProjectsGroup> ProjectsGroups { get; set; }
    public virtual ICollection<Student> Students { get; set; }
    public virtual ICollection<TutorsGroup> TutorsGroups { get; set; }
}