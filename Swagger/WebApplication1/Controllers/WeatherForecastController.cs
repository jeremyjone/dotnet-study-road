using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("index")]
        //[Route("index")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = "user")]
        public string Post([FromBody] Product product)
        {
            return $"我们收到您的产品，名字为{product.Name}";
        }

        /// <summary>
        /// 返回天气列表
        /// </summary>
        /// <remarks>Remarks!</remarks>
        /// <response code="201">成功啦</response>
        /// <response code="400">好像缺点什么</response>
        /// <response code="500">服务器崩啦</response>
        [HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<WeatherForecast>), 201)]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public IEnumerable<WeatherForecast> MyFunc()
        {
            var rng = new Random();
            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
                .ToArray();
        }
    }
}
