using LibLogger;
using LibQQDecoder;
using LibQQDecoder.Packages;
using LibQQDecoder.Packages.In;
using LibQQDecoder.Packages.Out;
using LibQQDecoder.Util;
using System;
using System.Net;

namespace LightQQBot {
    internal class Program {
        private class QQBot {
            private Account account;

            public static IPAddress GetRemote(String ServerName) {
                IPAddress[] Addresses = Dns.GetHostAddresses(ServerName);
                if (Addresses.Length > 0) {
                    return Addresses[0];
                } else {
                    throw new Exception("Wrong!!!!!");
                }
            }

            public static Account GetNewAccount(UInt32 QQNumber, Byte[] Password) {
                String[] RandServer = { String.Empty, "2", "3", "4", "5", "6", "7" };
                var Rand = new Random();

                var Tlv0105 = ByteHelper.MergeByteArray(new Byte[] { 0x01, 0x05, 0x00, 0x30, 0x00, 0x01, 0x01, 0x02, 0x00, 0x14, 0x01, 0x01, 0x00, 0x10 }, ByteHelper.GetRandomBytes(16), new Byte[] { 0x00, 0x14, 0x01, 0x02, 0x00, 0x10 }, ByteHelper.GetRandomBytes(16));
                IPAddress IPAddress = GetRemote(String.Format("sz{0}.tencent.com", RandServer[Rand.Next(0, RandServer.Length - 1)]));

                var Account = new Account(QQNumber, Password, Tlv0105, IPAddress);

                return Account;
            }

            public void OnDataSend(Int32 DataSize) {

            }

            public void OnDataRecv(Byte[] Data) {

                if (ByteHelper.ArrCmp(Data, Commands.Touch, 3, 0, Commands.Touch.Length)) {
                    var package = new TouchReplyPackage(Data);
                    if (package.NeedRedirect) {
                        this.account.ChangeServer(package.RedirectTargetAddress);
                        this.account.AsyncUDP.SendData(new RedirectTouchPackage(this.account));
                    } else {
                        this.account.TouchToken = package.TouchToken;
                        this.account.HexLoginTime = package.HexLoginTime;
                        this.account.HexLoginAddress = package.HexLoginAddress;
                    }
                }

                if (ByteHelper.ArrCmp(Data, Commands.Redirect, 3, 0, Commands.Redirect.Length)) {
                    var package = new RedirectTouchReplyPackage(Data);
                    if (package.NeedRedirect) {
                        this.account.ChangeServer(package.RedirectTargetAddress);
                        this.account.AsyncUDP.SendData(new RedirectTouchPackage(this.account));
                    } else {
                        this.account.TouchToken = package.TouchToken;
                        this.account.HexLoginTime = package.HexLoginTime;
                        this.account.HexLoginAddress = package.HexLoginAddress;
                    }
                }
            }

            public void start() {
                this.account = GetNewAccount(1111111111, System.Text.Encoding.Default.GetBytes("111111111"));
                this.account.ReceiveCallBack = this.OnDataRecv;
                this.account.SendCallBack = this.OnDataSend;
                this.account.AsyncUDP.SendData(new TouchPackage(this.account));
            }
        }

        private static void Main(String[] args) {
            //(new QQBot()).start();
            Byte[] test_arr = { 0x02, 0x37, 0x13, 0x08, 0x25, 0x31, 0x02, 0xDE, 0xBD, 0x01, 0xC7, 0x00, 0x00, 0x00, 0xCD, 0x8E, 0x7B, 0x0F, 0x6D, 0x11, 0xBB, 0x92, 0x00, 0x52, 0x62, 0xCC, 0x2C, 0x69, 0x94, 0x28, 0xD1, 0x06, 0xF2, 0x7F, 0xE5, 0xDB, 0x81, 0x53, 0xBC, 0x0B, 0x39, 0x2C, 0xDF, 0x56, 0xE3, 0xB6, 0x5E, 0x3E, 0x78, 0x33, 0x8F, 0x65, 0xA9, 0x1A, 0x2C, 0x89, 0x65, 0xB8, 0x41, 0x70, 0xD2, 0x9D, 0xFE, 0xC8, 0x89, 0x3A, 0x65, 0x85, 0x57, 0xC8, 0x8C, 0x01, 0xA8, 0x56, 0xAD, 0xA1, 0x6D, 0xD8, 0xDB, 0x8A, 0x23, 0x8B, 0x0D, 0x8E, 0xB8, 0x03, 0xB7, 0x89, 0xA8, 0xAD, 0xD3, 0x17, 0xEE, 0xA5, 0x3B, 0xB9, 0x97, 0x6C, 0x7C, 0x92, 0x28, 0xD2, 0x62, 0xB2, 0x45, 0xE3, 0xBD, 0xD3, 0x4F, 0x67, 0xD2, 0x20, 0xCE, 0x3D, 0xE1, 0x94, 0xD1, 0x76, 0x03 };
            var bot = new QQBot();

            bot.OnDataRecv(test_arr);

            //var encode_test = System.Text.Encoding.UTF8.GetBytes("encode_test");
            while (true) { }
        }
    }
}
