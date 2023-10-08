using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ResultModels
{
    public class EditProfile
    {
        public List<WorkDirectionDto> WorkDirections { get; set; }
        public List<TechnologyDto> Technologies { get; set; }
        public string Info { get; set; }
        public string Info2 { get; set; }
    }
}
