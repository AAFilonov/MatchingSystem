using System;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Student;

namespace MatchingSystem.DataLayer.Feature.StudentPreference;

public class StudentsPreferenceEntity
{
    public int PreferenceId { get; set; }
    public int StudentId { get; set; }
    public int ProjectId { get; set; }
    public short OrderNumber { get; set; }
    public bool? IsAvailable { get; set; }
    public int? TypeId { get; set; }
    public bool? IsInUse { get; set; }
    public bool? IsUsed { get; set; }
    public DateTime? CreateDate { get; set; }

    public virtual ProjectEntity Project { get; set; } = null!;
    public virtual StudentEntity Student { get; set; } = null!;
    public virtual ChoosingTypeEntity? Type { get; set; }
}