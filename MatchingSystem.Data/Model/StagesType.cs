﻿using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class StagesType
{
    public StagesType()
    {
        Stages = new HashSet<Stage>();
    }

    public int StageTypeId { get; set; }
    public int StageTypeCode { get; set; }
    public string StageTypeName { get; set; } = null!;
    public string? StageTypeNameRu { get; set; }

    public virtual ICollection<Stage> Stages { get; set; }
}