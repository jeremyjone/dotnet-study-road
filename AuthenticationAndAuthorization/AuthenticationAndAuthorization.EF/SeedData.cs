// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Linq;
using System.Security.Claims;
using AuthenticationAndAuthorization.EF.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationAndAuthorization.EF
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            #region 运行前须知

            /*
             * 运行前，需要先执行以下命令来创建数据库表
             *
             * 如果模型未修改，可以使用 ./MigrationsMySql 中的内容。若修改了模型结构，删除整个文件夹后重新生成即可。
             * > 如果使用 ./MigrationsMySql 中的内容，依次运行 update-database 命令即可（共3条）。
             * > 如果不使用 ./MigrationsMySql 中的内容，将整个文件夹删除，依次运行下面6条语句即可。
             *
             * PM> add-migration InitialIdentityServerPersistedGrantDbMigrationMysql -c PersistedGrantDbContext -o MigrationsMySql/PersistedGrantDb
             * Build started...
             * Build succeeded.
             * To undo this action, use Remove-Migration.
             *
             * PM> update-database -context PersistedGrantDbContext
             * Build started...
             * Build succeeded.
             * Done.
             *
             * PM> add-migration InitialIdentityServerConfigurationDbMigrationMysql -c ConfigurationDbContext -o MigrationsMySql/ConfigurationDb
             * Build started...
             * Build succeeded.
             * To undo this action, use Remove-Migration.
             *
             * PM> update-database -context ConfigurationDbContext
             * Build started...
             * Build succeeded.
             * Done.
             *
             * PM> add-migration AppDbMigration -c ApplicationDbContext -o MigrationsMySql
             * Build started...
             * Build succeeded.
             * To undo this action, use Remove-Migration.
             *
             * PM> update-database -context ApplicationDbContext
             * Build started...
             * Build succeeded.
             * Done.
             */

            #endregion

            //var services = new ServiceCollection();

            // 不需要 SQLite
            //services.AddOperationalDbContext(options =>
            //{
            //    options.ConfigureDbContext = db => db.UseSqlite(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            //});
            //services.AddConfigurationDbContext(options =>
            //{
            //    //options.ConfigureDbContext = db => db.UseMySql(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            //});

            // 不使用模板提供的 serviceProvider，通过 host 直接传递 ServiceProvider
            //var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
                context.Database.Migrate();
                EnsureSeedData(context);

                #region 添加用户数据

                var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
                ctx.Database.Migrate();
                EnsureSeedData(scope);

                #endregion
            }
        }

        #region 命令创建的函数，无修改

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Log.Debug("Clients being populated");
                foreach (var client in Config.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Log.Debug("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                Log.Debug("IdentityResources being populated");
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Log.Debug("IdentityResources already populated");
            }

            if (!context.ApiResources.Any())
            {
                Log.Debug("ApiScopes being populated");
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Log.Debug("ApiScopes already populated");
            }
        }

        #endregion

        #region 创建用户和角色

        private static void EnsureSeedData(IServiceScope scope)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // 创建角色
            foreach (var role in Config.Roles)
            {
                var res = roleManager.CreateAsync(role).Result;
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.First().Description);
                }
                Console.WriteLine($"{role.Name} created!");
            }

            // 创建用户
            foreach (var user in Config.Users)
            {
                // 默认密码为 Test23
                var res = userManager.CreateAsync(user, "Test_123").Result;
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.First().Description);
                }

                // 创建用户的声明
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Name, user.NickName),
                    new Claim(JwtClaimTypes.Email, user.Email)
                };

                res = userManager.AddClaimsAsync(user, claims).Result;
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.First().Description);
                }

                // 创建用户的角色
                var role = user.UserName == "user1" ? "admin" : "user";
                res = userManager.AddToRoleAsync(user, role).Result;
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.First().Description);
                }

                Console.WriteLine($"{user.NickName} created!");
            }
        }

        #endregion
    }
}
