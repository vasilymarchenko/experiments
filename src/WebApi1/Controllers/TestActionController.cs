using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi1.Services;

namespace WebApi1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestActionController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            TestWorker.Run(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            return Ok();
        }
    }
}
