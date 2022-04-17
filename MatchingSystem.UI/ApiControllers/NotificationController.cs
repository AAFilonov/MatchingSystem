using System;
using System.Collections.Generic;
using MatchingSystem.DataLayer.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IExecutiveRepository executiveRepository;
        private readonly IUserRepository userRepository;

        public NotificationController(ITutorRepository tutorRepository, IExecutiveRepository executiveRepository, IUserRepository userRepository)
        {
            this.tutorRepository = tutorRepository;
            this.executiveRepository = executiveRepository;
            this.userRepository = userRepository;
        }
  
        [Route("api/[controller]/get_notifications")]
        [HttpGet]
        public IActionResult GetNotificationsByExecutive(int userId, int matchingId)
        {
            var actionResult = new Dictionary<string, int>(1);

            try
            {
                var result = executiveRepository.GetNotificationsCountByExecutive(userId, matchingId);

                actionResult.Add("count", result);
                userRepository.ReadNotifications(userId, matchingId);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, 500, "Внутренняя ошибка сервера");
            }

            return new JsonResult(actionResult);
        }

        [Route("api/[controller]/getNotificationsByTutor")]
        [HttpGet]
        public IActionResult GetNotificationsByTutor(int tutorId, int userId, int matchingId)
        {
            var count = tutorRepository.GetNotificationsCountByTutor(tutorId);

            var result = new Dictionary<string, int> {{"count", count}};

            try
            {
                userRepository.ReadNotifications(userId, matchingId, tutorId);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
            
            return new JsonResult(result);
        }
    }
}