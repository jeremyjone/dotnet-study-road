using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace WebApplication1
{
    public class Startup
    {
        internal readonly string[] MultiVersion = { "v1", "v2", "user" };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddAuthorization();
            services.AddSwaggerGen(c =>
            {
                foreach (var v in MultiVersion)
                {
                    c.SwaggerDoc(v, new OpenApiInfo { Title = "WebApplication1", Version = v });
                }

                #region �Զ�����Ϣ

                //c.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Title = "WebApplication1",
                //    Version = "v1",
                //    Description = "v1 document",
                //    Contact = new OpenApiContact
                //    {
                //        Name = "Jeremy Jone",
                //        Email = "jeremyjone@qq.com"
                //    },
                //    License = new OpenApiLicense
                //    {
                //        Name = "Apache 2.0",
                //        Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
                //    }
                //});

                #endregion

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "WebApplication1.xml");
                c.IncludeXmlComments(filePath);


                #region ��֤

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            // ��������޸� Url
                            AuthorizationUrl = new Uri("/auth-server/connect/authorize", UriKind.RelativeOrAbsolute),
                            Scopes = new Dictionary<string, string>
                            {
                                { "readAccess", "Access read operations" },
                                { "writeAccess", "Access write operations" }
                            }
                        },

                        // �����ʽ����ͬʱ����
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            // ��������޸� Url
                            AuthorizationUrl = new Uri("auth-server/connect/authorize", UriKind.RelativeOrAbsolute),
                            TokenUrl = new Uri("auth-server/connect/token", UriKind.RelativeOrAbsolute),
                            Scopes = new Dictionary<string, string> {
                                { "OidcApiName", "ApiName" }
                            }
                        }
                    }
                });

                #endregion
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    foreach (var v in MultiVersion)
                    {
                        c.SwaggerEndpoint($"/swagger/{v}/swagger.json", $"WebApplication1 {v}");
                    }
                    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1");

                    #region ������֤��������

                    // �������ݻ����ĵ����Զ���д��������Ҫ��д����

                    c.OAuthClientId("OidcSwaggerUIClientId");
                    c.OAuthClientSecret("OidcSwaggerUIClientSecret");
                    c.OAuthAppName("ApiName");
                    c.OAuthRealm("ApiRealm");
                    c.OAuthScopeSeparator(" ");
                    c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "foo", "bar" } });
                    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                    c.OAuthUsePkce();

                    #endregion
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
