using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto;

public class TutorQuotaData
{
    public IEnumerable<QuotaHistoryTutor> History { get; set; }
    public IEnumerable<ProjectDto> Projects { get; set; }
    public int? CommonQuota { get; set; }
}
