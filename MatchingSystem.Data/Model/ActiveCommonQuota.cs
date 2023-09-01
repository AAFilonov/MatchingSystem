using System;

namespace MatchingSystem.DataLayer.Model;

public class ActiveCommonQuota
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
}