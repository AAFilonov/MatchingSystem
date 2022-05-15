using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchingSystem.UI.ResultModels;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.UI.RequestModels;

namespace Service.Executive
{
    internal interface IExecutiveService
    {
        public IActionResult SetNextStage([FromQuery] int? matchingId, [FromQuery] int? userId);

        public IActionResult SetEndDate([FromForm] string endDate, [FromForm] int matchingId);

        public IActionResult GetAllocationByExecutive([FromQuery] int? userId, [FromQuery] int? matchingId);

        public IActionResult SetAllocationByExecutive([FromBody] AdjustmentRequest request);

    }
}
