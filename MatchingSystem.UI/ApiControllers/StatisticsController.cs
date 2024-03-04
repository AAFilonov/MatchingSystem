using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MatchingSystem.Service.Statistics;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        [Route("api/[controller]/getStatisticsMain")]
        [HttpGet]
        public IActionResult GetStatisticsMain([FromQuery] int? matchingId, [FromQuery] int? currentStage)
        {
            var result = statisticsService.GetStatisticsMain((int) matchingId);
            return new JsonResult(result);
        }
        
        [Route("api/[controller]/getStatisticsGroups")]
        [HttpGet]
        public IActionResult GetStatisticsGroups([FromQuery] int? matchingId)
        {
            var result = statisticsService.GetStatisticsGroups(matchingId);    
            return new JsonResult(result);            
        }

        [Route("api/[controller]/getStatisticsTutors")]
        [HttpGet]
        public IActionResult GetStatisticsTutors([FromQuery] int? matchingId, [FromQuery] int? currentStage)
        {
            var result = statisticsService.GetStatisticsTutors(matchingId, currentStage);

            return new JsonResult(result);

        }

        [Route("api/[controller]/getStatisticsStudents")]
        [HttpGet]
        public IActionResult GetStatisticsStudents([FromQuery] int matchingId, [FromQuery] int currentStage)
        {
            var result = statisticsService.GetStatisticsStudents(matchingId);
            return new JsonResult(result);
        }

        [Route("api/[controller]/getStatisticsTutorProjectAllocated")]
        [HttpGet]
        public IActionResult GetStatisticsTutorsProjectAllocated([FromQuery] int? matchingId, [FromQuery] int? tutorId)
        {
            var result = statisticsService.GetStatisticsTutorsProjectAllocated(matchingId, tutorId);

            return new JsonResult(result);
        }

        [Route("api/[controller]/getStatisticsStudentsProjects")]
        [HttpGet]
        public IActionResult GetStatisticsStudentsProjects([FromQuery] int matchingId, [FromQuery] int studentId)
        {
            var model = statisticsService.GetStatisticsStudentsProjects(matchingId, studentId);
            return new JsonResult(model);
        }

        [Route("api/[controller]/getStatisticsTutorProjects")]
        [HttpGet]
        public IActionResult GetStatisticsTutorsProjects([FromQuery] int? matchingId, [FromQuery] int? tutorId)
        {
            var result = statisticsService.GetStatisticsTutorsProjects(matchingId, tutorId);
            return new JsonResult(result);
        }
    }
}