using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationAndAuthorization.Jwt
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

            services.AddControllers();

            #region 添加 Jwt 认证服务

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // 需要认证哪些内容，就填写哪些
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    // 这里将 audience 的值修改。如果不验证，则通过，需要验证则不通过，可以修改 false 为 true 测试
                    ValidateAudience = false,
                    ValidAudience = "jeremyjone1", // 应该是 jeremyjone

                    // 所有验证内容需要和颁发时的内容一致
                    ValidateIssuer = true,
                    ValidIssuer = "jeremyjone@qq.com",

                    // 尤其是该秘钥字段，该字段属于绝密内容
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("www.jeremyjone.com")),

                    // 验证有效期
                    ValidateLifetime = true

                    // 上面的内容属于建议但不强制验证
                    // 还可以添加其他内容
                });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // 同样添加鉴权
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
