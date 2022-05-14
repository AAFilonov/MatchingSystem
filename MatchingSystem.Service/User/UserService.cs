using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;

namespace Service.User
{
    internal class UserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMatchingRepository matchingRepository;

        public UserService(IUserRepository userRepository, IMatchingRepository matchingRepository)
        {
            this.matchingRepository = matchingRepository;
            this.userRepository = userRepository;
        }

        public IActionResult GetMatchingsForUser([FromQuery] int userId)
        {
            var model = matchingRepository.GetMatchingsByUser(userId).OrderByDescending(matching => matching.MatchingID);
            return new JsonResult(model);
        }

        public IActionResult GetRolesForUser([FromQuery] int matchingId, [FromQuery] int userId)
        {
            var model = userRepository.GetRolesForUserAndMatching(userId, matchingId);

            return new JsonResult(model);
        }
    }
}
