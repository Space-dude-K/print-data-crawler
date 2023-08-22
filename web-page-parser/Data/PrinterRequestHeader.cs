using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static data_access_layer.Enums;

namespace web_page_parser.Data
{
    class PrinterRequestHeader
    {
        private PrinterType printerType;
        private string url;
        private string additionalUrl;
        private string contentExtension;
        private bool postAuthentication = false;
        private Dictionary<string, string> headers;

        [JsonConverter(typeof(StringEnumConverter))]
        public PrinterType PrinterType
        {
            get
            {
                return printerType;
            }

            set
            {
                printerType = value;
            }
        }
        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                url = value;
            }
        }
        public string AdditionalUrl
        {
            get
            {
                return additionalUrl;
            }

            set
            {
                additionalUrl = value;
            }
        }
        public string ContentExtension
        {
            get
            {
                return contentExtension;
            }

            set
            {
                contentExtension = value;
            }
        }
        public bool PostAuthentication
        {
            get
            {
                return postAuthentication;
            }

            set
            {
                postAuthentication = value;
            }
        }
        public Dictionary<string, string> Headers
        {
            get
            {
                return headers;
            }

            set
            {
                headers = value;
            }
        }
        public PrinterRequestHeader()
        {
        }
    }
}