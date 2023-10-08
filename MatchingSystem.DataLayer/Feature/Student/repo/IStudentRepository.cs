using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Feature.Student.repo;

public interface IStudentRepository :IRepository<StudentEntity>
{
    public List<StudentView> getStundentsView();
}