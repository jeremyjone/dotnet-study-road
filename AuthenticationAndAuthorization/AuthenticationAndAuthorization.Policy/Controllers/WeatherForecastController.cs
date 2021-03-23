using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationAndAuthorization.Policy.Controllers
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

        // 使用角色的策略
        //[Authorize(Policy = "AdminOrSuper")]

        // 使用声明的策略
        [Authorize(Policy = "Founders")]

        // 使用自定义策略
        //[Authorize(Policy = "HasBirthDay")]
        //[Authorize(Policy = "AtLeast18")]

        // 注意：策略名要写对，否则报错

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
                    new Claim("EmployeeNo", "1"),
                    new Claim(ClaimTypes.DateOfBirth, new DateTime(2010, 1, 1).ToString(CultureInfo.InvariantCulture)),
                });

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
