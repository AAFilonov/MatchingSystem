/* Контроллер отвечающий за получение/прочтение уведомлений */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly DataContext context;

        public NotificationController(DataContext ctx)
        {
            context = ctx;
        }

        [Route("api/[controller]/get_notifications")]
        [HttpGet]
        public async Task<IActionResult> GetNotificationsByExecutive([FromQuery] int? userId, [FromQuery] int? matchingId)
        {
            Dictionary<string, int> actionResult = new Dictionary<string, int>(1);

            try
            {
                var result = await context.GetNotificationsCountByExecutive(userId, matchingId);

                actionResult.Add("count", result);
                context.ReadNotifications(userId, matchingId);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, 500, "Внутренняя ошибка сервера");
            }

            return new JsonResult(actionResult);
        }

        [Route("api/[controller]/getNotificationsByTutor")]
        [HttpGet]
        public async Task<IActionResult> GetNotificationsByTutor([FromQuery] int? tutorId, [FromQuery] int? userId, [FromQuery] int? matchingId)
        {
            var count = await context.GetNotificationsCountByTutor(tutorId);

            Dictionary<string, int> result = new Dictionary<string, int> {{"count", count}};

            try
            {
                context.ReadNotifications(userId, matchingId, tutorId);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
            
            return new JsonResult(result);
        }
    }
}