using crawler_base.Helpers;
using data_access_layer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using service_layer;
using System.IO;

namespace crawler_base
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory());
              //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<IMyConfiguration, MyConfiguration>();

            services.AddServices();

            /*
            // Ad db service.
            services.AddDbContext<PrinterContext>(
                options => options.UseSqlite($"Filename={Path.Combine(DirectoryHelpers.GetCurrentSolutionDirectory(), "PrinterDB.db")}"));

            // Add NLog service.
            services.AddLogging(loggingBuilder => {
                loggingBuilder.AddNLog(@"Configs\Nlog.config");
            });
            */


            services.AddTransient<ConsoleApp>();
        }
    }
}