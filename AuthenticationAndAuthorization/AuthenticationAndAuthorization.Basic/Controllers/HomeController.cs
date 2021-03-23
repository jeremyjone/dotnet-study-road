using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAndAuthorization._01Basic.Controllers
{
    /// <summary>
    /// 控制器
    /// 整个控制器将需要授权
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// 无授权方案
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 授权方案
        /// 授权也可以单独给某一个方法
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        /// <summary>
        /// 认证
        /// 在整个控制器都需要授权的情况下，可以单独修改某一方法的授权方案
        /// </summary>
        /// <returns></returns>
        // 不需要任何授权
        [AllowAnonymous]
        public IActionResult Auth()
        {
            // 添加一些声明
            var claims1 = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Jz"),
                new Claim(ClaimTypes.Email, "Jz@qq.com"),
                // 声明的键是可以自定义的
                new Claim("Custom-Claim", "This is a custom claim.")
            };

            // 创建一个身份声明
            var identity1 = new ClaimsIdentity(claims1, "Claims1");

            // 添加第二个声明
            var claims2 = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Tom"),
                new Claim(ClaimTypes.Email, "Tom@qq.com")
            };

            // 创建第二个身份声明
            var identity2 = new ClaimsIdentity(claims2, "Claims2");

            // 将两个身份声明放到用户身份中
            var principal = new ClaimsPrincipal(new[] {identity1, identity2});

            // 签入身份验证方案的主体
            HttpContext.SignInAsync(principal);

            return RedirectToAction("Index");
        }
    }
}
