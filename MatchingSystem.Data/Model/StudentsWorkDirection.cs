﻿using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class StudentsWorkDirection
{
    public int StudentDirectionId { get; set; }
    public int StudentId { get; set; }
    public int DirectionId { get; set; }

    public virtual WorkDirection Direction { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
}