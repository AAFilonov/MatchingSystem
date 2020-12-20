using System;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.UI.ResultModels;
using MatchingSystem.UI.Services;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class AllocationController : ControllerBase
    {
        private readonly DataContext context;

        public AllocationController(DataContext ctx) => context = ctx;

        [Route("api/[controller]/getFinalAllocations")]
        [HttpGet]
        public async Task<IActionResult> GetAllocations()
        {
            try
            {
                var model = new AllocationData
                {
                    Allocations = await context.GetFinalAllocation(),
                    Matchings = await context.GetMatchingsInfoAsync()
                };
                return new JsonResult(model);
            }
            catch (Exception ex)
            {
                return Problem(detail: "произошла неизвестная ошибка.", statusCode: 500);
            }
        }
    }
}