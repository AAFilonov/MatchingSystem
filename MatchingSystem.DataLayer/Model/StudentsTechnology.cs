using MatchingSystem.DataLayer.Feature.Student;

namespace MatchingSystem.DataLayer.Model;

public class StudentsTechnology
{
    public int StudentTechnologyId { get; set; }
    public int StudentId { get; set; }
    public int TechnologyId { get; set; }

    public virtual StudentEntity Student { get; set; } = null!;
    public virtual Technology Technology { get; set; } = null!;
}