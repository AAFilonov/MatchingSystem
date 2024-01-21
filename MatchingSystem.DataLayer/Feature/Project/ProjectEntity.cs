using System;
using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature._LinkTables;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.TutorChoise;

namespace MatchingSystem.DataLayer.Feature.Project;

public class ProjectEntity
{
    public ProjectEntity()
    {
        ProjectsGroups = new HashSet<ProjectsGroupEntity>();
        ProjectsTechnologies = new HashSet<ProjectsTechnology>();
        ProjectsWorkDirections = new HashSet<ProjectsWorkDirection>();
        StudentsPreferences = new HashSet<StudentsPreference>();
        TutorsChoices = new HashSet<TutorsChoiceEntity>();
    }

    public int ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public string? Info { get; set; }
    public int TutorId { get; set; }
    public bool? IsClosed { get; set; }
    public bool? IsDefault { get; set; }
    public int? MatchingId { get; set; }
    public short? ProjectQuotaQty { get; set; }
    public int? CloseStage { get; set; }
    public DateTime? CloseDate { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public short? ProjectQuotaDelta { get; set; }

    public virtual TutorEntity Tutor { get; set; } = null!;
    public virtual ICollection<ProjectsGroupEntity> ProjectsGroups { get; set; }
    public virtual ICollection<ProjectsTechnology> ProjectsTechnologies { get; set; }
    public virtual ICollection<ProjectsWorkDirection> ProjectsWorkDirections { get; set; }
    public virtual ICollection<StudentsPreference> StudentsPreferences { get; set; }
    public virtual ICollection<TutorsChoiceEntity> TutorsChoices { get; set; }
}