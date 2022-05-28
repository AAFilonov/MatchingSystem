using System;
using System.Collections.Generic;
using MatchingSystem.DataLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Service.Notification;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }
  
        [Route("api/[controller]/get_notifications")]
        [HttpGet]
        public IActionResult GetNotificationsByExecutive(int userId, int matchingId)
        {
            var result = notificationService.GetNotificationsByExecutive(userId, matchingId);
            return new JsonResult(result);
        }

        [Route("api/[controller]/getNotificationsByTutor")]
        [HttpGet]
        public IActionResult GetNotificationsByTutor(int tutorId, int userId, int matchingId)
        {
            var result = notificationService.GetNotificationsByTutor(tutorId, userId,matchingId);
            return new JsonResult(result);
        }
    }
}