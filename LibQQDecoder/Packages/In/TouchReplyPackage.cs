using LibLogger;
using LibQQDecoder.Packages.Exceptions;
using LibQQDecoder.Util;
using System;
using System.Net;

namespace LibQQDecoder.Packages.In {
    public class TouchReplyPackage : InPackage {
        public IPAddress RedirectTargetAddress { get; private set; }
        public Byte[] HexLoginAddress { get; private set; }
        public Byte[] HexLoginTime { get; private set; }
        public Byte[] TouchToken { get; private set; }

        public Boolean NeedRedirect { get; private set; }

        public TouchReplyPackage(Byte[] Data) : base(Data) {
            var DecodedData = QQTea.Decrypt(Data, 14, Data.Length - 15, Keys.Touch);

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
                this.TouchToken = new Byte[56];
                Array.ConstrainedCopy(DecodedData, 5, this.TouchToken, 0, 56);
                this.HexLoginTime = new Byte[4];
                Array.ConstrainedCopy(DecodedData, 67, this.HexLoginTime, 0, 4);
                this.HexLoginAddress = new Byte[4];
                Array.ConstrainedCopy(DecodedData, 71, this.HexLoginAddress, 0, 4);
            } else {
                var exception = new BadPackageException("unable to recognition magic " + DecodedData[0]);
                Logger.Exception(exception);
                throw exception;
            }
        }
    }
}
