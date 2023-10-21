using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.TutorChoise;

namespace MatchingSystem.DataLayer.OldDto;

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
