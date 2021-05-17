using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Jz.IdServer
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
             * PM> add-migration InitialIdentityServerPersistedGrantDbMigrationMysql -c PersistedGrantDbContext -o Data/MigrationsMySql/IdentityServer/PersistedGrantDb
             * Build started...
             * Build succeeded.
             * To undo this action, use Remove-Migration.
             *
             * PM> update-database -context PersistedGrantDbContext
             * Build started...
             * Build succeeded.
             * Done.
             *
             * PM> add-migration InitialIdentityServerConfigurationDbMigrationMysql -c ConfigurationDbContext -o Data/MigrationsMySql/IdentityServer/ConfigurationDb
             * Build started...
             * Build succeeded.
             * To undo this action, use Remove-Migration.
             *
             * PM> update-database -context ConfigurationDbContext
             * Build started...
             * Build succeeded.
             * Done.
             *
             * PM> add-migration AppDbMigration -c ApplicationDbContext -o Data/MigrationsMySql
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

            //using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            //scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            //var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            //context.Database.Migrate();
            //EnsureSeedData(context);

            //#region 添加用户数据

            //var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
            //ctx.Database.Migrate();

            //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            //foreach (var user in TestData.Users)
            //{
            //    var res = userManager.CreateAsync(user, "Test123").Result;
            //    if (!res.Succeeded)
            //    {
            //        throw new Exception(res.Errors.First().Description);
            //    }

            //    var claims = new List<Claim>
            //    {
            //        new Claim(JwtClaimTypes.Name, user.NickName),
            //        new Claim(JwtClaimTypes.Email, user.Email),
            //        new Claim("role_name", "super"),
            //        new Claim(JwtClaimTypes.Role, "1")
            //    };

            //    res = userManager.AddClaimsAsync(user, claims).Result;
            //    if (!res.Succeeded)
            //    {
            //        throw new Exception(res.Errors.First().Description);
            //    }
            //    Console.WriteLine($"{user.NickName} created!");
            //}

            //foreach (var role in TestData.Roles)
            //{
            //    var res = roleManager.CreateAsync(role).Result;
            //    if (!res.Succeeded)
            //    {
            //        throw new Exception(res.Errors.First().Description);
            //    }
            //    Console.WriteLine($"{role.Name} created!");
            //}

            //#endregion
        }

        //    private static void EnsureSeedData(ConfigurationDbContext context)
        //    {
        //        if (!context.Clients.Any())
        //        {
        //            Log.Debug("Clients being populated");
        //            foreach (var client in Config.Clients.ToList())
        //            {
        //                context.Clients.Add(client.ToEntity());
        //            }
        //            context.SaveChanges();
        //        }
        //        else
        //        {
        //            Log.Debug("Clients already populated");
        //        }

        //        if (!context.IdentityResources.Any())
        //        {
        //            Log.Debug("IdentityResources being populated");
        //            foreach (var resource in Config.IdentityResources.ToList())
        //            {
        //                context.IdentityResources.Add(resource.ToEntity());
        //            }
        //            context.SaveChanges();
        //        }
        //        else
        //        {
        //            Log.Debug("IdentityResources already populated");
        //        }

        //        if (!context.ApiResources.Any())
        //        {
        //            Log.Debug("ApiScopes being populated");
        //            foreach (var resource in Config.ApiScopes.ToList())
        //            {
        //                context.ApiScopes.Add(resource.ToEntity());
        //            }
        //            context.SaveChanges();
        //        }
        //        else
        //        {
        //            Log.Debug("ApiScopes already populated");
        //        }
        //    }
        //}

        //internal class TestData
        //{
        //    public static IEnumerable<ApplicationUser> Users =>
        //        new[]
        //        {
        //            new ApplicationUser
        //            {
        //                BirthDate = DateTime.Now,
        //                Email = "user1@qq.com",
        //                UserName = "user1",
        //                NickName = "用户1",
        //                Sex = Sex.Male,
        //                EmailConfirmed = true
        //            },
        //            new ApplicationUser
        //            {
        //                BirthDate = DateTime.Now,
        //                Email = "user2@qq.com",
        //                UserName = "user2",
        //                NickName = "用户2",
        //                Sex = Sex.Female,
        //                EmailConfirmed = true
        //            },
        //        };

        //    public static IEnumerable<ApplicationRole> Roles =>
        //        new[]
        //        {
        //            new ApplicationRole
        //            {
        //                Name = "super",
        //                Description = "超级管理员",
        //            },
        //            new ApplicationRole
        //            {
        //                Name = "admin",
        //                Description = "管理员",
        //            },
        //            new ApplicationRole
        //            {
        //                Name = "vip",
        //                Description = "超级会员",
        //            },
        //            new ApplicationRole
        //            {
        //                Name = "member",
        //                Description = "会员",
        //            },
        //            new ApplicationRole
        //            {
        //                Name = "user",
        //                Description = "用户",
        //            },
        //            new ApplicationRole
        //            {
        //                Name = "guest",
        //                Description = "访客",
        //            },
        //        };
    }
}
