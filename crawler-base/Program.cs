using crawler_base.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using NLog.Extensions.Logging;
using NLog;
using Microsoft.Extensions.Logging;
using System.IO;

namespace crawler_base
{
    class Program
    {
        public static void Main(string[] args)
        {
            //LogManager.Configuration = 
            //    new NLog.Config.XmlLoggingConfiguration(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\NLog.config");
            //var logger = LogManager.GetCurrentClassLogger();

            try
            {
                //logger.Info("Init main.");

                var dbFile = Path.Combine(DirectoryHelpers.GetCurrentSolutionDirectory(), "PrinterDB.db");

                if (File.Exists(dbFile))
                {
                    File.Delete(dbFile);
                }

                IServiceCollection services = new ServiceCollection();

                Startup startup = new Startup();
                startup.ConfigureServices(services);

                IServiceProvider serviceProvider = services.BuildServiceProvider();

                // entry to run app
                serviceProvider.GetService<ConsoleApp>().Run1();

                //serviceProvider.GetService<ILogger<Program>>().LogWarning("test");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            //Init init = new Init();
            //init.InitDALForPrinters();
            /*
            Parser parser = new Parser();

            var lexD = parser.ParseRawDataFromFile(@"E:\work\Projects\print-data-crawler\crawler-test\TestFiles\LexmarkMX421ade.html", web_page_parser.Data.Enums.PrinterType.LexmarkMS421dn).Result;
            var okiD = parser.ParseRawDataFromFile(@"E:\work\Projects\print-data-crawler\crawler-test\TestFiles\OkiMB491.html", web_page_parser.Data.Enums.PrinterType.OkiMB491).Result;

            Console.WriteLine(lexD.TonerLevel + " " + lexD.DrumLevel + " " + lexD.NumberOfPages);
            Console.WriteLine(okiD.TonerLevel + " " + okiD.DrumLevel + " " + okiD.NumberOfPages);
            */

            Console.ReadLine();
        }
        static private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(logging =>
            {
                //logging.ClearProviders();
                logging.AddNLog();
            });
        }
    }
}
