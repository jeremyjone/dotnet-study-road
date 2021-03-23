using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Text;
using AuthenticationAndAuthorization.Policy.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationAndAuthorization.Policy
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

            #region Jwt 认证

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = "jeremyjone",

                        ValidateIssuer = true,
                        ValidIssuer = "jeremyjone@qq.com",

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("www.jeremyjone.com")),

                        ValidateLifetime = true,
                        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
                    };
                });

            #endregion

            #region 基于策略授权

            services.AddAuthorization(options =>
            {
                // 创建角色的策略
                options.AddPolicy("AdminAndSuper",
                    policy => policy.RequireRole("Admin").RequireRole("Super"));
                options.AddPolicy("AdminOrSuper", policy => policy.RequireRole("Admin", "Super"));

                // 创建声明的策略
                options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNo"));
                options.AddPolicy("Founders", policy =>
                    // 给声明添加指定允许值的列表
                    policy.RequireClaim("EmployeeNo", "1", "2", "3", "4", "5"));

                // 自定义策略
                options.AddPolicy("HasBirthDay",
                    // 实现一个简易的自定义策略
                    policy => policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth)));


                // 继承自 IAuthorizationRequirement 接口的策略
                options.AddPolicy("AtLeast18", policy =>
                    // 实现一个至少18岁的策略
                    policy.Requirements.Add(new MinimumAgeRequirement(18)));
            });

            // 注册自定义策略的处理程序
            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler2>();

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
