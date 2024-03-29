﻿using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class ProjectsGroup
{
    public int ProjectGroupId { get; set; }
    public int ProjectId { get; set; }
    public int GroupId { get; set; }

    public virtual Group Group { get; set; } = null!;
    public virtual Project Project { get; set; } = null!;
}