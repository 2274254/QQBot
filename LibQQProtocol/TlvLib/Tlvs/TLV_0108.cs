using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;
using System.Text;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.AccountBasicInfo)]
    internal class TLV0108 : BaseTLV {
        public TLV0108() {
            this.Command = 0x0108;
            this.Name = "SSO2::TLV_AccountBasicInfo_0x108";
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            this.WSubVer = buf.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0001) {
                var len = buf.BeReadUInt16();
                var buffer = buf.ReadBytes(len);
                var bufAccountBasicInfo = new BinaryReader(new MemoryStream(buffer));

                len = bufAccountBasicInfo.BeReadUInt16();
                buffer = bufAccountBasicInfo.ReadBytes(len);
                var info = new BinaryReader(new MemoryStream(buffer));
                var wSsoAccountWFaceIndex = info.BeReadUInt16();
                len = info.ReadByte();
                if (len > 0) {
                    user.NickName = Encoding.UTF8.GetString(info.ReadBytes(len));
                }

                user.Gender = info.ReadByte();
                var dwSsoAccountDwUinFlag = info.BeReadUInt32();
                user.Age = info.ReadByte();

                var bufStOther =
                    bufAccountBasicInfo.ReadBytes(
                        (Int32)(bufAccountBasicInfo.BaseStream.Length - bufAccountBasicInfo.BaseStream.Position));
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}