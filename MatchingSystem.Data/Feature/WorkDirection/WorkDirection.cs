using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class WorkDirection
{
    public WorkDirection()
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