using System;
using System.Collections.Generic;
using System.Text;
using QQBot.Util;

namespace QQBot.Packages {
    public class OutPackage : Package {
        public byte[] PackageData { get { return this._PackageData; } }
        public int DataLength { get { return this._PackageData.Length; } }

        public void AppendData(byte[] Data) {
            this._PackageData = ByteHelper.MergeByteArray(_PackageData, Data);
        }
    }
}
