using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.UI.ResultModels
{
    #nullable enable
    public class TutorChoiceData
    {
        public TutorChoiceData() => Choices = new List<TutorChoice>();

        public int? ProjectID { get; set; }
        public string? ProjectName { get; set; }
        public short? Qty { get; set; }
        public List<TutorChoice>? Choices { get; set; }
        public bool? ProjectIsClosed { get; set; }
    }
}
