using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.CommonQuota;

public class CommonQuotaRepository :Repository<CommonQuota>, ICommonQuotaRepository
{
    public CommonQuotaRepository(DbContext context) : base(context)
    {
    }
}