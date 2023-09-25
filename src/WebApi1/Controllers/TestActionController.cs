using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi1.Services;

namespace WebApi1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestActionController : ControllerBase
    {
        private readonly TestWorker _testWorker;

        public TestActionController(TestWorker testWorker)
        {
            _testWorker = testWorker;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            _testWorker.Run(TestWorker.GetRandomOperation(), "dummy", 123);
            return Ok();
        }
    }
}
