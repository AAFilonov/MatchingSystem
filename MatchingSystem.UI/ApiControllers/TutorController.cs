using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Service.Tutor;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ITutorService tutorService;

        public TutorController(ITutorService tutorService)
        {
            this.tutorService = tutorService;
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
            return Ok();
        }

        [Route("api/[controller]/saveChoice")]
        [HttpPatch]
        public IActionResult SaveChoice([FromBody] List<TutorChoice_1> data, [FromQuery] int tutorId)
        {
            tutorService.SaveChoice(data, tutorId);
            return Ok();
        }
    }
}