using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Project;

namespace MatchingSystem.DataLayer.Model;

public class ProjectsGroup
{
    public int ProjectGroupId { get; set; }
    public int ProjectId { get; set; }
    public int GroupId { get; set; }

    public virtual GroupEntity groupEntity { get; set; } = null!;
    public virtual ProjectEntity Project { get; set; } = null!;
}