using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.UI.ResultModels;
using MatchingSystem.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly DataContext context;

        public TutorController(DataContext ctx) => context = ctx;

        [Route("api/[controller]/getChoice")]
        public async Task<IActionResult> GetChoice([FromQuery] int? tutorId)
        {
            try
            {
                var result = await context.GetTutorChoiceAsync(tutorId);

                var projects = result.Select(x => x.ProjectID).Distinct();

                IterationData model = new IterationData();

                model.CommonQuota = await context.GetCommonQuotaAsync(tutorId);

                foreach (var proj in projects)
                {
                    model.ChoiceDatas.Add(new TutorChoiceData {
                        ProjectID = proj,
                        ProjectName = result.Find(x => x.ProjectID == proj)?.ProjectName,
                        Qty = result.Find(x => x.ProjectID == proj)?.Qty,
                        Choices = result.FindAll(x => x.ProjectID == proj).OrderBy(t => t.SortOrderNumber).ToList(),
                        ProjectIsClosed = result.Find(x => x.ProjectID == proj)?.ProjectIsClosed
                    });
                }

                return new JsonResult(model);
            } catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
        
        [Route("api/[controller]/set_ready")]
        [HttpPatch]
        public async Task<IActionResult> SetReady([FromQuery] int? tutorId)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync("exec napp.upd_Tutor_IsReadyToStart " +
                                                          "@TutorID, " +
                                                          "@IsReady",
                    new SqlParameter("@TutorID", tutorId),
                    new SqlParameter("@IsReady", true));
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message);
            }
        }

        [Route("api/[controller]/saveChoice")]
        [HttpPatch]
        public async Task<IActionResult> SaveChoice([FromBody] List<TutorChoice_1> data, [FromQuery] int? tutorId)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("ChoiceID", typeof(int));
                table.Columns.Add("SortOrderNumber", typeof(short));
                table.Columns.Add("IsInQuota", typeof(bool));

                foreach (var item in data)
                {
                    var row = table.NewRow();
                    row.SetField("ChoiceID", item.ChoiceID);
                    row.SetField("SortOrderNumber", item.SortOrderNumber);
                    row.SetField("IsInQuota", item.IsInQuota);
                    table.Rows.Add(row);
                }

                await context.Database.ExecuteSqlRawAsync("exec napp.upd_TutorsChoice @Choices, @TutorID",
                    new SqlParameter("@Choices", table) { TypeName = "dbo.TutorsChoice_1"},
                    new SqlParameter("@TutorID", tutorId));
                return Ok();
            }
            catch (SqlException ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
    }
}