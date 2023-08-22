using Newtonsoft.Json;

using PrintDataCrawler.Helpers;
using service_layer.DataClasses;
using service_layer.Settings;
using service_layer.Settings.PrinterObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace service_layer.Settings
{
    public class Configurator : IConfigurator
    {
        public Configurator()
        {
        }
        #region User Helpers Comps, Emails
        private Configuration LoadConfig()
        {
#if DEBUG
            string applicationName =
                Environment.GetCommandLineArgs()[0];
#else
                string applicationName =
                    Environment.GetCommandLineArgs()[0];
                    //Environment.GetCommandLineArgs()[0]+ ".exe";
#endif

            string exePath = System.IO.Path.Combine(Environment.CurrentDirectory, applicationName);

            Console.WriteLine("Opening conf -> " + exePath);

            // Get the configuration file. The file name has
            // this format appname.exe.config.
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);

            return config;
        }
        private Configuration LoadCustomConfig()
        {
            ExeConfigurationFileMap customConfigFileMap = new ExeConfigurationFileMap();
            customConfigFileMap.ExeConfigFilename = Path.Combine(AppContext.BaseDirectory, "Configs", "MainSettings.xml");

            Configuration customConfig = ConfigurationManager.OpenMappedExeConfiguration(customConfigFileMap, ConfigurationUserLevel.None);

            return customConfig;
        }
        // Read all headers for requests.
        private List<PrinterRequestHeader> LoadRequestHeaders()
        {
            return JsonConvert.DeserializeObject<List<PrinterRequestHeader>>(
                File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"Configs", "RequestHeaders.json")));
        }
        /// <summary>
        /// Load configuration from file then loads and assigns headers according to the type of printer.
        /// </summary>
        /// <returns><see cref="List{T}"/> of <see cref="Tuple{T1, T2}"/> where T1, T2 <see cref="Printer"/> and <see cref="PrinterRequestHeader"/></returns>
        public List<Tuple<Printer, PrinterRequestHeader>> LoadPrinterSettings()
        {
            List<Tuple<Printer, PrinterRequestHeader>> printerHeaderSet = new List<Tuple<Printer, PrinterRequestHeader>>();

            List<Printer> printers = new List<Printer>();
            List<PrinterRequestHeader> headers = LoadRequestHeaders();

            Configuration config = LoadCustomConfig();
            SettingsConfiguration myConfig = config.GetSection("mainSettings") as SettingsConfiguration;

            foreach (PrinterObjectElement printerSetting in myConfig.Printers)
            {
                Printer p = new Printer();
                p.Ip = printerSetting.Ip;
                p.Type = printerSetting.Type.ConvertToEnumTypeHelper();
                p.Login = printerSetting.Login;
                p.Password = printerSetting.Password;


                var h = headers.Single(prh => prh.PrinterType == p.Type);
                p.SavePartOfPath =  h.ContentExtension;
                p.SavePartOfPathForAdditionalUrl = h.ContentExtension;

                Dictionary<string, string> requestsDict = new Dictionary<string, string>();
                requestsDict = h.Headers.Where(e => e.Key != "Host" && e.Key != "Referer").ToDictionary(e => e.Key, e => e.Value);
                requestsDict.Add("Host", h.Headers["Host"].Replace("%IP%", p.Ip));
                requestsDict.Add("Referer", h.Headers["Referer"].Replace("%IP%", p.Ip));

                PrinterRequestHeader newPrh = new PrinterRequestHeader();
                newPrh.PrinterType = p.Type;
                newPrh.Url = h.Url.Replace("%IP%", p.Ip);
                newPrh.AdditionalUrl = h.AdditionalUrl != null ? h.AdditionalUrl.Replace("%IP%", p.Ip) : string.Empty;
                newPrh.Headers = requestsDict;

                printerHeaderSet.Add(new Tuple<Printer, PrinterRequestHeader>(p, newPrh));
            }

            return printerHeaderSet;
        }
        // Read task delay from config.
        public int LoadTaskDelay()
        {
            Configuration config = LoadCustomConfig();
            SettingsConfiguration myConfig = config.GetSection("mainSettings") as SettingsConfiguration;

            int taskDelay = int.TryParse(myConfig.TaskDelayInMinutes, out taskDelay) ? taskDelay : 120;

            return taskDelay;;
        }
        /// <summary>
        /// Load save directory path from configuration.
        /// </summary>
        /// <returns></returns>
        public string LoadSaveDirPath()
        {
            Configuration config = LoadCustomConfig();
            SettingsConfiguration myConfig = config.GetSection("mainSettings") as SettingsConfiguration;
#if DEBUG
            string dirPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
#else
            string dirPath = myConfig.SaveDirectory.Equals("%currentDirectory%") 
            ? Directory.GetParent(Environment.CurrentDirectory).Parent.FullName : myConfig.SaveDirectory;
#endif
            return dirPath;
        }
        #endregion
    }
    public interface IConfigurator
    {
        string LoadSaveDirPath();
        int LoadTaskDelay();
        public List<Tuple<Printer, PrinterRequestHeader>> LoadPrinterSettings();
    }
}