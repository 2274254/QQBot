using LibAsyncUDP;
using LibQQProtocol.Packets.Out.Login;
using LibQQProtocol.Structs;
using System;
using System.Net;

namespace ChatBot {
    internal class Program {

        private class QQBot {
            public void OnDataSend(Int32 DataSize) {

            }

            public void OnDataRecv(Byte[] Data) {

            }

            public void Start() {
                var Account = new QQAccount(123456789, "somepassword");

                var asyncUdpClient = new AsyncUDPClient(IPAddress.Parse(Account.TXProtocol.DwServerIP), Account.TXProtocol.WServerPort, Account.TXProtocol.WClientPort) {
                    ReceiveCallBack = this.OnDataRecv,
                    SendCallBack = this.OnDataSend
                };

                asyncUdpClient.BeginReceiveData();

                asyncUdpClient.SendData(new ServerTouchPacket(Account, false));
            }
        }

        private static void Main(String[] args) {
            (new QQBot()).Start();
            while (true) { }
        }
    }
}
