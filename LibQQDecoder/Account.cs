using LibAsyncUDP;
using LibQQDecoder.Util;
using System;
using System.Net;

namespace LibQQDecoder {
    [Serializable]
    public class Account {
        public UInt32 QQNumber { get; private set; }
        public Byte[] HexQQNumber { get; private set; }
        public Byte[] Password { get; private set; }
        public Byte[] Tlv0105 { get; private set; }
        public IPAddress Server { get; private set; }
        public Byte[] HexServer { get; private set; }

        public Byte[] HexLoginAddress { get; set; }
        public Byte[] HexLoginTime { get; set; }
        public Byte[] TouchToken { get; set; }

        public AsyncUDPClient AsyncUDP { get; private set; } = null;
        public TypeReceiveCallback ReceiveCallBack { get => this.AsyncUDP.ReceiveCallBack; set => this.AsyncUDP.ReceiveCallBack = value; }
        public TypeSendCallback SendCallBack { get => this.AsyncUDP.SendCallBack; set => this.AsyncUDP.SendCallBack = value; }

        public void ChangeServer(IPAddress NewServer) {
            this.Server = NewServer;
            this.HexServer = this.Server.GetAddressBytes();
            this.AsyncUDP.Stop();

            TypeReceiveCallback OldReceiveCallBack = this.AsyncUDP.ReceiveCallBack;
            TypeSendCallback OldSendCallBack = this.AsyncUDP.SendCallBack;

            this.AsyncUDP = new AsyncUDPClient(this.Server, 8000, 0) {
                ReceiveCallBack = OldReceiveCallBack,
                SendCallBack = OldSendCallBack
            };

            this.AsyncUDP.Begin();
        }

        public Account(UInt32 QQNumber, Byte[] Password, Byte[] Tlv0105, IPAddress Server) {
            this.QQNumber = QQNumber;
            this.HexQQNumber = ByteHelper.GetHexQQnumber(QQNumber);
            this.Password = Password;
            this.Tlv0105 = Tlv0105;
            this.Server = Server;
            this.HexServer = Server.GetAddressBytes();
            this.AsyncUDP = new AsyncUDPClient(Server, 8000, 0);
            this.AsyncUDP.Begin();
        }
    }
}
