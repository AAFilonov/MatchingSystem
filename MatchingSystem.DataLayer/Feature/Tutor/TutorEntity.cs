using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature._LinkTables;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Role;
using Newtonsoft.Json;

namespace MatchingSystem.DataLayer.Feature.Tutor;

public class TutorEntity
{
    public TutorEntity()
    {
        CommonQuota = new HashSet<CommonQuota.CommonQuotaEntity>();
        Projects = new HashSet<ProjectEntity>();
        TutorsGroups = new HashSet<TutorsGroup>();
        UsersRoles = new HashSet<UsersRoleEntity>();
    }

    public int TutorId { get; set; }
    public int? TutorBk { get; set; }
    public bool IsClosed { get; set; }
    public short? CloseIterationNumber { get; set; }
    public bool? IsReadyToStart { get; set; }
    public int MatchingId { get; set; }
    public bool IsDeleted { get; set; }

    [JsonIgnore]
    public virtual ICollection<CommonQuota.CommonQuotaEntity> CommonQuota { get; set; }
    [JsonIgnore]
    public virtual ICollection<ProjectEntity> Projects { get; set; }
    [JsonIgnore]
    public virtual ICollection<TutorsGroup> TutorsGroups { get; set; }
    [JsonIgnore]
    public virtual ICollection<UsersRoleEntity> UsersRoles { get; set; }
}