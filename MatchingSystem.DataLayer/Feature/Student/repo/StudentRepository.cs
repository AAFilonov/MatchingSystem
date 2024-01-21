using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Student.repo;

public class StudentRepository : GenericRepository<StudentEntity>,IStudentRepository
{
    public List<StudentView> getStundentsView()
    {
        return _matchingSystemContext.StudentsViews.ToList();
    }
}