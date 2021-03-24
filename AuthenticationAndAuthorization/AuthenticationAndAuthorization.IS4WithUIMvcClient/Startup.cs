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

            // �ر�Ĭ�� Token �����ռ�
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddAuthentication(options =>
                {
                    // ����Э��
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                // ���ʹ�� Chrome �ڵ�¼���޷��� signin-oidc ·����ת��Ŀ��·�������²�����
                // ����������� chrome://flags������ Cookies without SameSite must be secure����������Ϊ Disabled ���ɡ�
                .AddOpenIdConnect("oidc", options =>
                {
                    // ��������� https������Ҫ��Ӹ� meta ��
                    //options.RequireHttpsMetadata = false;
                    // �����ַ����֤�������ĵ�ַ
                    options.Authority = "https://localhost:5001";
                    // ����������Ҫ����֤�����������ݱ���һ��
                    options.ClientId = "mvc_client";
                    options.ClientSecret = "www.jeremyjone.com";
                    // ��֤��ʽ
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    // ��֤�Ƿ����ĳ��������
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
