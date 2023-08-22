using data_access_layer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using service_layer.Helpers;
using System.IO;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using service_layer.Settings;

namespace service_layer
{
    public static class ServiceManager
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //services.AddSingleton<IConfigurationRoot>(Configuration);
            //services.AddSingleton<IMyConfiguration, MyConfiguration>();

            // Add settings service.
            services.AddSingleton<IConfigurator, Configurator>();

            // Add db service.
            services.AddDbContext<PrinterContext>(
                options => options.UseSqlite($"Filename={Path.Combine(DirectoryHelpers.GetCurrentSolutionDirectory(), "PrinterDB.db")}"));

            // Add NLog service.
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog(@"Configs\Nlog.config");
                loggingBuilder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

                //loggingBuilder.AddSingleton<ILoggerFactory>(services => new NLog.Extensions.Logging.NLogLoggerFactory());
                //loggingBuilder.AddConfiguration(config.GetSection("Logging"));
            });

            return services;
        }
    }
}