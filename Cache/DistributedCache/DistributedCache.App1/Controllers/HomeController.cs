using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DistributedCache.App1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace DistributedCache.App1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _cache;

        public HomeController(ILogger<HomeController> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            string time;
            const string key = "CachedTime";

            // 获取到的内容是 byte[]
            var encodedTime = await _cache.GetAsync(key);

            if (encodedTime != null)
            {
                // 转换格式
                time = Encoding.UTF8.GetString(encodedTime);
            }
            else
            {
                // 为空的话，创建一个缓存
                time = DateTime.Now.ToLongTimeString();
                var byteTime = Encoding.UTF8.GetBytes(time);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));
                await _cache.SetAsync(key, byteTime, options);
            }

            ViewData["time"] = time;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
