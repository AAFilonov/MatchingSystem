using System;
using System.Collections.Generic;
using MatchingSystem.Data.Model;

namespace MatchingSystem.DataLayer.Model;

public class Stage
{
    public Stage()
    {
        CommonQuota = new HashSet<CommonQuota>();
        Documents = new HashSet<Document>();
        TutorsChoices = new HashSet<TutorsChoice>();
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

    public virtual Matching Matching { get; set; } = null!;
    public virtual StagesType StageType { get; set; } = null!;
    public virtual ICollection<CommonQuota> CommonQuota { get; set; }
    public virtual ICollection<Document> Documents { get; set; }
    public virtual ICollection<TutorsChoice> TutorsChoices { get; set; }
}