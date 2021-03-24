// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Reflection;
using AuthenticationAndAuthorization.EF.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationAndAuthorization.EF
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            #region 添加数据库

            var connectionMySql = Configuration["ConnectionStrings:MySQL"];
            if (string.IsNullOrWhiteSpace(connectionMySql))
            {
                throw new Exception("数据库配置异常，请检查 appsettings.json");
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionMySql));

            #endregion

            #region 配置身份参数

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                // 添加实体库
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            #endregion

            #region 配置 IS4 服务

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                    options.EmitStaticAudienceClaim = true;
                })
                // 注释掉测试用户，使用数据库的内容
                //.AddTestUsers(TestUsers.Users)
                // 配置 IS4 使用哪种用户模型
                .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources, CORS)
                .AddConfigurationStore(options =>
                {
                    //options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);
                    // 使用 MySQL
                    options.ConfigureDbContext = b =>
                        b.UseMySql(connectionMySql, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    //options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);
                    // 使用 MySQL
                    options.ConfigureDbContext = b =>
                        b.UseMySql(connectionMySql, sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                });

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            #endregion

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to https://localhost:5001/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            // 注册使用 IS4 服务。它需要在 *路由* 之后，*授权* 之前。
            app.UseIdentityServer();
            
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
