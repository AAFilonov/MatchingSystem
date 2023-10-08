using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Dto;
public class IterationData
{
    public IterationData()
    {
        ChoiceDatas = new List<TutorChoiceData>();
    }

    public List<TutorChoiceData> ChoiceDatas { get; set; }
    public int? CommonQuota { get; set; }
}
