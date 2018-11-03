using LibQQDecoder.Util;
using System;
using System.Net;

namespace LibQQDecoder.Packages.In {
    public class RedirectTouchReplyPackage : InPackage {
        public IPAddress RedirectTargetAddress { get; private set; }

        public Boolean NeedRedirect { get; private set; }

        public RedirectTouchReplyPackage(Byte[] Data) : base(Data) {
            var DecodedData = QQTea.Decrypt(Data, 14, Data.Length - 15, Keys.Redirect);

            if (DecodedData[0] == 0xFE) {
                this.NeedRedirect = true;
                var IpAddress = new Byte[4];
                IpAddress[0] = DecodedData[95];
                IpAddress[1] = DecodedData[96];
                IpAddress[2] = DecodedData[97];
                IpAddress[3] = DecodedData[98];

                this.RedirectTargetAddress = new IPAddress(IpAddress);
            } else if (DecodedData[0] == 0x00) {
                this.NeedRedirect = false;
            } else {
                this.NeedRedirect = false;
            }

        }
    }
}
