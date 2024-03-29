﻿using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class TutorsGroup
{
    public int TutorGroupId { get; set; }
    public int TutorId { get; set; }
    public int GroupId { get; set; }

    public virtual Group Group { get; set; } = null!;
    public virtual Tutor Tutor { get; set; } = null!;
}