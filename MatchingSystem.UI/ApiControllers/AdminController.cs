/* Контроллер для управление распределениями */

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using Microsoft.Data.SqlClient;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.UI.RequestModels;
using MatchingSystem.UI.ResultModels;
using MatchingSystem.UI.Services;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class ExecutiveController : ControllerBase
    {
        private readonly DataContext context;

        public ExecutiveController(DataContext ctx) => context = ctx;

        [Route("api/[controller]/setNextStage")]
        [HttpPatch]
        public async Task<IActionResult> SetNextStage([FromQuery] int? matchingId, [FromQuery] int? userId)
        {
            if (matchingId == null || userId == null) return Problem(detail: "Неверный формат запроса", statusCode: 400);
            var quotaRequest = await context.GetQuotaRequestAsync(userId, matchingId);
            if (quotaRequest.Count > 0) return Problem(detail: "Вы не можете завершить этап, пока есть активные запросы на изменение квоты.", statusCode: 400);

            try
            {
                await context.SetNextStage(matchingId);
                return Ok();
            } catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/setEndDate")]
        [HttpPatch]
        public async Task<IActionResult> SetEndDate([FromForm] string endDate, [FromForm] int matchingId)
        {
            DateTime? time = DateTime.Parse(endDate);

            try
            {
                await context.SetEndDate(time, matchingId);
                return Ok();
            }
            catch (SqlException ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/getStatisticsMain")]
        [HttpGet]
        public async Task<IActionResult> GetStatisticsMain([FromQuery] int? matchingId, [FromQuery] int? currentStage)
        {
            if (!matchingId.HasValue || !currentStage.HasValue) return Problem(detail: "Некорректный запрос.", statusCode: 400);
            try
            {
                if (currentStage > 4) currentStage = 4;
                var result = context.GetMainStatisticsAsync(matchingId, currentStage);
                foreach (var stat in result)
                {
                    stat.StatValue_Str = stat.StatValue_Str?.Replace(",", "<br>");
                }
                return new JsonResult(result);
            }
            catch (SqlException ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
            
        }

        [Route("api/[controller]/getAllocation")]
        [HttpGet]
        public async Task<IActionResult> GetAllocationByExecutive([FromQuery] int? userId, [FromQuery] int? matchingId)
        {
            if (!userId.HasValue || !matchingId.HasValue)
                return Problem(detail: "Некорректный запрос.", statusCode: 400);

            try
            {
                var model = new AdjustmentData()
                {
                    Allocations = await context.GetAllocationsByExecutiveAsync(userId, matchingId),
                    Tutors = await context.GetTutorsByMatchingAsync(matchingId),
                    Projects = new List<Project>()
                };

                var tutorIds = model.Tutors.Select(x => x.TutorID).ToList();

                foreach (var tutorId in tutorIds)
                {
                    var temp = await context.GetTutorProjectsAsync(tutorId);
                    temp.ForEach(x => x.TutorID = tutorId);
                    model.Projects.AddRange(temp);
                }

                return new JsonResult(model);
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/setAllocation")]
        [HttpPatch]
        public async Task<IActionResult> SetAllocationByExecutive([FromBody] AdjustmentRequest request)
        {
            if (!request.MatchingID.HasValue || !request.UserID.HasValue || 
                request.Allocations.Count == 0 || request.Allocations == null)
                return Problem(detail: "Некорректный запрос.", statusCode: 400);

            try
            {
                foreach (var student in request.Allocations)
                {
                    await context.SetAdjustmentAsync(request.UserID, request.MatchingID, student.StudentID, student.ProjectID);
                }
            }
            catch (Exception)
            {
                return Problem(
                    detail: "При обработке запроса произошла неизвестная ошибка. Перезагрузите страницу и проверье данные.", 
                    statusCode: 500
                );
            }
            
            return Ok();
        }
    }
}