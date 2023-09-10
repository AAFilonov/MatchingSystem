using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Feature.Student;

public interface IStudentRepository :IRepository<Student>
{
    public List<StudentView> getStundentsView();
}