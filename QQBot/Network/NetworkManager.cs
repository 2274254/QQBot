using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using QQBot.Account;
using QQBot.Packages;

namespace QQBot.Network {
    public class NetworkManager {
        public static void CreateUdpClientForAccount(QQAccount Account) {
            Account.UDPClient =  new AsyncUdpClient(Account.Server, 8000, 0);
        }

        public static IPAddress GetRemote(String ServerName) {
            var Addresses = Dns.GetHostAddresses(ServerName);
            if (Addresses.Length > 0) {
                return Addresses[0];
            } else {
                throw new Exception("Wrong!!!!!");
            }
        }
    }
}
