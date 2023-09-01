using System;
using MatchingSystem.DataLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using MatchingSystem.Service.Quotas;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class QuotasController : ControllerBase
    {
        private readonly IQuotasService quotasService;

        public QuotasController(IQuotasService quotasService)
        {
            this.quotasService = quotasService;
        }

        [Route("api/[controller]/tutor/initialize")]
        [HttpGet]
        public IActionResult Initialize(int tutorId, int stageTypeCode)
        {
            if (tutorId == default || stageTypeCode == default)
            {
                return BadRequest("Некорректный параметр запроса");
            }
            var model = quotasService.Initialize(tutorId, stageTypeCode);
            return new JsonResult(model);
        }

        [Route("api/[controller]/send_request")]
        [HttpPost]
        public IActionResult CreateRequest([FromBody] ChangeQuotaRequest data)
        {
            quotasService.CreateRequest(data);
            return NoContent();
        }
        
        [Route("api/[controller]/sendRequestForLastStage")]
        [HttpPost]
        public IActionResult CreateRequestForLastStage([FromBody] ChangeQuotaRequest data)
        {
            quotasService.CreateRequestForLastStage(data);
            return Ok();
        }

        [Route("api/[controller]/process_request")]
        [HttpPatch]
        public IActionResult AcceptRequest()
        {
            var data = HttpContext.Request.Form;
            int quotaId = Convert.ToInt32(data["quotaId"]);
            string action = data["action"];
            quotasService.AcceptRequest(quotaId, action);

            return NoContent();
        }
    }
}