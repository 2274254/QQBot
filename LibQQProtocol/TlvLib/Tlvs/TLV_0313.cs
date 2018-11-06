using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.GUID_Ex)]
    internal class TLV0313 : BaseTLV {
        public TLV0313() {
            this.Command = 0x0313;
            this.Name = "SSO2::TLV_GUID_Ex_0x313";
            this.WSubVer = 0x01;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x01) {
                data.Write((Byte)1);
                data.Write((Byte)1);
                data.Write((Byte)2);
                data.WriteKey(user.TXProtocol.BufMacGuid);
                data.BeWrite(2);
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