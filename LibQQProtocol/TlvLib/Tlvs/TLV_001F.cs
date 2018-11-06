using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.DeviceID)]
    internal class TLV001F : BaseTLV {
        public TLV001F() {
            this.Command = 0x001F;
            this.Name = "TLV_DeviceID";
            this.WSubVer = 0x0001;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            if (user.TXProtocol.BufDeviceId == null) {
                return new Byte[] { };
            }

            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0001) {
                data.BeWrite(this.WSubVer); //wSubVer
                data.Write(user.TXProtocol.BufDeviceId);
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }

            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            if (this.WSubVer == 0x0001) {
                this.WSubVer = buf.BeReadUInt16(); //wSubVer
                user.TXProtocol.BufDeviceId =
                    buf.ReadBytes(length - 2);
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}