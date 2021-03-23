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

            #region Jwt ��֤

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

            #region ���ڲ�����Ȩ

            services.AddAuthorization(options =>
            {
                // ������ɫ�Ĳ���
                options.AddPolicy("AdminAndSuper",
                    policy => policy.RequireRole("Admin").RequireRole("Super"));
                options.AddPolicy("AdminOrSuper", policy => policy.RequireRole("Admin", "Super"));

                // ���������Ĳ���
                options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNo"));
                options.AddPolicy("Founders", policy =>
                    // ���������ָ������ֵ���б�
                    policy.RequireClaim("EmployeeNo", "1", "2", "3", "4", "5"));

                // �Զ������
                options.AddPolicy("HasBirthDay",
                    // ʵ��һ�����׵��Զ������
                    policy => policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth)));


                // �̳��� IAuthorizationRequirement �ӿڵĲ���
                options.AddPolicy("AtLeast18", policy =>
                    // ʵ��һ������18��Ĳ���
                    policy.Requirements.Add(new MinimumAgeRequirement(18)));
            });

            // ע���Զ�����ԵĴ������
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
