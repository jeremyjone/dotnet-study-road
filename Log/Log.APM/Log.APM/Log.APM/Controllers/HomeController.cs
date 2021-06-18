using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Log.APM.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Index string");
        }

        [HttpGet]
        public IActionResult Wait()
        {
            Thread.Sleep(1000);
            return Ok("Wait string");
        }

        [HttpGet]
        public async Task<IActionResult> Weather()
        {
            var client = new HttpClient();
            var r = await client.GetAsync("https://localhost:44325/WeatherForecast");

            if (!r.IsSuccessStatusCode) return BadRequest();

            var result = r.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
            return Ok(result);
        }
    }
}
