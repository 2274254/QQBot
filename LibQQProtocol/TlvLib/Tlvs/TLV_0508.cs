using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags._0x0508)]
    internal class TLV0508 : BaseTLV {
        public TLV0508() {
            this.Command = 0x0508;
            this.Name = "SSO2::TLV_0508";
        }

        public System.Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            data.Write((System.Byte)1);
            data.BeWrite(0);


            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            buf.ReadBytes(length);
        }
    }
}