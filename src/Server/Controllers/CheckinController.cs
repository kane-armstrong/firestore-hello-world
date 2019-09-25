using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Application;

namespace Server.Controllers
{
    [Route("api/checkin")]
    public class CheckinController : ControllerBase
    {
        private readonly IAdviserNotifier _notifier;

        public CheckinController(IAdviserNotifier notifier)
        {
            _notifier = notifier;
        }

        [HttpPost]
        public async Task<IActionResult> Checkin([FromQuery]int adviserId, [FromBody]VisitorDetails details)
        {
            await _notifier.NotifyVisitorArrived(adviserId, details);
            return Ok();
        }
    }
}