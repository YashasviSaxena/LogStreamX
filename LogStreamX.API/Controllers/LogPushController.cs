using LogStreamX.Infrastructure.Models;
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
            return Ok(new
            {
                status = "received",
                dto
            });
        }
    }
}