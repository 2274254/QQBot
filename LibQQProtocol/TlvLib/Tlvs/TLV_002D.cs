using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;
using System.Net;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.LocalIP)]
    internal class TLV002D : BaseTLV {
        public TLV002D() {
            this.Command = 0x002D;
            this.Name = "TLV_LocalIP";
            this.WSubVer = 0x0001;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0001) {
                data.BeWrite(this.WSubVer); //wSubVer 
                data.Write(ByteHelper.IPStringToByteArray(GetLocalIP())); //本机内网IP地址
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }

            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }

        public static String GetLocalIP() {
            var localIP = "192.168.1.2";
            try {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList) {
                    if (ip.AddressFamily.ToString() == "InterNetwork") {
                        localIP = ip.ToString();
                        break;
                    }
                }
            } catch (Exception) {
                localIP = "192.168.1.2";
            }

            return localIP;
        }
    }
}