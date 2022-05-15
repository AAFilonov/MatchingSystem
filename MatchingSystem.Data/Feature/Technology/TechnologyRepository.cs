using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Data.Feature.Technology;

public class TechnologyRepository : Repository<Model.Technology>, ITechnologyRepository
{
    public TechnologyRepository(DbContext context) : base(context)
    {
    }
}