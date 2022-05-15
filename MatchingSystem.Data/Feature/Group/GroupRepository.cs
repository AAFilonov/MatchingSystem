using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Data.Feature.Group;

public class GroupRepository : Repository<Model.Group>, IGroupRepository
{
    public GroupRepository(DbContext context) : base(context)
    {
    }
}