using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Student;

namespace MatchingSystem.UI.ViewModels
{
    public class StudentViewModel
    {
        public StudentDto studentDto { get; set; }
        public IEnumerable<WorkDirectionDto> WorkDirection { get; set; }
        public IEnumerable<TechnologyDto> Technology { get; set; }
    }
}
