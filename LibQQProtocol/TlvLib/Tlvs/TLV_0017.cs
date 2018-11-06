using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.ClientInfo)]
    internal class TLV0017 : BaseTLV {
        public TLV0017() {
            this.Command = 0x0017;
            this.Name = "SSO2::TLV_ClientInfo_0x17";
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            this.WSubVer = buf.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0001) {
                Int64 timeMillis = buf.BeReadUInt32();
                user.TXProtocol.DwServerTime = ByteHelper.GetDateTimeFromMillis(timeMillis);
                user.TXProtocol.TimeDifference = (UInt32.MaxValue & timeMillis) - ByteHelper.CurrentTimeMillis() / 1000;
                user.TXProtocol.DwClientIP = ByteHelper.GetIpStringFromBytes(buf.ReadBytes(4));
                user.TXProtocol.WClientPort = buf.BeReadUInt16();
                buf.BeReadUInt16(); //UNKNOW
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}