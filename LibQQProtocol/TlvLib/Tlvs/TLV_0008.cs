using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.TimeZone)]
    internal class TLV0008 : BaseTLV {
        public TLV0008() {
            this.Command = 0x0008;
            this.Name = "SSO2::TLV_TimeZone_0x8";
            this.WSubVer = 0x0001;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            //if (PCQQGlobal.dwLocaleID == 0x00000804 && PCQQGlobal.wTimeZoneoffsetMin == 0x01E0)
            //{
            //    return null;
            //}
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0001) {
                data.BeWrite(this.WSubVer); //wSubVer 
                data.BeWrite(0x00000804); //此乃LCID
                data.BeWrite(0x01E0); //此乃时区信息
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