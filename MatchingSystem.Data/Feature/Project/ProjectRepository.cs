using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Data.Feature.Project;

public class ProjectRepository : Repository<Model.Project>, IProjectRepository
{
    public ProjectRepository(DbContext context) : base(context)
    {
    }
}