using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.DataLayer.Dto;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class ExecutiveController : ControllerBase
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IExecutiveRepository executiveRepository;
        private readonly IMatchingRepository matchingRepository;
        private readonly IProjectRepository projectRepository;

        public ExecutiveController(
            ITutorRepository tutorRepository, 
            IExecutiveRepository executiveRepository, 
            IMatchingRepository matchingRepository, 
            IProjectRepository projectRepository
            )
        {
            this.tutorRepository = tutorRepository;
            this.executiveRepository = executiveRepository;
            this.matchingRepository = matchingRepository;
            this.projectRepository = projectRepository;
        }

        [Route("api/[controller]/setNextStage")]
        [HttpPatch]
        public IActionResult SetNextStage([FromQuery] int? matchingId, [FromQuery] int? userId)
        {
            if (matchingId == null || userId == null)
            {
                return BadRequest("Неверный формат запроса");
            }

            var quotaRequest = executiveRepository.GetQuotaRequestsByExecutive(userId.Value, matchingId.Value);
            
            if (quotaRequest.Any())
            {
                return Problem("Вы не можете завершить этап, пока есть активные запросы на изменение квоты.", statusCode: 400);
            }

            try
            {
                matchingRepository.SetNextStage(matchingId.Value);
                return Ok();
            } 
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/setEndDate")]
        [HttpPatch]
        public IActionResult SetEndDate([FromForm] string endDate, [FromForm] int matchingId)
        {
            var isParsed = DateTime.TryParse(endDate, out var time);
            
            if (!isParsed)
            {
                return BadRequest("Получена некорректная дата");
            }

            try
            {
                matchingRepository.SetStageEndDate(time, matchingId);
                return Ok();
            }
            catch (SqlException ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

   
        
        
        
        [Route("api/[controller]/getAllocation")]
        [HttpGet]
        public IActionResult GetAllocationByExecutive([FromQuery] int? userId, [FromQuery] int? matchingId)
        {
            if (!userId.HasValue || !matchingId.HasValue)
            {
                return BadRequest("Некорректный запрос.");
            }

            try
            {
                var model = new AdjustmentData()
                {
                    Allocations = executiveRepository.GetAllocationsByExecutive(userId.Value, matchingId.Value),
                    Tutors = tutorRepository.GetTutorsByMatching(matchingId.Value),
                    Projects = new List<Project>()
                };

                var tutorIds = model.Tutors.Select(x => x.TutorID).ToList();

                foreach (var tutorId in tutorIds)
                {
                    var temp = projectRepository.GetProjectsByTutor(tutorId).ToList();
                    temp.ForEach(x => x.TutorID = tutorId);
                    model.Projects.AddRange(temp);
                }

                return new JsonResult(model);
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/setAllocation")]
        [HttpPatch]
        public IActionResult SetAllocationByExecutive([FromBody] AdjustmentRequest request)
        {
            if (!request.MatchingID.HasValue || !request.UserID.HasValue ||
                request.Allocations.Count == 0 || request.Allocations == null)
            {
                return BadRequest("Некорректный запрос.");
            }

            try
            {
                foreach (var student in request.Allocations)
                {
                    var inParams = new AdjustmentParams()
                    {
                        MatchingId = request.MatchingID.Value,
                        ProjectId = student.ProjectID.Value,
                        StudentId = student.StudentID.Value,
                        UserId = request.UserID.Value
                    };
                    executiveRepository.SetAdjustmentByExecutive(inParams);
                }
            }
            catch (Exception)
            {
                return Problem(
                    "При обработке запроса произошла неизвестная ошибка. Перезагрузите страницу и проверье данные.", 
                    statusCode: 500
                );
            }
            
            return Ok();
        }
    }
}