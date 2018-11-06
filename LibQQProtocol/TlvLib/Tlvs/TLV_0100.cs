using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;
using System.Text;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.ErrorCode)]
    internal class TLV0100 : BaseTLV {
        public TLV0100() {
            this.Command = 0x0100;
            this.Name = "SSO2::TLV_ErrorCode_0x100";
        }

        public String ErrorMsg { get; private set; }
        public Char PacketCommand { get; private set; }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            this.Parser_Tlv2(user, buf, length);
        }

        public void Parser_Tlv2(QQAccount user, BinaryReader buf, Int32 length) {
            this.WSubVer = buf.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0001) {
                this.PacketCommand = (Char)buf.BeReadUInt16();
                var errorCode = buf.BeReadUInt32();
                this.ErrorMsg = Encoding.UTF8.GetString(buf.ReadBytes(buf.BeReadUInt16()));
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}