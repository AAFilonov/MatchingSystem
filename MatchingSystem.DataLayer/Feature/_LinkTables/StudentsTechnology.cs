using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Student;

namespace MatchingSystem.DataLayer.Feature._LinkTables;

public class StudentsTechnology
{
    public int StudentTechnologyId { get; set; }
    public int StudentId { get; set; }
    public int TechnologyId { get; set; }

    public virtual StudentEntity Student { get; set; } = null!;
    public virtual TechnologyEntity Technology { get; set; } = null!;
}