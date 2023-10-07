using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ResultModels
{
    #nullable enable
    public class TutorChoiceData
    {
        public TutorChoiceData() => Choices = new List<TutorChoiceDto>();

        public int? ProjectID { get; set; }
        public string? ProjectName { get; set; }
        public short? Qty { get; set; }
        public List<TutorChoiceDto>? Choices { get; set; }
        public bool? ProjectIsClosed { get; set; }
    }
}
