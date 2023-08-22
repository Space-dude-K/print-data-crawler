using System;
using static data_access_layer.Enums;

namespace PrintDataCrawler.Helpers
{
    public static class Extensions
    {
        public static PrinterType ConvertToEnumTypeHelper(this string typeString)
        {
            PrinterType typeValue = PrinterType.DefaultType;

            try
            {
                typeValue = (PrinterType)Enum.Parse(typeof(PrinterType), typeString, true);

                if (Enum.IsDefined(typeof(PrinterType), typeValue) | typeValue.ToString().Contains(","))
                {
                    //Console.WriteLine("Converted '{0}' to {1}.", typeString, typeValue.ToString());
                }
                else
                {
                    Console.WriteLine("{0} is not an underlying value of the Colors enumeration.", typeString);
                } 
            }
            catch (ArgumentException)
            {
                Console.WriteLine("{0} is not a member of the Colors enumeration.", typeString);
            }

            return typeValue;
        }
    }
}