using System;
using System.IO;
using static data_access_layer.Enums;

namespace web_page_parser.Data
{
    class Printer
    {
        private string ip;
        private PrinterType type;
        private string login;
        private string password;
        private string savePartOfPath;
        private string savePartOfPathForAdditionalUrl;

        public string Ip
        {
            get
            {
                return ip;
            }

            set
            {
                ip = value;
            }
        }
        internal PrinterType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }
        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string SavePartOfPath
        {
            get
            {
                return savePartOfPath;
            }
            set
            {
                savePartOfPath = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), type.ToString() + "_" + ip, type.ToString() + "_1" + "." + value);
            }
        }
        public string SavePartOfPathForAdditionalUrl
        {
            get
            {
                return savePartOfPathForAdditionalUrl;
            }
            set
            {
                savePartOfPathForAdditionalUrl = 
                    Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), type.ToString() + "_" + ip, type.ToString() + "_2" + "." + value);
            }
        }
    }
}