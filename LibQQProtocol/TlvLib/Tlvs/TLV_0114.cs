using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.DHParams)]
    internal class TLV0114 : BaseTLV {
        public TLV0114() {
            this.Command = 0x0114;
            this.Name = "SSO2::TLV_DHParams_0x114";
            this.WSubVer = 0x0102;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0102) {
                data.BeWrite(this.WSubVer); //wDHVer
                data.BeWrite((UInt16)user.TXProtocol.BufDhPublicKey.Length); //bufDHPublicKey长度
                data.Write(user.TXProtocol.BufDhPublicKey);
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }

            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }
    }
}