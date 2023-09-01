using System;

namespace MatchingSystem.DataLayer.Model;

public class CommonQuota1
{
    public int? MatchingId { get; set; }
    public int TutorId { get; set; }
    public bool? IsReadyToStart { get; set; }
    public int CommonQuotaId { get; set; }
    public short? Qty { get; set; }
    public DateTime CreateDate { get; set; }
    public int QuotaStateId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool? IsNotification { get; set; }
    public string? Message { get; set; }
    public int? StageId { get; set; }
    public bool IsCurrent { get; set; }
    public int StageTypeCode { get; set; }
    public string StageTypeName { get; set; } = null!;
    public string? StageTypeNameRu { get; set; }
    public short? IterationNumber { get; set; }
    public int QuotaStateCode { get; set; }
    public string QuotaStateName { get; set; } = null!;
    public string? QuotaStateNameRu { get; set; }
}