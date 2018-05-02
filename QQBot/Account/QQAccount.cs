using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using QQBot.Network;
using QQBot.Util;

namespace QQBot.Account {
    [Serializable]
    public class QQAccount {
        public UInt32 QQNumber { get; private set; }
        public byte[] HexQQNumber { get; private set; }
        public byte[] Password { get; private set; }
        public byte[] Tlv0105 { get; private set; }
        public IPAddress Server { get; private set; }
        public byte[] HexServer { get; private set; }

        public AsyncUdpClient UDPClient { get; set; }

        public QQAccount(UInt32 QQNumber, byte[] Password, byte[] Tlv0105, IPAddress Server) {
            this.QQNumber = QQNumber;
            this.HexQQNumber = ByteHelper.GetHexQQnumber(QQNumber);
            this.Password = Password;
            this.Tlv0105 = Tlv0105;
            this.Server = Server;
            this.HexServer = Server.GetAddressBytes();
        }
    }
}
