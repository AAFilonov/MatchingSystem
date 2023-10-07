using System.Collections.Generic;
using MatchingSystem.DataLayer.Model;

namespace MatchingSystem.DataLayer.Feature.Dictionary;

public class TechnologyEntity
{
    public TechnologyEntity()
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