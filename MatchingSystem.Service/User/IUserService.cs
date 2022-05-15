using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;

namespace Service.User
{
    internal interface IUserService
    {
        public IActionResult GetMatchingsForUser([FromQuery] int userId);

        public IActionResult GetRolesForUser([FromQuery] int matchingId, [FromQuery] int userId);
    }
}
