using System;
using System.Configuration;

namespace service_layer.Settings.PrinterObject
{
    [ConfigurationCollection(typeof(PrinterObjectElement), AddItemName = "printer", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class PrinterObjectElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PrinterObjectElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("Printer ip was null.");

            return ((PrinterObjectElement)element).Ip;
        }
    }
}