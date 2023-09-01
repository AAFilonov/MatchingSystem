using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Model;

public class QuotasState
{
    public QuotasState()
    {
        CommonQuota = new HashSet<CommonQuota>();
    }

    public int QuotaStateId { get; set; }
    public int QuotaStateCode { get; set; }
    public string QuotaStateName { get; set; } = null!;
    public string? QuotaStateNameRu { get; set; }

    public virtual ICollection<CommonQuota> CommonQuota { get; set; }
}