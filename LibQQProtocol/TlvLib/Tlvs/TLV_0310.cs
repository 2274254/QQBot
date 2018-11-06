using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.ServerAddress)]
    internal class TLV0310 : BaseTLV {
        public TLV0310() {
            this.Command = 0x0310;
            this.Name = "SSO2::TLV_ServerAddress_0x310";
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            user.TXProtocol.DwServerIP = ByteHelper.GetIpStringFromBytes(buf.ReadBytes(4));
        }
    }
}