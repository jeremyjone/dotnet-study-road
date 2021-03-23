using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthenticationAndAuthorization.IdentityServer4
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
            // ��� IdentityServer ����
            // �������ú��Ѿ����Է����ˡ�
            // [URL]/.well-known/openid-configuration
            var builder = services.AddIdentityServer()
                .AddInMemoryApiScopes(Config.Apis)
                .AddInMemoryClients(Config.Clients);
                //.AddTestUsers(Config.Users);

            // ����������������ʱǩ����֤
            builder.AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // ����м��
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync("Hello World!");
                    });
                });
        }
    }
}
