using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature._LinkTables;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Student;

namespace MatchingSystem.DataLayer.Feature.Group;

public class GroupEntity
{
    public GroupEntity()
    {
        ProjectsGroups = new HashSet<ProjectsGroup>();
        Students = new HashSet<StudentEntity>();
        TutorsGroups = new HashSet<TutorsGroup>();
    }

    public int GroupId { get; set; }
    public int? GroupBk { get; set; }
    public string GroupName { get; set; } = null!;
    public int? MatchingId { get; set; }

    public virtual MatchingEntity? Matching { get; set; }
    public virtual ICollection<ProjectsGroup> ProjectsGroups { get; set; }
    public virtual ICollection<StudentEntity> Students { get; set; }
    public virtual ICollection<TutorsGroup> TutorsGroups { get; set; }
}