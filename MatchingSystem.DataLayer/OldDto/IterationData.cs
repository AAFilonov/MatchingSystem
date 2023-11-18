using System.Collections.Generic;

namespace MatchingSystem.DataLayer.OldDto;
public class IterationData
{
    public IterationData()
    {
        ChoiceDatas = new List<TutorChoiceData>();
    }

    public List<TutorChoiceData> ChoiceDatas { get; set; }
    public int? CommonQuota { get; set; }
}
