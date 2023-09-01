using System;

namespace MatchingSystem.DataLayer.Model;

public class Log
{
    public Guid Id { get; set; }
    public string? Request { get; set; }
    public string? Endpoint { get; set; }
    public DateTime? RequestDate { get; set; }
}