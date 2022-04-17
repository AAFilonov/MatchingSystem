using System;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.UI.ResultModels;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class AllocationController : ControllerBase
    {
        private readonly IMatchingRepository matchingRepository;

        public AllocationController(IMatchingRepository matchingRepository)
        {
            this.matchingRepository = matchingRepository;
        }

        [Route("api/[controller]/getFinalAllocations")]
        [HttpGet]
        public IActionResult GetAllocations()
        {
            try
            {
                var model = new AllocationData
                {
                    Allocations = matchingRepository.GetFinalAllocations(),
                    Matchings = matchingRepository.GetMatchingsInfo()
                };
                return new JsonResult(model);
            }
            catch (Exception ex)
            {
                return Problem("произошла неизвестная ошибка.", statusCode: 500);
            }
        }
    }
}