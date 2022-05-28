using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Dto;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsRepository statisticsRepository;
        public StatisticsController(IStatisticsRepository statisticsRepository)
        {
            this.statisticsRepository = statisticsRepository;
        }

        [Route("api/[controller]/getStatisticsMain")]
        [HttpGet]
        public IActionResult GetStatisticsMain([FromQuery] int? matchingId, [FromQuery] int? currentStage)
        {
            if (!matchingId.HasValue || !currentStage.HasValue)
            {
                return BadRequest("Некорректный запрос.");
            }
            
            try
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
            catch (SqlException ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
            
        }
        
        [Route("api/[controller]/getStatisticsGroups")]
        [HttpGet]
        public IActionResult GetStatisticsGroups([FromQuery] int? matchingId)
        {
            if (!matchingId.HasValue)
            {
                return BadRequest("Некорректный запрос.");
            }
            
            try
            {
              
                var result = statisticsRepository.GetStatisticsGroups(matchingId.Value);
                
                foreach (var stat in result)
                {
                    stat.StatValue_Str = stat.StatValue_Str?.Replace(",", "<br>");
                }
                
                return new JsonResult(result);
            }
            catch (SqlException ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
            
        }

        [Route("api/[controller]/getStatisticsTutors")]
        [HttpGet]
        public IActionResult GetStatisticsTutors([FromQuery] int? matchingId, [FromQuery] int? currentStage)
        {
            if (!matchingId.HasValue || !currentStage.HasValue)
            {
                return BadRequest("Некорректный запрос.");
            }

            try
            {
                if (currentStage > 4)
                {
                    currentStage = 4;
                }
                
                var result = statisticsRepository.GetStatisticsTutors(matchingId.Value, currentStage.Value);

                return new JsonResult(result);
            }
            catch (SqlException ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }

        }

        [Route("api/[controller]/getStatisticsStudents")]
        [HttpGet]
        public IActionResult GetStatisticsStudents([FromQuery] int? matchingId, [FromQuery] int? currentStage)
        {
            if (!matchingId.HasValue || !currentStage.HasValue)
            {
                return BadRequest("Некорректный запрос.");
            }

            try
            {
                if (currentStage > 4)
                {
                    currentStage = 4;
                }

                var result = statisticsRepository.GetStatisticsStudents(matchingId.Value, currentStage.Value);

                return new JsonResult(result);
            }
            catch (SqlException ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }

        }


        [Route("api/[controller]/getStatisticsTutorProjectAllocated")]
        [HttpGet]
        public IActionResult GetStatisticsTutorsProjectAllocated([FromQuery] int? matchingId, [FromQuery] int? tutorId)
        {
            if (!matchingId.HasValue || !tutorId.HasValue)
            {
                return BadRequest("Некорректный запрос.");
            }
            try
            {
                var result = statisticsRepository.GetStatisticsTutorProjectAllocated(matchingId.Value, tutorId.Value);
                return new JsonResult(result);
            }
            catch (SqlException ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }

        }
        
        
        [Route("api/[controller]/getStatisticsStudentsProjects")]
        [HttpGet]
        public IActionResult GetStatisticsStudentsProjects([FromQuery] int? matchingId, [FromQuery] int? studentId)
        {
            if (!matchingId.HasValue || !studentId.HasValue)
            {
                return BadRequest("Некорректный запрос.");
            }
            try
            {
                var model = statisticsRepository.GetStatisticsStudentsProjects(matchingId.Value, studentId.Value);
              
                return new JsonResult(  model.OrderBy(project => project.OrderNumber));
            }
            catch (SqlException ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }

        }

 
        [Route("api/[controller]/getStatisticsTutorProjects")]
        [HttpGet]
        public IActionResult GetStatisticsTutorsProjects([FromQuery] int? matchingId, [FromQuery] int? tutorId)
        {
            if (!matchingId.HasValue || !tutorId.HasValue)
            {
                return BadRequest("Некорректный запрос.");
            }
            try
            {
                var result = statisticsRepository.GetStatisticsTutorsProjects(matchingId.Value, tutorId.Value);
                return new JsonResult(result);
            }
            catch (SqlException ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }

        }


    }
}