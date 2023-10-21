using System.Collections.Generic;
using System.Text.Json.Serialization;
using MatchingSystem.DataLayer.Feature.Allocation;

namespace MatchingSystem.DataLayer.OldDto;

public class AdjustmentRequest
{
    [JsonPropertyName("userId")]
    public int? UserID { get; set; }
    [JsonPropertyName("matchingId")]
    public int? MatchingID { get; set; }
    [JsonPropertyName("adjustment")]
    public List<AllocationDto> Allocations { get; set; }
}
