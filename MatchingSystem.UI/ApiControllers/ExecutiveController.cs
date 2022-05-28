using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.DataLayer.Dto;
using Service.Executive;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class ExecutiveController : ControllerBase
    {
        private readonly IExecutiveService executiveService;

        public ExecutiveController(IExecutiveService executiveService)
        {
            this.executiveService = executiveService;
        }

        [Route("api/[controller]/setNextStage")]
        [HttpPatch]
        public IActionResult SetNextStage(int? matchingId,int? userId)
        {
            if (matchingId == null || userId == null)
            {
                return BadRequest("Неверный формат запроса");
            }
            executiveService.SetNextStage(matchingId, userId);
            return Ok();
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
            executiveService.SetEndDate(time, matchingId);
            return Ok();
        }
        
        [Route("api/[controller]/getAllocation")]
        [HttpGet]
        public IActionResult GetAllocationByExecutive([FromQuery] int? userId, [FromQuery] int? matchingId)
        {
            if (!userId.HasValue || !matchingId.HasValue)
            {
                return BadRequest("Некорректный запрос.");
            }
            var model = executiveService.GetAllocationByExecutive(userId, matchingId);
            return new JsonResult(model);
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
            executiveService.SetAllocationByExecutive(request);
            return Ok();
        }
    }
}