using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationAndAuthorization.Jwt.Controllers
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

        /// <summary>
        /// 权限获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // 这样就添加了授权方案
        [Authorize]
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

        /// <summary>
        /// 颁发 token
        /// </summary>
        /// <returns></returns>
        [HttpGet("token")]
        public ActionResult GetToken()
        {
            // 秘钥，与验证秘钥相同
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("www.jeremyjone.com"));
            var token = new JwtSecurityToken(
                issuer: "jeremyjone@qq.com",
                audience: "jeremyjone",
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                // claim 可以任意填写内容
                claims: new Claim[] { });

            // 注意需要 handler 写入
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
