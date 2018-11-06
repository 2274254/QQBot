using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.Misc_Flag)]
    internal class TLV0312 : BaseTLV {
        public TLV0312() {
            this.Command = 0x0312;
            this.Name = "SSO2::TLV_Misc_Flag_0x312";
        }

        public System.Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            data.Write((System.Byte)1);
            data.BeWrite(1);
            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }
    }
}