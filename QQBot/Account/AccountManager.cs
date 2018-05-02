using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using QQBot.Network;
using QQBot.Packages.Out;
using QQBot.Util;

namespace QQBot.Account {
    public class AccountManager {
        private static String[] RandServer = { String.Empty, "2", "3", "4", "5", "6", "7" };
        private static Random Rand = new Random();

        public static void Login(QQAccount Account) {
            var Package = new TouchPackage(Account);
            Account.UDPClient.SendData(Package);
        }

        public static QQAccount GetNewAccount(UInt32 QQNumber, byte[] Password) {
            var Tlv0105 = ByteHelper.MergeByteArray(new byte[] { 0x01, 0x05, 0x00, 0x30, 0x00, 0x01, 0x01, 0x02, 0x00, 0x14, 0x01, 0x01, 0x00, 0x10 }, ByteHelper.GetRandomBytes(16), new byte[] { 0x00, 0x14, 0x01, 0x02, 0x00, 0x10 }, ByteHelper.GetRandomBytes(16));
            var IPAddress = NetworkManager.GetRemote(String.Format("sz{0}.tencent.com", RandServer[Rand.Next(0, RandServer.Length - 1)]));

            var Account =  new QQAccount(QQNumber, Password, Tlv0105, IPAddress);
            NetworkManager.CreateUdpClientForAccount(Account);
            return Account;
        }
    }
}
