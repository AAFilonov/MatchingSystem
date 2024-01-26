using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.TutorChoise;
using MatchingSystem.DataLayer.OldEntities;
using TutorChoiceDto = MatchingSystem.DataLayer.Feature.TutorChoise.TutorChoiceDto;

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
