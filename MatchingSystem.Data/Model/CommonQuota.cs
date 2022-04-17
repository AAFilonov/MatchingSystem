using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class CommonQuota
{
    public int CommonQuotaId { get; set; }
    public int? TutorId { get; set; }
    public short? Qty { get; set; }
    public DateTime CreateDate { get; set; }
    public int QuotaStateId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool? IsNotification { get; set; }
    public string? Message { get; set; }
    public int? StageId { get; set; }

    public virtual QuotasState QuotaState { get; set; } = null!;
    public virtual Stage? Stage { get; set; }
    public virtual Tutor? Tutor { get; set; }
}