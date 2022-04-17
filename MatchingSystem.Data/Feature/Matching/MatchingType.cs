// ReSharper disable once CheckNamespace
namespace MatchingSystem.Data.Model;

public partial class MatchingType
{
    public MatchingType()
    {
        Matchings = new HashSet<Matching>();
    }

    public int MatchingTypeId { get; set; }
    public string? MatchingTypeName { get; set; }
    public string? MatchingTypeNameRu { get; set; }
    public string? MatchingTypeCode { get; set; }

    public virtual ICollection<Matching> Matchings { get; set; }
}