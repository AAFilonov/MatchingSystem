using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Student;

public class StudentRepository : Repository<StudentEntity>,IStudentRepository
{
    public StudentRepository(DbContext context) : base(context)
    {
    }

    public List<StudentView> getStundentsView()
    {
        return _matchingSystemContext.StudentsViews.ToList();
    }
}