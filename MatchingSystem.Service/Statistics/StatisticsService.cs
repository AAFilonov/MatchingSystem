using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;

namespace Service.Statistics
{
    internal class StatisticsService
    {
        private readonly IStatisticsRepository statisticsRepository;
        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            this.statisticsRepository = statisticsRepository;
        }

        public IActionResult GetStatisticsMain([FromQuery] int? matchingId, [FromQuery] int? currentStage)
        {
            if (currentStage > 4)
            {
                currentStage = 4;
            }

            var result = statisticsRepository.GetStatisticsMain(matchingId.Value, currentStage.Value);

            foreach (var stat in result)
            {
                stat.StatValue_Str = stat.StatValue_Str?.Replace(",", "<br>");
            }

            return new JsonResult(result);
        }

        public IActionResult GetStatisticsGroups([FromQuery] int? matchingId)
        {
            var result = statisticsRepository.GetStatisticsGroups(matchingId.Value);

            foreach (var stat in result)
            {
                stat.StatValue_Str = stat.StatValue_Str?.Replace(",", "<br>");
            }

            return new JsonResult(result);

        }

        public IActionResult GetStatisticsTutors([FromQuery] int? matchingId, [FromQuery] int? currentStage)
        {
            if (currentStage > 4)
            {
                currentStage = 4;
            }

            var result = statisticsRepository.GetStatisticsTutors(matchingId.Value, currentStage.Value);

            return new JsonResult(result);
        }

        public IActionResult GetStatisticsStudents([FromQuery] int? matchingId, [FromQuery] int? currentStage)
        {
            if (currentStage > 4)
            {
                currentStage = 4;
            }

            var result = statisticsRepository.GetStatisticsStudents(matchingId.Value, currentStage.Value);

            return new JsonResult(result);
        }

        public IActionResult GetStatisticsTutorsProjectAllocated([FromQuery] int? matchingId, [FromQuery] int? tutorId)
        {
            var result = statisticsRepository.GetStatisticsTutorProjectAllocated(matchingId.Value, tutorId.Value);
            return new JsonResult(result);
        }

        public IActionResult GetStatisticsStudentsProjects([FromQuery] int? matchingId, [FromQuery] int? studentId)
        {
            var model = statisticsRepository.GetStatisticsStudentsProjects(matchingId.Value, studentId.Value);

            return new JsonResult(model.OrderBy(project => project.OrderNumber));
        }

        public IActionResult GetStatisticsTutorsProjects([FromQuery] int? matchingId, [FromQuery] int? tutorId)
        {
            var result = statisticsRepository.GetStatisticsTutorsProjects(matchingId.Value, tutorId.Value);
            return new JsonResult(result);
        }
    }
}
