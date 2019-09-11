using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notifier.API.Application;

namespace Notifier.API.Controllers
{
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IOrderChangeNotifier _notifier;

        public NotificationsController(IOrderChangeNotifier notifier)
        {
            _notifier = notifier;
        }

        [HttpPost("statuschange")]
        public async Task<IActionResult> NotifyStatusChange([FromBody]NotifyStatusChange statusChange)
        {
            await _notifier.NotifyOrderChanged(statusChange.LocationId, statusChange.OrderId);
            return Ok();
        }
    }
}