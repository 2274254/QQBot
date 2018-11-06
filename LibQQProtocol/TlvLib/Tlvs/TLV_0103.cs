using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.SID)]
    internal class TLV0103 : BaseTLV {
        public TLV0103() {
            this.Command = 0x0103;
            this.Name = "SSO2::TLV_SID_0x103";
            this.WSubVer = 0x0001;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            if (user.TXProtocol.BufSid == null || user.TXProtocol.BufSid.Length == 0) {
                return null;
            }

            var data = new BinaryWriter(new MemoryStream());
            data.BeWrite(this.WSubVer);
            data.WriteKey(user.TXProtocol.BufSid);
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
                var len = buf.BeReadUInt16();
                user.TXProtocol.BufSid = buf.ReadBytes(len);
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}