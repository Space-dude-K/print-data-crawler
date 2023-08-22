
using service_layer.Settings.PrinterObject;
using System.Configuration;

namespace service_layer.Settings
{
    public class SettingsConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("printers", IsDefaultCollection = false)]
        public PrinterObjectElementCollection Printers
        {
            get
            {
                return ((PrinterObjectElementCollection)(base["printers"]));
            }
        }
        [ConfigurationProperty("saveDirectory", IsDefaultCollection = false)]
        public string SaveDirectory
        {
            get
            {
                return (string)this["saveDirectory"];
            }
            set
            {
                this["saveDirectory"] = value;
            }
        }
        [ConfigurationProperty("taskDelayInMinutes", IsDefaultCollection = false)]
        public string TaskDelayInMinutes
        {
            get
            {
                return (string)this["taskDelayInMinutes"];
            }
            set
            {
                this["taskDelayInMinutes"] = value;
            }
        }
    }
}