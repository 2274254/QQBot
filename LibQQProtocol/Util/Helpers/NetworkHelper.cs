using System;
using System.Net;

namespace LibQQProtocol.Util {
    public static class NetworkHelper {
        public static IPAddress GetRemote(String ServerName) {
            IPAddress[] Addresses = Dns.GetHostAddresses(ServerName);
            if (Addresses.Length > 0) {
                return Addresses[0];
            } else {
                throw new Exception("Error On Get Remote IP");
            }
        }

        public static IPAddress GetRandomServer() {
            String[] RandServer = { String.Empty, "2", "3", "4", "5", "6", "7" };
            var Rand = new Random();

            return GetRemote(String.Format("sz{0}.tencent.com", RandServer[Rand.Next(0, RandServer.Length - 1)]));
        }

        public static String GetRandomServerAsString() {
            return GetRandomServer().ToString();
        }
    }
}
