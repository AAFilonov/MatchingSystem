using System;
using System.Text.Json.Serialization;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Student;

namespace MatchingSystem.DataLayer.Feature.TutorChoice;

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

    [JsonIgnore]
    public virtual ProjectEntity Project { get; set; } = null!;
    [JsonIgnore]
    public virtual StageEntity Stage { get; set; } = null!;
    [JsonIgnore]
    public virtual StudentEntity Student { get; set; } = null!;
    [JsonIgnore]
    public virtual ChoosingTypeEntity typeEntity { get; set; } = null!;
}