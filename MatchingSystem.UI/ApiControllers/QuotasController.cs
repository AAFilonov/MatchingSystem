using System;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.DataLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class QuotasController : ControllerBase
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IExecutiveRepository executiveRepository;

        public QuotasController(
            ITutorRepository tutorRepository, 
            IProjectRepository projectRepository,
            IExecutiveRepository executiveRepository
        )
        {
            this.tutorRepository = tutorRepository;
            this.projectRepository = projectRepository;
            this.executiveRepository = executiveRepository;
        }

        [Route("api/[controller]/tutor/initialize")]
        [HttpGet]
        public IActionResult Initialize(int tutorId, int stageTypeCode)
        {
            if (tutorId == default || stageTypeCode == default)
            {
                return BadRequest("Некорректный параметр запроса");
            }
            
            try
            {
                var model = new TutorQuotaData
                {
                    History = tutorRepository.GetQuotaRequestHistoryByTutor(tutorId),
                    CommonQuota = tutorRepository.GetCommonQuotaByTutor(tutorId),
                };
                if (stageTypeCode == 4)
                {
                    model.Projects = projectRepository.GetProjectsByTutor(tutorId);
                }
                
                return new JsonResult(model);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/send_request")]
        [HttpPost]
        public IActionResult CreateRequest([FromBody] ChangeQuotaRequest data)
        {
            try
            {
                tutorRepository.CreateCommonQuotaRequestForSecondStage(data.TutorId, data.NewQuotaQty, data.Message);
                return NoContent();
            }
            catch (SqlException ex)
            {
                return ex.Number switch
                {
                    50006 => Problem("У вас уже есть необработанный запрос на изменение квоты", statusCode: 400),
                    _ => Problem("Произошла неизвестная ошибка", statusCode: 500)
                };
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
        
        [Route("api/[controller]/sendRequestForLastStage")]
        [HttpPost]
        public IActionResult CreateRequestForLastStage([FromBody] ChangeQuotaRequest data)
        {
            try
            {
                    var request = new CreateCommonQuotaParams
                    {
                        Message = data.Message,
                        NewQuota = data.NewQuotaQty,
                        TutorId = data.TutorId
                    };
                    request.FillProjectQuota(data.Deltas);
             
                tutorRepository.CreateCommonQuotaRequestForLastStage(request);
                return Ok();
            }
            catch (SqlException ex)
            {
                return ex.Number switch
                {
                    50006 => Problem("У вас уже есть необработанный запрос на изменение квоты", statusCode: 400),
                    _ => Problem(detail: "Произошла неизвестная ошибка", statusCode: 500)
                };
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/process_request")]
        [HttpPatch]
        public IActionResult AcceptRequest()
        {
            var data = HttpContext.Request.Form;

            try
            {
                executiveRepository.AcceptQuotaRequest(Convert.ToInt32(data["quotaId"]), data["action"] == "accept");
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}