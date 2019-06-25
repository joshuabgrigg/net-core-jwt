using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreJwt.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return new OkObjectResult(new { message = "woohoo, you're in!" });
        }
    }
}