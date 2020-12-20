#nullable enable
using System;
using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using MatchingSystem.UI.RequestModels;
using MatchingSystem.UI.ResultModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class QuotasController : ControllerBase
    {
        private readonly DataContext context;

        public QuotasController(DataContext ctx)
        {
            context = ctx;
        }

        [Route("api/[controller]/tutor/initialize")]
        [HttpGet]
        public async Task<IActionResult> Initialize([FromQuery] int? tutorId, [FromQuery] int? stageTypeCode)
        {
            if (!tutorId.HasValue || !stageTypeCode.HasValue) return BadRequest(error: "Некорректный параметр запроса");
            try
            {
                var model = new TutorQuotaData
                {
                    History = await context.GetTutorQuotaHistoryAsync(tutorId),
                    CommonQuota = await context.GetCommonQuotaAsync(tutorId),
                };
                if (stageTypeCode == 4) model.Projects = await context.GetTutorProjectsAsync(tutorId);
                
                return new JsonResult(model);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/send_request")]
        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] ChangeQuotaRequest data)
        {
            SqlParameter tid = new SqlParameter("@TutorID", data.TutorId);
            SqlParameter quotaQty = new SqlParameter("@NewQuotaQty", data.NewQuotaQty);
            SqlParameter msg = new SqlParameter("@Message", data.Message ?? string.Empty);

            try
            {
                await context.Database.ExecuteSqlRawAsync("exec napp.create_CommonQuota_Request " +
                                                          "@TutorID, " +
                                                          "@NewQuotaQty, " +
                                                          "@Message", tid, quotaQty, msg);
                return NoContent();
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 50006: return Problem("У вас уже есть необработанный запрос на изменение квоты", statusCode: 400);
                    default: return Problem(detail: "Произошла неизвестная ошибка", statusCode: 500);
                }
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /*TODO: Тут ошибка проектирования. Нужно было сделать этот метот закрытый и инкапсулровать всю логику создания
         запросов на изменение квоты в методе выше. */
        [Route("api/[controller]/sendRequestForLastStage")]
        [HttpPost]
        public async Task<IActionResult> CreateRequestForLastStage([FromBody] ChangeQuotaRequest data)
        {
            try
            {
                var projectQuota = context.FillProjectQuota(data.Deltas);
                // TODO CreateCommonQuotaRequestForLastStageAsync
                await context.CreateCommonQuotaRequestForIterationsAsync(data.TutorId, data.NewQuotaQty, data.Message,
                    projectQuota);
                return Ok();
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 50006: return Problem(detail: "У вас уже есть необработанный запрос на изменение квоты", statusCode: 400);
                    default: return Problem(detail: "Произошла неизвестная ошибка", statusCode: 500);
                }
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
            return Ok();
        }

        [Route("api/[controller]/process_request")]
        [HttpPatch]
        public async Task<IActionResult> AcceptRequest()
        {
            var data = await HttpContext.Request.ReadFormAsync();

            SqlParameter quotaId = new SqlParameter("@QuotaID", Convert.ToInt32(data["quotaId"]));
            SqlParameter reqResult = new SqlParameter("@RequestResult", (data["action"] == "accept") ? true : false);

            try
            {
                await context.Database.ExecuteSqlRawAsync("exec napp.upd_CommonQuota_Request " +
                                                          "@QuotaID, " +
                                                          "@RequestResult",
                    quotaId, reqResult);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}