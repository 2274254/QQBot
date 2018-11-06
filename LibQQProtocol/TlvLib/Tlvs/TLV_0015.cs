using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.ComputerGuid)]
    internal class TLV0015 : BaseTLV {
        public TLV0015() {
            this.Command = 0x0015;
            this.Name = "SSO2::TLV_ComputerGuid_0x15";
            this.WSubVer = 0x0001;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0001) {
                data.BeWrite(this.WSubVer); //wSubVer

                data.Write((Byte)0x01);
                var thisKey = user.TXProtocol.BufComputerId;
                data.BeWrite(CRC32.CRC32Reverse(thisKey));
                data.WriteKey(thisKey);

                data.Write((Byte)0x02);
                thisKey = user.TXProtocol.BufComputerIdEx;
                data.BeWrite(CRC32.CRC32Reverse(thisKey));
                data.WriteKey(thisKey);
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