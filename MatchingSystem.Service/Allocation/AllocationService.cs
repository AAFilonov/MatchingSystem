using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchingSystem.UI.ResultModels;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;

namespace Service.Allocation
{
    internal class AllocationService
    {
        private readonly IMatchingRepository matchingRepository;

        public AllocationService(IMatchingRepository matchingRepository)
        {
            this.matchingRepository = matchingRepository;
        }

        public IActionResult GetAllocations()
        {
            var model = new AllocationData
            {
                Allocations = matchingRepository.GetFinalAllocations(),
                Matchings = matchingRepository.GetMatchingsInfo()
            };
            return new JsonResult(model);
        }
    }
}
