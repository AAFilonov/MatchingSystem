using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class Document
{
    public int DocumentId { get; set; }
    public int StageId { get; set; }
    public string Path { get; set; } = null!;
    public string DocumentName { get; set; } = null!;

    public virtual Stage Stage { get; set; } = null!;
}