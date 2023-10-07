using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Student;

namespace MatchingSystem.DataLayer.Model;

public class StudentsWorkDirection
{
    public int StudentDirectionId { get; set; }
    public int StudentId { get; set; }
    public int DirectionId { get; set; }

    public virtual WorkDirectionEntity directionEntity { get; set; } = null!;
    public virtual StudentEntity Student { get; set; } = null!;
}