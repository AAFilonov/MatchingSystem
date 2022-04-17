using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class ProjectsTechnology
{
    public int ProjectTechnologyId { get; set; }
    public int ProjectId { get; set; }
    public int TechnologyId { get; set; }

    public virtual Project Project { get; set; } = null!;
    public virtual Technology Technology { get; set; } = null!;
}