namespace MatchingSystem.Data.Model;

public partial class Technology
{
    public Technology()
    {
        ProjectsTechnologies = new HashSet<ProjectsTechnology>();
        StudentsTechnologies = new HashSet<StudentsTechnology>();
    }

    public int TechnologyId { get; set; }
    public string TechnologyNameRu { get; set; } = null!;
    public int TechnologyCode { get; set; }

    public virtual ICollection<ProjectsTechnology> ProjectsTechnologies { get; set; }
    public virtual ICollection<StudentsTechnology> StudentsTechnologies { get; set; }
}