using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Log.Serilog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Serilog 最简单用法

            //using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
            //{
            //    log.Information("This is a serilog info.");
            //}

            #endregion

            #region Serilog 进行配置

            global::Serilog.Log.Logger = new LoggerConfiguration()
                // 对其自身限制
                .MinimumLevel.Debug()
                // 重写其它日志规则，捕获 Microsoft 高于 Debug 级别的日志并输出到 Serilog
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                // 写入控制台
                .WriteTo.Console()
                // 写入文件，这里配置为生成文件按天
                .WriteTo.File(Path.Combine("Logs", @"serilog.log"), rollingInterval: RollingInterval.Day)
                // 写入 DB，这里使用 SQLServer，也可以通过插件使用其他数据库
                .WriteTo.MSSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", autoCreateSqlTable: true, tableName: "Logs")
                .CreateLogger();

            // 输出
            global::Serilog.Log.Information("Serilog information!!!");

            #endregion

            // 对程序整体进行处理，最后释放空间
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                // 最后一定要释放空间
                global::Serilog.Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        // 将 Serilog 注入中间件，将 Serilog 与 ILogger 绑定，其配置才会生效
                        .UseSerilog()
                        .UseStartup<Startup>();
                });
    }
}
