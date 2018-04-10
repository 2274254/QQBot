using QQBot.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.Packages {
    class Package {
        protected static byte[] head = { 0x02 };
        protected static byte[] version = { 0x37, 0x13 };

        public byte[] PackageData { get; private set; } = new byte[] { };

        void PutData(byte[] data) {
            PackageData = ByteHelper.MergeByteArray(PackageData, data);
        }
    }
}
