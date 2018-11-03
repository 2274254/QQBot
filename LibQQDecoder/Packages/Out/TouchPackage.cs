using LibQQDecoder.Util;
using System;

namespace LibQQDecoder.Packages.Out {
    public class TouchPackage : OutPackage {
        protected static Byte[] Data0825_0 = { 0x00, 0x18, 0x00, 0x16, 0x00, 0x01 };
        protected static Byte[] Data0825_2 = { 0x00, 0x00, 0x04, 0x53, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x15, 0x85 };

        private readonly Byte[] UnknowPrivateData0 = { 0x00, 0x00, 0x00, 0x00, 0x03, 0x09, 0x00, 0x08, 0x00, 0x01 };
        private readonly Byte[] UnknowPrivateData1 = { 0x00, 0x02, 0x00, 0x36, 0x00, 0x12, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x14, 0x00, 0x1D, 0x01, 0x02, 0x00, 0x19 };

        public TouchPackage(Account account) {
            var DataToEncrypt = ByteHelper.MergeByteArray(Data0825_0, Data0825_2, account.HexQQNumber, this.UnknowPrivateData0, account.HexServer, this.UnknowPrivateData1, Package.PublicKey);

            this.AppendData(Package.Head);
            this.AppendData(Package.Version);
            this.AppendData(Commands.Touch);
            this.AppendData(account.HexQQNumber);
            this.AppendData(Package.UnknowData01);
            this.AppendData(Keys.Touch);
            this.AppendData(QQTea.Encrypt(DataToEncrypt, 0, DataToEncrypt.Length, Keys.Touch));
            this.AppendData(Package.Tail);
        }
    }
}
