using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Log.APM.Value.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var r = await client.GetAsync("https://www.baidu.com");
            if (r.IsSuccessStatusCode) return Ok();
            return BadRequest();
        }
    }
}
