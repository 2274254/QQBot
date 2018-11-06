using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.Uin)]
    internal class TLV0005 : BaseTLV {
        public TLV0005() {
            this.Command = 0x0005;
            this.Name = "SSO2::TLV_Uin_0x5";
            this.WSubVer = 0x0002;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            if (user.QQ == 0) {
                return null;
            }

            var buf = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0002) {
                buf.BeWrite(this.WSubVer);
                buf.BeWrite((UInt32)user.QQ);
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }

            this.FillHead(this.Command);
            this.FillBody(buf.ToByteArray(), buf.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }
    }
}