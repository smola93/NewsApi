using Microsoft.AspNetCore.Mvc;

namespace NewsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok();
    }
}