using System;

namespace MatchingSystem.DataLayer.Utils;

public class VersionInfo
{
    public long Version { get; set; }
    public DateTime? AppliedOn { get; set; }
    public string? Description { get; set; }
}