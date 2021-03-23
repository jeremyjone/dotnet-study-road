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

namespace AuthenticationAndAuthorization.Role.Controllers
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

        #region 基于角色的授权

        // 使用特定角色权限
        [Authorize(Roles = "Admin")]

        // 多个角色使用逗号（,）分开，它们是 “或” 的关系
        //[Authorize(Roles = "Admin,User")]

        // 多个角色分开写是 “且” 的关系
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "Super")]

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
                    // 角色需要在这里填写
                    new Claim(ClaimTypes.Role, "Admin"),
                    // 多个角色可以重复写，生成的 JWT 会是一个数组
                    new Claim(ClaimTypes.Role, "Super")
                });

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
