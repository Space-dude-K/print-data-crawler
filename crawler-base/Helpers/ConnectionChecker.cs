using System;
using System.Net.NetworkInformation;

namespace PrintDataCrawler.Helpers
{
    public static class ConnectionChecker
    {
        public static bool CheckConnection(string host)
        {
            try
            {
                Ping myPing = new Ping();
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}