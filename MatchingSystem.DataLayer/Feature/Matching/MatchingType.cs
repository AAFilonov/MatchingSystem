// ReSharper disable once CheckNamespace

using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Feature.Matching;

public class MatchingType
{
    public MatchingType()
    {
        Matchings = new HashSet<MatchingEntity>();
    }

    public int MatchingTypeId { get; set; }
    public string? MatchingTypeName { get; set; }
    public string? MatchingTypeNameRu { get; set; }
    public string? MatchingTypeCode { get; set; }

    public virtual ICollection<MatchingEntity> Matchings { get; set; }
}