using LibQQProtocol.Defines;
using LibQQProtocol.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace LibQQProtocol.Structs {
    public class QQAccount {

        public QQAccount(UInt32 qqNum, String pwd) {
            this.QQ = qqNum;
            this.SetPassword(pwd);
            this.Initialize();
        }

        public Boolean IsLoginRedirect { get; set; }

        public Byte[] QQPacket0825Key { get; set; } = ByteHelper.RandomKey();

        public Byte[] QQPacketRedirectionkey { get; set; } = ByteHelper.RandomKey();

        public Byte[] QQPacket00BaKey { get; set; } = ByteHelper.RandomKey();

        public Byte[] QQPacketTgtgtKey { get; set; } = ByteHelper.RandomKey();

        public Byte[] QQPacket00BaFixKey { get; set; } ={
            0x69, 0x20, 0xD1, 0x14, 0x74, 0xF5, 0xB3,
            0x93, 0xE4, 0xD5, 0x02, 0xB3, 0x71, 0x1A, 0xCD, 0x2A
        };

        public Byte[] QQPacket0836Key1 { get; set; } = ByteHelper.RandomKey();

        public Byte[] QQPacket00BaVerifyCode { get; set; }
        public Byte QQPacket00BaSequence { get; set; } = 0x01;

        public Byte[] AddFriend0018Value { get; set; }

        public Byte[] AddFriend0020Value { get; set; }

        public Byte[] MD51 { get; set; }

        public UInt32 QQ { get; set; }

        public Boolean IsLoggedIn { get; set; }

        public LoginMode LoginMode { get; set; }

        public Boolean IsUdp { get; set; } = true;

        public String NickName { get; set; }

        public Byte Age { get; set; }

        public Byte Gender { get; set; }

        public String QQSkey { get; set; }
        public String QQGtk { get; set; }
        public String Bkn { get; set; }

        public String QunPSkey { get; set; }
        public String QunGtk { get; set; }

        public List<Char> ReceiveSequences { get; set; } = new List<Char>();

        public String Ukey { get; set; }

        private void Initialize() {
            this.IsLoggedIn = false;
            this.LoginMode = LoginMode.Normal;
            this.IsUdp = true;
        }

        public void SetPassword(String pwd) {
            this.MD51 = QQTea.MD5(ByteHelper.GetBytes(pwd));
        }

        public Byte[] Md52() {
            var byteBuffer = new BinaryWriter(new MemoryStream());
            byteBuffer.Write(this.MD51);
            byteBuffer.BeWrite(0);
            byteBuffer.BeWrite(this.QQ);
            return MD5.Create().ComputeHash(((MemoryStream)byteBuffer.BaseStream).ToArray());
        }

        public TXProtocol TXProtocol { get; set; } = new TXProtocol();
    }
}

