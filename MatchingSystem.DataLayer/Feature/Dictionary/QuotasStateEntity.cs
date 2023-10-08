using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.CommonQuota;

namespace MatchingSystem.DataLayer.Feature.Dictionary;

public class QuotasStateEntity
{
    public QuotasStateEntity()
    {
        CommonQuota = new HashSet<CommonQuotaEntity>();
    }

    public int QuotaStateId { get; set; }
    public int QuotaStateCode { get; set; }
    public string QuotaStateName { get; set; } = null!;
    public string? QuotaStateNameRu { get; set; }

    public virtual ICollection<CommonQuotaEntity> CommonQuota { get; set; }
}