using NUnit.Framework;
using System.IO;
using web_page_parser;
using static data_access_layer.Enums;

namespace crawler_test
{
    [TestFixture]
    class PrinterDataParsingTests
    {
        // Toner, drum, total pages, printer type.
        [TestCase(20, 81, 59181, PrinterType.LexmarkMX421ade)]
        [TestCase(50, 0, 22227, PrinterType.OkiMB491)]
        public void ParsePrinterData_IsDataParsedCorrectly_True(
            int tonerLevel, int drumLevel, int numberOfPages,
            PrinterType printerType)
        {
            string filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, 
                $@"TestFiles\{printerType}.html");

            Parser parser = new Parser();

            var printerData = 
                parser.ParseRawDataFromFile(filePath, printerType).Result;

            Assert.AreEqual(tonerLevel, printerData.TonerLevel);
            Assert.AreEqual(drumLevel, printerData.DrumLevel);
            Assert.AreEqual(numberOfPages, printerData.NumberOfPages);
        }
    }
}