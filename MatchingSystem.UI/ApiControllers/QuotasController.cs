using System;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.Service.Quotas;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class QuotasController : ControllerBase
    {
        private readonly ICommonQuotaService commonQuotaService;

        public QuotasController(ICommonQuotaService commonQuotaService)
        {
            this.commonQuotaService = commonQuotaService;
        }

        [Route("api/[controller]/tutor/initialize")]
        [HttpGet]
        public IActionResult Initialize(int tutorId, int stageTypeCode)
        {
            if (tutorId == default || stageTypeCode == default)
            {
                return BadRequest("Некорректный параметр запроса");
            }
            var model = commonQuotaService.Initialize(tutorId, stageTypeCode);
            return new JsonResult(model);
        }

        [Route("api/[controller]/send_request")]
        [HttpPost]
        public IActionResult CreateRequest([FromBody] ChangeQuotaRequest data)
        {
            commonQuotaService.CreateQuotaRequest(data);
            return NoContent();
        }
        
        [Route("api/[controller]/sendRequestForLastStage")]
        [HttpPost]
        public IActionResult CreateRequestForLastStage([FromBody] ChangeQuotaRequest data)
        {
            commonQuotaService.CreateQuotaRequest(data);
            return Ok();
        }

        [Route("api/[controller]/process_request")]
        [HttpPatch]
        public IActionResult AcceptRequest()
        {
            var data = HttpContext.Request.Form;
            int quotaId = Convert.ToInt32(data["quotaId"]);
            string action = data["action"];
            commonQuotaService.AcceptRequest(quotaId, action);

            return NoContent();
        }
    }
}