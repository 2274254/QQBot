using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags._0x0404)]
    internal class TLV0404 : BaseTLV {
        public TLV0404() {
            this.Command = 0x0404;
            this.Name = "SSO2::TLV_0404";
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            buf.ReadBytes(length);
        }
    }
}