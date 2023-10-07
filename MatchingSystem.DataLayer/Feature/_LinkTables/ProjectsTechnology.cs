using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Project;

namespace MatchingSystem.DataLayer.Model;

public class ProjectsTechnology
{
    public int ProjectTechnologyId { get; set; }
    public int ProjectId { get; set; }
    public int TechnologyId { get; set; }

    public virtual ProjectEntity Project { get; set; } = null!;
    public virtual TechnologyEntity Technology { get; set; } = null!;
}