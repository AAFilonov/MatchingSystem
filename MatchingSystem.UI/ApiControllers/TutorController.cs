using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.Service.Follow;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.Service.Tutor;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ITutorService tutorService;
        private readonly IStageTransitionService stageTransitionService;
        private readonly ITutorRepository tutorRepository;
        
        public TutorController(ITutorService tutorService, IStageTransitionService stageTransitionService, ITutorRepository tutorRepository)
        {
            this.tutorService = tutorService;
            this.stageTransitionService = stageTransitionService;
            this.tutorRepository = tutorRepository;
        } 

        [Route("api/[controller]/getChoice")]
        public IActionResult GetChoice(int tutorId)
        {
            var viewModel = tutorService.GetChoice(tutorId);
            return new JsonResult(viewModel);
        }
        
        [Route("api/[controller]/set_ready")]
        [HttpPatch]
        public async Task<IActionResult> SetReady([FromQuery] int tutorId)
        {
            tutorService.SetReady(tutorId);
            var sessionData = HttpContext.Session.Get<SessionData>("Data"); 
            var currentMatchingId = sessionData .SelectedMatching;
            var need = stageTransitionService.isNeedToTransit(currentMatchingId);
            if (need)
                stageTransitionService.TransitionIfExistNeed(currentMatchingId);
            return Ok();
        }

        [Route("api/[controller]/saveChoice")]
        [HttpPatch]
        public IActionResult SaveChoice([FromBody] List<TutorChoice_1> data, [FromQuery] int tutorId)
        {
            tutorService.SaveChoice(data, tutorId);
            var sessionData = HttpContext.Session.Get<SessionData>("Data"); 
            var currentMatchingId = sessionData .SelectedMatching;
            //check transition between Iterations
            var need = stageTransitionService.isNeedToTransit(currentMatchingId);
            if (need)
                stageTransitionService.TransitionIfExistNeed(currentMatchingId);
            //check transition from 'manual adjustment' to 'final'
            need = stageTransitionService.isNeedToTransit(currentMatchingId);
            if (need)
                stageTransitionService.TransitionIfExistNeed(currentMatchingId);
            return Ok();
        }
    }
}