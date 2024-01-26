using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Lombok.NET;
using MatchingSystem.DataLayer.Feature.Allocation.repo;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.Service.Executive;
using MatchingSystem.Service.StageTransition;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    [AllArgsConstructor]
    public partial class ExecutiveController : ControllerBase
    {
        private readonly IExecutiveService executiveService;
        private readonly IStageTransitionService stageTransitionService;
        private readonly IAllocationRepository allocationRepository;

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
        public IActionResult GetAllocationByExecutive([FromQuery] int userId, [FromQuery] int matchingId)
        {
            var model = executiveService.GetAllocationByExecutive(userId, matchingId);
            return new JsonResult(model);
        }

        [Route("api/[controller]/setAllocation")]
        [HttpPatch]
        public IActionResult SetAllocationByExecutive( AdjustmentRequest request)
        {
            if (!request.MatchingID.HasValue || !request.UserID.HasValue ||
                request.Allocations.Count == 0 || request.Allocations == null)
            {
                return BadRequest("Некорректный запрос.");
            }
            allocationRepository.SetAdjustmentByExecutive(request);
            var sessionData = HttpContext.Session.Get<SessionData>("Data"); 
            var currentMatchingId = sessionData .SelectedMatching;
            //check transition from 'manual adjustment' to 'final'
            var need = stageTransitionService.isNeedToTransit(currentMatchingId);
            if (need)
                stageTransitionService.MoveToNextStage(currentMatchingId);
            return Ok();
        }
    }
}