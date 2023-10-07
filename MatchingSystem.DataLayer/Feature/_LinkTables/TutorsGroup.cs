using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.DataLayer.Model;

public class TutorsGroup
{
    public int TutorGroupId { get; set; }
    public int TutorId { get; set; }
    public int GroupId { get; set; }

    public virtual GroupEntity groupEntity { get; set; } = null!;
    public virtual TutorEntity Tutor { get; set; } = null!;
}