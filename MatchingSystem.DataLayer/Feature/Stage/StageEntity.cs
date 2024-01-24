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

    public override string ToString()
    {
        return $"{nameof(StageId)}: {StageId}, {nameof(StageTypeId)}: {StageTypeId}, {nameof(StageName)}: {StageName}, {nameof(IterationNumber)}: {IterationNumber}, {nameof(StartDate)}: {StartDate}, {nameof(IsCurrent)}: {IsCurrent}, {nameof(MatchingId)}: {MatchingId}";
    }
    
    protected bool Equals(StageEntity other)
    {
        return StageId == other.StageId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((StageEntity)obj);
    }

    public override int GetHashCode()
    {
        return StageId;
    }
}