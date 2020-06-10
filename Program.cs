using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CityInfoAPI.Contexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace CityInfoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder
                   .ConfigureNLog("nlog.config")
                   .GetCurrentClassLogger();

            try
            {
                logger.Info("Initializing application...JEOÑFJEOJDLJCOVLXKM{ODCJ...");
                var host = CreateWebHostBuilder(args).Build();
                //CreateWebHostBuilder(args).Build().Run();

                using(var scope = host.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetService<CityInfoContext>();
                        // for demo purposes, delete the database & migrate on startup so 
                        // we can start with a clean slate
                        context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error ocurred while migrating the database.");
                    }
                }

                //run the web app
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Aplication stoped because of exception.");
                throw;
            }
            finally //limpiamos todo y hacemos //llamando para desbloquear el admin de registro de puntos
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseNLog();                
    }
}
