using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class StudentsTechnology
{
    public int StudentTechnologyId { get; set; }
    public int StudentId { get; set; }
    public int TechnologyId { get; set; }

    public virtual Student Student { get; set; } = null!;
    public virtual Technology Technology { get; set; } = null!;
}