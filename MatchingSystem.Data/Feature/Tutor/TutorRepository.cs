using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Data.Feature.Tutor;

public class TutorRepository : Repository<Model.Tutor>, ITutorRepository
{
    public TutorRepository(DbContext context) : base(context)
    {
    }
}