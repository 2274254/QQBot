using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.PacketMd5)]
    internal class TLV0115 : BaseTLV {
        public TLV0115() {
            this.Command = 0x0115;
            this.Name = "SSO2::TLV_PacketMd5_0x115";
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            var bufPacketMD5 = buf.ReadBytes(length);
        }
    }
}