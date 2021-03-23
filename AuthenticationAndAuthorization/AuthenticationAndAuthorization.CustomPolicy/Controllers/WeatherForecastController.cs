using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AuthenticationAndAuthorization.CustomPolicy.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationAndAuthorization.CustomPolicy.Controllers
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
        /// 认证测试 Api
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        #region 基于策略的授权

        // 现在就可以通过一个整型参数实现更加灵活的授权
        [MinimumAgeAuthorize(10)]

        #endregion

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
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("www.jeremyjone.com"));
            var token = new JwtSecurityToken(
                issuer: "jeremyjone@qq.com",
                audience: "jeremyjone",
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                claims: new Claim[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Role, "User"),
                    // 自定义声明
                    new Claim(ClaimTypes.DateOfBirth, new DateTime(1998, 7, 20).ToString(CultureInfo.InvariantCulture)),
                });

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
