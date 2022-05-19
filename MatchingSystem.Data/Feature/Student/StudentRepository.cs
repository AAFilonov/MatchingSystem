using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Data.Feature.Student;

public class StudentRepository : Repository<Model.Stage>, IStudentRepository
{
    public StudentRepository(DbContext context) : base(context)
    {
    }
}