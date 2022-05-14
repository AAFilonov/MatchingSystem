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
    internal interface IAllocationservice
    {
        void ReadNotifications(int userId, int matchingId);
        IActionResult GetAllocations();
    }
}
