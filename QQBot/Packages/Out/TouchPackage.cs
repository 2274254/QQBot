using System;
using System.Collections.Generic;
using System.Text;
using QQBot.Account;
using QQBot.Util;

namespace QQBot.Packages.Out {
    public class TouchPackage : OutPackage {
        private byte[] PackageSign0825 = { 0x08, 0x25, 0x31, 0x01 };
        private byte[] UnknowPrivateData0 = { 0x00, 0x00, 0x00, 0x00, 0x03, 0x09, 0x00, 0x08, 0x00, 0x01 };
        private byte[] UnknowPrivateData1 = { 0x00, 0x02, 0x00, 0x36, 0x00, 0x12, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x14, 0x00, 0x1D, 0x01, 0x02, 0x00, 0x19 };

        public TouchPackage(QQAccount account) {
            var DataToEncrypt = ByteHelper.MergeByteArray(Package.Data0825_0, Package.Data0825_2, account.HexQQNumber, UnknowPrivateData0, account.HexServer, UnknowPrivateData1, Package.PublicKey);

            this.AppendData(Package.Head);
            this.AppendData(Package.Version);
            this.AppendData(this.PackageSign0825);
            this.AppendData(account.HexQQNumber);
            this.AppendData(Package.UnknowData01);
            this.AppendData(Package.Key0825);
            this.AppendData(QQTea.Encrypt(DataToEncrypt, 0, DataToEncrypt.Length, Package.Key0825));
            this.AppendData(Package.Tail);
        }
    }
}
