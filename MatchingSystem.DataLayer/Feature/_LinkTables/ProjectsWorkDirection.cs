using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Project;

namespace MatchingSystem.DataLayer.Model;

public class ProjectsWorkDirection
{
    public int ProjectDirectionId { get; set; }
    public int ProjectId { get; set; }
    public int DirectionId { get; set; }

    public virtual WorkDirectionEntity directionEntity { get; set; } = null!;
    public virtual ProjectEntity Project { get; set; } = null!;
}