using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Models;
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

namespace PayDel.Api
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        //    try
        //    {
        //        CreateHostBuilder(args).Build().Run();
        //    }
        //    catch (Exception exception)
        //    {
        //        //NLog: catch setup errors
        //        logger.Error(exception, "Stopped program because of exception");
        //        throw;
        //    }
        //    finally
        //    {
        //        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        //        NLog.LogManager.Shutdown();
        //    }
        //}

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        })
        //        .ConfigureLogging((hostingContext, logging) =>
        //        {
        //            logging.AddNLog();
        //            logging.AddEntityFramework<LogDbContext, ExtendedLog>();
        //        });

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
