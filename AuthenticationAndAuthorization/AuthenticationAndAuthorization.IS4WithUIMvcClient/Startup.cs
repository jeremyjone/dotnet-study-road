using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AuthenticationAndAuthorization.IS4WithUIMvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // 关闭默认 Token 命名空间
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddAuthentication(options =>
                {
                    // 配置协议
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                // 如果使用 Chrome 在登录后无法从 signin-oidc 路径跳转到目标路径，如下操作：
                // 打开浏览器键入 chrome://flags，搜索 Cookies without SameSite must be secure，将其设置为 Disabled 即可。
                .AddOpenIdConnect("oidc", options =>
                {
                    // 如果不启用 https，则需要添加该 meta 项
                    //options.RequireHttpsMetadata = false;
                    // 这个地址是认证服务器的地址
                    options.Authority = "https://localhost:5001";
                    // 下面内容需要与认证服务器的内容保持一致
                    options.ClientId = "mvc_client";
                    options.ClientSecret = "www.jeremyjone.com";
                    // 认证方式
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    // 验证是否包含某个作用域
                    options.Scope.Add("api.jeremyjone.com");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
