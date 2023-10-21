using System;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Student;

namespace MatchingSystem.DataLayer.Feature.TutorChoise;

public class TutorsChoiceEntity
{
    public int ChoiceId { get; set; }
    public int StudentId { get; set; }
    public int ProjectId { get; set; }
    public short? SortOrderNumber { get; set; }
    public bool IsInQuota { get; set; }
    public bool? IsChangeble { get; set; }
    public int TypeId { get; set; }
    public int? PreferenceId { get; set; }
    public short? IterationNumber { get; set; }
    public int StageId { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool? IsFromPreviousIteration { get; set; }

    public virtual ProjectEntity Project { get; set; } = null!;
    public virtual StageEntity Stage { get; set; } = null!;
    public virtual StudentEntity Student { get; set; } = null!;
    public virtual ChoosingTypeEntity typeEntity { get; set; } = null!;
}