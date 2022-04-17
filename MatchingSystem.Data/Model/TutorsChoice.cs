using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class TutorsChoice
{
    public int ChoiceId { get; set; }
    public int StudentId { get; set; }
    public int ProjectId { get; set; }
    public short? SortOrderNumber { get; set; }
    public bool IsInQuota { get; set; }
    public bool? IsChangeble { get; set; }
    public int TypeId { get; set; }
    public int? PreferenceId { get; set; }
    public short? IterationNumber { get; set; }
    public int StageId { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool? IsFromPreviousIteration { get; set; }

    public virtual Project Project { get; set; } = null!;
    public virtual Stage Stage { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
    public virtual ChoosingType Type { get; set; } = null!;
}