using data_access_layer.Context;
using data_access_layer.Model.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using service_layer.Settings;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using web_page_parser;
using Model = data_access_layer.Model.Db;

namespace crawler_base
{
    public class ConsoleApp
    {
        private readonly IConfigurator config;
        private readonly ILogger<ConsoleApp> logger;
        private readonly PrinterContext printerDbContext;

        public ConsoleApp(IConfigurator config, ILogger<ConsoleApp> logger, PrinterContext printerContext)
        {
            this.logger = logger;
            this.printerDbContext = printerContext;
            this.config = config;

            this.logger.LogInformation("Hi from ConsoleApp!");

            //printerDbContext.Cities.Add(city);
            //printerDbContext.SaveChanges();

            // Make sure if test data was correctly saved.
            //var cityDb = printerDbContext.Cities.Find(1);
            //var org = cityDb.Organizations.Where(e => e.Id == 1).FirstOrDefault();
            //var room = org.Rooms.Where(e => e.Id == 1).FirstOrDefault();

            //Console.WriteLine("Context data: " + room.Name);
            
        }

        private void TestLogger()
        {
            this.logger.LogCritical("Critical log.");
            this.logger.LogDebug("Debug log.");
            this.logger.LogError("Error log.");
            this.logger.LogInformation("Information log.");
            this.logger.LogTrace("Trace log.");
            this.logger.LogWarning("Warning log.");
            this.logger.Log(LogLevel.None, "Non level log");
        }
        public void Run()
        {
            //var test = config.YourItem;

            //logger.LogCritical(test);
            foreach(var printer in config.LoadPrinterSettings())
            {
                Console.WriteLine(printer.Item1.Ip + " " + printer.Item2.PrinterType);
            }


            TestLogger();

            System.Console.ReadKey();
        }
        
        public void Run1()
        {
            string saveDir = config.LoadSaveDirPath();
            int taskDelay = config.LoadTaskDelay();
            string feedPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\", @"crawler-test\TestFiles"));

            DbWritter dw = new DbWritter(config, logger, printerDbContext);

            Console.WriteLine("Save dir: " + saveDir);
            Console.WriteLine("Task delay: " + taskDelay);
            Console.WriteLine("Test feed: " + feedPath);

            Parser parser = new Parser();
            
            foreach(var file in Directory.GetFiles(feedPath))
            {
                if(Path.GetFileNameWithoutExtension(file).Contains("OkiMB491"))
                {
                    Console.WriteLine("OkiMB491 was found! => " + file);

                    var parsedOki = parser.ParseRawDataFromFile(file, data_access_layer.Enums.PrinterType.OkiMB491).Result;

                    Console.WriteLine(parsedOki.TonerLevel + " " + parsedOki.DrumLevel + " " + parsedOki.NumberOfPages);

                    dw.WriteToDb("City1", "Org1", "Room1", parsedOki.TonerLevel, parsedOki.DrumLevel, parsedOki.NumberOfPages,
                        data_access_layer.Enums.PrinterType.OkiMB491.ToString(), "historyRoom1");
                }
                else if (Path.GetFileNameWithoutExtension(file).Contains("Lexmark"))
                {
                    Console.WriteLine("Lexmark was found! => " + file);

                    var parsedLexmark = parser.ParseRawDataFromFile(file, data_access_layer.Enums.PrinterType.LexmarkMX421ade).Result;

                    Console.WriteLine(parsedLexmark.TonerLevel + " " + parsedLexmark.DrumLevel + " " + parsedLexmark.NumberOfPages);

                    dw.WriteToDb("City1", "Org1", "Room1", parsedLexmark.TonerLevel, parsedLexmark.DrumLevel, parsedLexmark.NumberOfPages,
                        data_access_layer.Enums.PrinterType.LexmarkMX421ade.ToString(), "historyRoom1");
                }
                else
                {
                    Console.WriteLine("Unknown printer type! => " + file);
                }
            }
        }
    }
}