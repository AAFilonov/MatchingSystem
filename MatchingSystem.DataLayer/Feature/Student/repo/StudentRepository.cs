using System.Collections.Generic;
using System.Linq;

namespace MatchingSystem.DataLayer.Feature.Student.repo;

public class StudentRepository : GenericRepository<StudentEntity>,IStudentRepository
{
    public List<StudentView> getStundentsView()
    {
        return _matchingSystemContext.StudentsViews.ToList();
    }
}