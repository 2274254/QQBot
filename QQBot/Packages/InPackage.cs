using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.Packages {
    public class InPackage : Package {
        public byte[] PackageData { get { return this._PackageData; } }
        public int DataLength { get { return this._PackageData.Length; } }

        public InPackage(byte[] Data) {
            this._PackageData = Data;
        }
    }
}
