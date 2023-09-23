using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ResultModels
{
    public class EditProfile
    {
        public List<WorkDirection> WorkDirections { get; set; }
        public List<TechnologyDto> Technologies { get; set; }
        public string Info { get; set; }
        public string Info2 { get; set; }
    }
}
