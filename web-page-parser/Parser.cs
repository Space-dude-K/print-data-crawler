using data_access_layer;
using HtmlAgilityPack;
using NLog;
using System;
using System.Threading.Tasks;
using web_page_parser.Data;

namespace web_page_parser
{
    public class Parser
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public Task<ParsedPrinterData> ParseRawDataFromFile(string filePath, Enums.PrinterType printerType)
        {
            ParsedPrinterData ppd = new ParsedPrinterData();

            // TODO - C# 9 or istead of case stacking (maybe?).
            switch (printerType)
            {
                case Enums.PrinterType.DefaultType:
                    break;
                case Enums.PrinterType.LexmarkMX421ade:
                case Enums.PrinterType.LexmarkMS421dn:
                case Enums.PrinterType.LexmarkMB2236adw:
                    ppd = ParseLexmarkPrinterData(filePath);
                    break;
                case Enums.PrinterType.XeroxWorkCentre3325:
                    break;
                case Enums.PrinterType.XeroxPhaser4620:
                    break;
                case Enums.PrinterType.HpLaserJet4250:
                    break;
                case Enums.PrinterType.HpLaserJetP3005:
                    break;
                case Enums.PrinterType.KyoceraEcosysP5021cdn:
                    break;
                case Enums.PrinterType.CanonLBP6670:
                    break;
                case Enums.PrinterType.OkiMB491:
                    ppd = ParseOki491PrinterData(filePath);
                    break;
                default:
                    break;
            }

            return Task.FromResult(ppd);
        }
        public ParsedPrinterData ParseLexmarkPrinterData(string filePath)
        {
            HtmlDocument doc = null;
            ParsedPrinterData ppd = null;

            try
            {
                doc = new HtmlWeb().Load(filePath);

                var rawNumberOfPages =
                        doc.DocumentNode
                               .SelectSingleNode("//table[tr/td/b/font[.='Media&nbsp;Printed&nbsp;Side&nbsp;Count']]/following-sibling::table[12]/tr/td[2]")
                               .InnerText;
                var rawTonerStatus =
                        doc.DocumentNode
                               .SelectSingleNode("//table[tr/td/b/font[.='Black&nbsp;Cartridge']]/following-sibling::table[4]/tr/td[2]")
                               .InnerText.Replace("%", "");
                var rawDrumStatus =
                        doc.DocumentNode
                               .SelectSingleNode("//table[tr/td/b/font[.='Imaging&nbsp;Unit']]/following-sibling::table[2]/tr/td[2]")
                               .InnerText.Replace("%", "");

                ppd = new ParsedPrinterData();

                int.TryParse(rawNumberOfPages, out int parsedNumberOfPages);
                ppd.NumberOfPages = parsedNumberOfPages;

                int.TryParse(rawTonerStatus, out int parsedTonerStatus);
                ppd.TonerLevel = parsedTonerStatus;

                int.TryParse(rawDrumStatus, out int parsedDrumStatus);
                ppd.DrumLevel = parsedDrumStatus;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (doc != null)
                    doc = null;
            }

            //Console.WriteLine(ppd.NumberOfPages + " " + ppd.TonerLevel + " " + ppd.DrumLevel);
            return ppd;
        }
        public ParsedPrinterData ParseOki491PrinterData(string filePath)
        {
            HtmlDocument doc = null;
            ParsedPrinterData ppd = null;

            try
            {
                doc = new HtmlWeb().Load(filePath);

                var rawNumberOfPages =
                    doc.DocumentNode
                           .SelectSingleNode("//table[tr/td/nobr[.='Total Impressions:']]/tr[10]/td[2]").InnerText;
                var rawTonerStatus =
                        doc.DocumentNode
                               .SelectSingleNode("//input[@name='AVAILABELBLACKTONER']").Attributes["value"].Value;
                var rawDrumStatus =
                        doc.DocumentNode
                               .SelectSingleNode("//tr[td[.='Drum Unit :']]/td[2]").InnerText.Replace("%", "");

                ppd = new ParsedPrinterData();

                int.TryParse(rawNumberOfPages, out int parsedNumberOfPages);
                ppd.NumberOfPages = parsedNumberOfPages;

                int.TryParse(rawTonerStatus, out int parsedTonerStatus);
                ppd.TonerLevel = parsedTonerStatus;

                int.TryParse(rawDrumStatus, out int parsedDrumStatus);
                ppd.DrumLevel = parsedDrumStatus;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (doc != null)
                    doc = null;
            }

            //Console.WriteLine(ppd.NumberOfPages + " " + ppd.TonerLevel + " " + ppd.DrumLevel);
            return ppd;
        }
    }
}
