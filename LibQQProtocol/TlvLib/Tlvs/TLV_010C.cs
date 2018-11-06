using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags._0x010C)]
    internal class TLV010C : BaseTLV {
        public TLV010C() {
            this.Command = 0x010C;
            this.Name = "TLV_010C";
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            this.WSubVer = buf.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0001) {
                user.TXProtocol.SessionKey = buf.ReadBytes(16);
                var dwUin = buf.BeReadInt32();
                var dwClientIP = ByteHelper.GetIpStringFromBytes(buf.ReadBytes(4)); //IP地址
                user.TXProtocol.WClientPort = buf.BeReadUInt16();
                DateTime dwServerTime = ByteHelper.GetDateTimeFromMillis(buf.BeReadInt32());
                var unknow = buf.BeReadInt32();
                var cPassSeqId = buf.ReadByte();
                var dwReLoginConnIP = buf.ReadBytes(4);
                var dwReLoginCtrlFlag = buf.BeReadInt32();

                Int32 len = buf.BeReadUInt16();
                var bufComputerIdSig = buf.ReadBytes(len);

                len = buf.ReadByte();
                var unknow2 = buf.ReadBytes(len);

                len = buf.BeReadUInt16();
                var unknow3 = buf.ReadBytes(len);
                var a = new BinaryReader(new MemoryStream(unknow3));
                a.ReadByte();
                var dwConnIP2 = a.ReadBytes(4);
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}