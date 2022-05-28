using System.Collections.Generic;
using System.Text.Json.Serialization;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Dto;

public class AdjustmentRequest
{
    [JsonPropertyName("userId")]
    public int? UserID { get; set; }
    [JsonPropertyName("matchingId")]
    public int? MatchingID { get; set; }
    [JsonPropertyName("adjustment")]
    public List<Allocation> Allocations { get; set; }
}
