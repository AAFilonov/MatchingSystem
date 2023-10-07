using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Feature.Student;

public interface IStudentRepository :IRepository<StudentEntity>
{
    public List<StudentView> getStundentsView();
}