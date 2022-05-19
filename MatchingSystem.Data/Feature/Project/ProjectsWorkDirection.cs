using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class ProjectsWorkDirection
{
    public int ProjectDirectionId { get; set; }
    public int ProjectId { get; set; }
    public int DirectionId { get; set; }

    public virtual WorkDirection Direction { get; set; } = null!;
    public virtual Project Project { get; set; } = null!;
}