using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags._0x002F)]
    internal class TLV002F : BaseTLV {
        public TLV002F() {
            this.Command = 0x002F;
            this.Name = "TLV_Control";
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            this.WSubVer = buf.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0001) {
                var bufControl = buf.ReadBytes(length - 2);
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}