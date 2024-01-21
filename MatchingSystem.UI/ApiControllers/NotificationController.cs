using System;
using System.Collections.Generic;
using MatchingSystem.Service.Notification;
using Microsoft.AspNetCore.Mvc;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IQuotaNotificationService quotaNotificationService;

        public NotificationController(IQuotaNotificationService quotaNotificationService)
        {
            this.quotaNotificationService = quotaNotificationService;
        }
  
        [Route("api/[controller]/get_notifications")]
        [HttpGet]
        public IActionResult GetNotificationsByExecutive(int userId, int matchingId)
        {
            var result = quotaNotificationService.ReadNotificationsByExecutive(matchingId);
            return new JsonResult(result);
        }

        [Route("api/[controller]/getNotificationsByTutor")]
        [HttpGet]
        public IActionResult GetNotificationsByTutor(int tutorId, int userId, int matchingId)
        {
            var result = quotaNotificationService.ReadNotificationsByTutor(tutorId);
            return new JsonResult(result);
        }
    }
}