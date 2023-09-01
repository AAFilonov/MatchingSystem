using System.Text.Json.Serialization;

namespace MatchingSystem.DataLayer.OldEntities
{
    public class TutorChoice_1
    {
        [JsonPropertyName("choiceId")]
        public int ChoiceID { get; set; }
        [JsonPropertyName("sortOrderNumber")]
        public short SortOrderNumber { get; set; }
        [JsonPropertyName("isInQuota")]
        public bool IsInQuota { get; set; }
    }
}