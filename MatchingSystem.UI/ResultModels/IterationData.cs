using System.Collections.Generic;

namespace MatchingSystem.UI.ResultModels
{
    public class IterationData
    {
        public IterationData()
        {
            ChoiceDatas = new List<TutorChoiceData>();
        }

        public List<TutorChoiceData> ChoiceDatas { get; set; }
        public int? CommonQuota { get; set; }
    }
}