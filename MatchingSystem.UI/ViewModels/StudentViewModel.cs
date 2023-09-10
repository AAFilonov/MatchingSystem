using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ViewModels
{
    public class StudentViewModel
    {
        public StudentDto studentDto { get; set; }
        public IEnumerable<WorkDirection> WorkDirection { get; set; }
        public IEnumerable<Technology> Technology { get; set; }
    }
}
