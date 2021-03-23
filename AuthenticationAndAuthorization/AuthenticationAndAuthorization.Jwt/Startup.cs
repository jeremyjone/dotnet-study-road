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

            #region ��� Jwt ��֤����

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // ��Ҫ��֤��Щ���ݣ�����д��Щ
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    // ���ｫ audience ��ֵ�޸ġ��������֤����ͨ������Ҫ��֤��ͨ���������޸� false Ϊ true ����
                    ValidateAudience = false,
                    ValidAudience = "jeremyjone1",

                    // ������֤������Ҫ�Ͱ䷢ʱ������һ��
                    ValidateIssuer = true,
                    ValidIssuer = "jeremyjone@qq.com",

                    // �����Ǹ���Կ�ֶΣ����ֶ����ھ�������
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("www.jeremyjone.com")),

                    // ��֤��Ч��
                    ValidateLifetime = true

                    // ������������ڽ��鵫��ǿ����֤
                    // �����������������
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

            // ͬ����Ӽ�Ȩ
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
