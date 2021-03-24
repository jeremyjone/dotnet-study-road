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
            // 秘钥，绝对私有的，使用该秘钥可以生成和验证所有 token
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("www.jeremyjone.com"));
            // 创建令牌
            var token = new JwtSecurityToken(
                // 发行人
                issuer: "jeremyjone@qq.com",
                // 接收人
                audience: "jeremyjone",
                // 有效时间
                expires: DateTime.UtcNow.AddHours(1),
                // 数字签名，使用指定的加密方式对秘钥进行加密
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                // 其他声明，这里可以任意填写
                claims: new Claim[]
                {
                    // 角色需要在这里填写
                    new Claim(ClaimTypes.Role, "Admin"),
                    // 多个角色可以重复写，生成的 JWT 会是一个数组
                    new Claim(ClaimTypes.Role, "Super")
                });

            // 写入 token 并生成 JWT
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
