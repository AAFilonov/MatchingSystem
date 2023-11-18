using System;
using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.TutorChoise;

namespace MatchingSystem.DataLayer.Feature.Stage;

public class StageEntity
{
    public StageEntity()
    {
        CommonQuota = new HashSet<CommonQuota.CommonQuotaEntity>();
        TutorsChoices = new HashSet<TutorsChoiceEntity>();
    }

    public int StageId { get; set; }
    public int StageTypeId { get; set; }
    public string? StageName { get; set; }
    public short? IterationNumber { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndPlanDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsCurrent { get; set; }
    public int MatchingId { get; set; }

    public virtual Matching.MatchingEntity Matching { get; set; } = null!;
    public virtual StagesType StageType { get; set; } = null!;
    public virtual ICollection<CommonQuota.CommonQuotaEntity> CommonQuota { get; set; }
    public virtual ICollection<TutorsChoiceEntity> TutorsChoices { get; set; }
}