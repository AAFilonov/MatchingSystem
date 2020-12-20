using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.UI.ResultModels
{
    public class EditProfile
    {
        public List<WorkDirection> WorkDirections { get; set; }
        public List<Technology> Technologies { get; set; }
        public string Info { get; set; }
        public string Info2 { get; set; }
    }
}
