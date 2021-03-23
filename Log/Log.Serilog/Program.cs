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
            #region Serilog ����÷�

            //using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
            //{
            //    log.Information("This is a serilog info.");
            //}

            #endregion

            #region Serilog ��������

            global::Serilog.Log.Logger = new LoggerConfiguration()
                // ������������
                .MinimumLevel.Debug()
                // ��д������־���򣬲��� Microsoft ���� Debug �������־������� Serilog
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                // д�����̨
                .WriteTo.Console()
                // д���ļ�����������Ϊ�����ļ�����
                .WriteTo.File(Path.Combine("Logs", @"serilog.log"), rollingInterval: RollingInterval.Day)
                // д�� DB������ʹ�� SQLServer��Ҳ����ͨ�����ʹ���������ݿ�
                .WriteTo.MSSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", autoCreateSqlTable: true, tableName: "Logs")
                .CreateLogger();

            // ���
            global::Serilog.Log.Information("Serilog information!!!");

            #endregion

            // �Գ���������д�������ͷſռ�
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                // ���һ��Ҫ�ͷſռ�
                global::Serilog.Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        // �� Serilog ע���м������ Serilog �� ILogger �󶨣������òŻ���Ч
                        .UseSerilog()
                        .UseStartup<Startup>();
                });
    }
}
