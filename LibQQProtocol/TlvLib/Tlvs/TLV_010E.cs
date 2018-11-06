using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags._0x010E)]
    internal class TLV010E : BaseTLV {
        public TLV010E() {
            this.Command = 0x010E;
            this.Name = "TLV_010E";
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            this.WSubVer = buf.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0001) {
                Int32 len = buf.BeReadUInt16();
                var buffer = buf.ReadBytes(len);
                var sig = new BinaryReader(new MemoryStream(buffer));
                var dwUinLevel = sig.BeReadInt32();
                var dwUinLevelEx = sig.BeReadInt32();

                len = sig.BeReadUInt16();
                buffer = sig.ReadBytes(len);
                var buf24ByteSignature = buffer;

                len = sig.BeReadUInt16();
                buffer = sig.ReadBytes(len);
                var buf32ByteValueAddedSignature = buffer;

                len = sig.BeReadUInt16();
                buffer = sig.ReadBytes(len);
                var buf12ByteUserBitmap = buffer;

                user.TXProtocol.ClientKey = buf32ByteValueAddedSignature;
                //client.QQUser.ClientKeyString = Util.ToHex(buf32byteValueAddedSignature).Replace(" ", "");
                //client.GetCookie();
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}