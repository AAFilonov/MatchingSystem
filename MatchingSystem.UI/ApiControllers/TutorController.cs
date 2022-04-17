using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.UI.ResultModels;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ITutorRepository tutorRepository;

        public TutorController(ITutorRepository tutorRepository)
        {
            this.tutorRepository = tutorRepository;
        } 

        [Route("api/[controller]/getChoice")]
        public IActionResult GetChoice(int tutorId)
        {
            try
            {
                var result = tutorRepository.GetChoiceByTutor(tutorId).ToList();

                var projects = result.Select(x => x.ProjectID).Distinct();

                var viewModel = new IterationData
                {
                    CommonQuota = tutorRepository.GetCommonQuotaByTutor(tutorId)
                };


                foreach (var proj in projects)
                {
                    viewModel.ChoiceDatas.Add(new TutorChoiceData {
                        ProjectID = proj,
                        ProjectName = result.FirstOrDefault(x => x.ProjectID == proj)?.ProjectName,
                        Qty = result.FirstOrDefault(x => x.ProjectID == proj)?.Qty,
                        Choices = result.Where(x => x.ProjectID == proj).OrderBy(t => t.SortOrderNumber).ToList(),
                        ProjectIsClosed = result.FirstOrDefault(x => x.ProjectID == proj)?.ProjectIsClosed
                    });
                }

                return new JsonResult(viewModel);
            } catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
        
        [Route("api/[controller]/set_ready")]
        [HttpPatch]
        public async Task<IActionResult> SetReady([FromQuery] int tutorId)
        {
            try
            {
                await tutorRepository.SetReadyAsync(tutorId);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message);
            }
        }

        [Route("api/[controller]/saveChoice")]
        [HttpPatch]
        public IActionResult SaveChoice([FromBody] List<TutorChoice_1> data, [FromQuery] int tutorId)
        {
            try
            {
                tutorRepository.SetPreferences(data, tutorId);
                return Ok();
            }
            catch (SqlException ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
    }
}