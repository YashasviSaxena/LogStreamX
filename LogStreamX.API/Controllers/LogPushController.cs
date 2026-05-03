using Microsoft.AspNetCore.Mvc;
using LogStreamX.Contracts;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogPushController : ControllerBase
    {
        [HttpPost]
        public IActionResult Push(LogDto dto)
        {
            // In real system: send to Kafka
            return Ok(new
            {
                status = "received",
                dto
            });
        }
    }
}