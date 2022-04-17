using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class AvailableStudentsPreference
{
    public int PreferenceId { get; set; }
    public int StudentId { get; set; }
    public int ProjectId { get; set; }
    public short OrderNumber { get; set; }
    public bool? IsInUse { get; set; }
    public int? MatchingId { get; set; }
}