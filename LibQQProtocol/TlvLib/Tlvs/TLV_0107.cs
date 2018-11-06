using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.TicketInfo)]
    internal class TLV0107 : BaseTLV {
        public TLV0107() {
            this.Command = 0x0107;
            this.Name = "SSO2::TLV_TicketInfo_0x107";
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            this.WSubVer = buf.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0001) {
                var len = buf.BeReadUInt16();
                var buffer = buf.ReadBytes(len);
                var bufTickStatus = new BinaryReader(new MemoryStream(buffer));
                var dwTgtServiceId = bufTickStatus.BeReadInt32();
                var dwTgtPriority = bufTickStatus.BeReadInt32();
                var dwTgtRefreshInterval = bufTickStatus.BeReadInt32();
                var dwTgtValidInterval = bufTickStatus.BeReadInt32();
                var dwTgtTryInterval = bufTickStatus.BeReadInt32();
                var wTgtTryCount = bufTickStatus.BeReadUInt16();

                buffer = buf.ReadBytes(16);
                user.TXProtocol.BufTgtGtKey = buffer;

                len = buf.BeReadUInt16();
                buffer = buf.ReadBytes(len);
                user.TXProtocol.BufTgt = buffer;

                buffer = buf.ReadBytes(16);
                user.TXProtocol.Buf16BytesGtKeySt = buffer;

                len = buf.BeReadUInt16();
                buffer = buf.ReadBytes(len);
                user.TXProtocol.BufServiceTicket = buffer;

                len = buf.BeReadUInt16();
                buffer = buf.ReadBytes(len);
                var bufStHttp = new BinaryReader(new MemoryStream(buffer));
                var bAllowPtlogin = bufStHttp.ReadByte();
                buffer = bufStHttp.ReadBytes(16);
                user.TXProtocol.Buf16BytesGtKeyStHttp = buffer;

                len = bufStHttp.BeReadUInt16();
                buffer = bufStHttp.ReadBytes(len);
                user.TXProtocol.BufServiceTicketHttp = buffer;

                buffer = buf.ReadBytes(16);
                user.TXProtocol.BufGtKeyTgtPwd = buffer;
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}