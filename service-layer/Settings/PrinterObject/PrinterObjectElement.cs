using System.Configuration;

namespace service_layer.Settings.PrinterObject
{
    class PrinterObjectElement : ConfigurationElement
    {
        #region Configuration Properties
        [ConfigurationProperty("ip", IsRequired = true)]
        public string Ip
        {
            get
            {
                return (string)this["ip"];
            }
            set
            {
                this["ip"] = value;
            }
        }
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }
        [ConfigurationProperty("login", IsRequired = false)]
        public string Login
        {
            get
            {
                return (string)this["login"];
            }
            set
            {
                this["login"] = value;
            }
        }
        [ConfigurationProperty("password", IsRequired = false)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
    }
}