using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.Service.Executive;
using MatchingSystem.Service.Follow;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class ExecutiveController : ControllerBase
    {
        private readonly IExecutiveService executiveService;
        private readonly IStageTransitionService stageTransitionService;

        public ExecutiveController(IExecutiveService executiveService,IStageTransitionService stageTransitionService)
        {
            this.executiveService = executiveService;
            this.stageTransitionService = stageTransitionService;
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
            var sessionData = HttpContext.Session.Get<SessionData>("Data"); 
            var currentMatchingId = sessionData .SelectedMatching;
            //check transition from 'manual adjustment' to 'final'
            var need = stageTransitionService.isNeedToTransit(currentMatchingId);
            if (need)
                stageTransitionService.TransitionIfExistNeed(currentMatchingId);
            return Ok();
        }
    }
}