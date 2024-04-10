using System.Collections.Generic;
using Newtonsoft.Json;

namespace MatchingSystem.DataLayer.Feature.Stage;

public class StagesType
{
    public StagesType()
    {
        Stages = new HashSet<StageEntity>();
    }

    public int StageTypeId { get; set; }
    public int StageTypeCode { get; set; }
    public string StageTypeName { get; set; } = null!;
    public string? StageTypeNameRu { get; set; }

    [JsonIgnore]
    public virtual ICollection<StageEntity> Stages { get; set; }
}