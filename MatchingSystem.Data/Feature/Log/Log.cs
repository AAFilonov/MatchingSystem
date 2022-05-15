using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class Log
{
    public Guid Id { get; set; }
    public string? Request { get; set; }
    public string? Endpoint { get; set; }
    public DateTime? RequestDate { get; set; }
}