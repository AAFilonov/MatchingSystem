using System;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Dto;
using Service.Allocation;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class AllocationController : ControllerBase
    {
        private readonly IAllocationService allocationService;

        public AllocationController(IAllocationService allocationService)
        {
            this.allocationService = allocationService;
        }

        [Route("api/[controller]/getFinalAllocations")]
        [HttpGet]
        public AllocationData GetAllocations()
        {
            var model = allocationService.GetAllocations();
            return model;
        }
    }
}