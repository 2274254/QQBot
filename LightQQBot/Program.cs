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
                        //...
                    }
                }

                if (ByteHelper.ArrCmp(Data, Commands.Redirect, 3, 0, Commands.Redirect.Length)) {
                    var package = new RedirectTouchReplyPackage(Data);
                    if (package.NeedRedirect) {
                        this.account.ChangeServer(package.RedirectTargetAddress);
                        this.account.AsyncUDP.SendData(new RedirectTouchPackage(this.account));
                    } else {
                        //...
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
            (new QQBot()).start();

            //var encode_test = System.Text.Encoding.UTF8.GetBytes("encode_test");
            while (true) { }
        }
    }
}
