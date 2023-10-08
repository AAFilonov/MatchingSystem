using System.Collections.Generic;
using MatchingSystem.DataLayer.Model;

namespace MatchingSystem.DataLayer.Feature.Dictionary;

public class WorkDirectionEntity
{
    public WorkDirectionEntity()
    {
        ProjectsWorkDirections = new HashSet<ProjectsWorkDirection>();
        StudentsWorkDirections = new HashSet<StudentsWorkDirection>();
    }

    public int DirectionId { get; set; }
    public string DirectionNameRu { get; set; } = null!;
    public int DirectionCode { get; set; }

    public virtual ICollection<ProjectsWorkDirection> ProjectsWorkDirections { get; set; }
    public virtual ICollection<StudentsWorkDirection> StudentsWorkDirections { get; set; }
}