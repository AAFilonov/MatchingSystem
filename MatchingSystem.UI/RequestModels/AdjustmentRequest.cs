using System.Collections.Generic;
using System.Text.Json.Serialization;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.RequestModels
{
    public class AdjustmentRequest
    {
        [JsonPropertyName("userId")]
        public int? UserID { get; set; }
        [JsonPropertyName("matchingId")]
        public int? MatchingID { get; set; }
        [JsonPropertyName("adjustment")]
        public List<Allocation> Allocations { get; set; }
    }
}