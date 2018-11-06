using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    internal class TLV0105 : BaseTLV {
        [TlvTag(TlvTags.m_vec0x12c)]
        public TLV0105() {
            this.Command = 0x0105;
            this.Name = "TLV_m_vec0x12c";
            this.WSubVer = 0x0001;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0001) {
                data.BeWrite(this.WSubVer); //wSubVer
                data.Write(user.TXProtocol.XxooB);
                data.Write((Byte)2);
                data.BeUshortWrite(0x0014);
                data.BeWrite(0x01010010);
                data.Write(ByteHelper.RandomKey());
                data.BeUshortWrite(0x0014);
                data.BeWrite(0x01020010);
                data.Write(ByteHelper.RandomKey());
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }

            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            this.WSubVer = buf.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0001) {
                buf.ReadByte(); //UNKNOWs
                var count = buf.ReadByte();
                for (var i = 0; i < count; i++) {
                    buf.ReadBytes(0x38); //buf
                }
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}