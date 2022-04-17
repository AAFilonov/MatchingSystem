using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class StudentsPreference
{
    public int PreferenceId { get; set; }
    public int StudentId { get; set; }
    public int ProjectId { get; set; }
    public short OrderNumber { get; set; }
    public bool? IsAvailable { get; set; }
    public int? TypeId { get; set; }
    public bool? IsInUse { get; set; }
    public bool? IsUsed { get; set; }
    public DateTime? CreateDate { get; set; }

    public virtual Project Project { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
    public virtual ChoosingType? Type { get; set; }
}