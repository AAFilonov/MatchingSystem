using System;
using Lombok.NET;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.DataLayer.Feature.CommonQuota;


public partial  class CommonQuotaEntity
{
    public int CommonQuotaId { get; set; }
    public int TutorId { get; set; }
    public short? Qty { get; set; }
    public DateTime CreateDate { get; set; }
    public int QuotaStateId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool IsNotification { get; set; }
    public string? Message { get; set; }
    public int StageId { get; set; }

    public virtual QuotasStateEntity QuotaState { get; set; } = null!;
    public virtual StageEntity Stage { get; set; }
    public virtual TutorEntity Tutor { get; set; }
}