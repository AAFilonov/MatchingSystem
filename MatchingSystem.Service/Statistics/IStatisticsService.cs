using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;

namespace Service.Statistics
{
    internal interface IStatisticsService
    {
        public IActionResult GetStatisticsMain([FromQuery] int? matchingId, [FromQuery] int? currentStage);
        public IActionResult GetStatisticsGroups([FromQuery] int? matchingId);
        
        public IActionResult GetStatisticsTutors([FromQuery] int? matchingId, [FromQuery] int? currentStage);

        public IActionResult GetStatisticsStudents([FromQuery] int? matchingId, [FromQuery] int? currentStage);

        public IActionResult GetStatisticsTutorsProjectAllocated([FromQuery] int? matchingId, [FromQuery] int? tutorId);

        public IActionResult GetStatisticsStudentsProjects([FromQuery] int? matchingId, [FromQuery] int? studentId);

        public IActionResult GetStatisticsTutorsProjects([FromQuery] int? matchingId, [FromQuery] int? tutorId);
    }
}
