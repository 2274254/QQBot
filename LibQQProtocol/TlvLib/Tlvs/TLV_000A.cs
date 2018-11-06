using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;
using System.Text;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.ErrorInfo)]
    internal class TLV000A : BaseTLV {
        public TLV000A() {
            this.Command = 0x000A;
            this.Name = "SSO2::TLV_ErrorCode_0x000A";
        }

        public String ErrorMsg { get; private set; }

        public void Parser_Tlv_0A(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            this.WSubVer = buf.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0001) {
                var wCsCmd = buf.BeReadUInt16();
                this.ErrorMsg = Encoding.UTF8.GetString(buf.ReadBytes(wCsCmd));
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}