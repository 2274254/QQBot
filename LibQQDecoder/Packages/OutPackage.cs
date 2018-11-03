using LibQQDecoder.Util;
using System;

namespace LibQQDecoder.Packages {
    public abstract class OutPackage : Package {
        public override Byte[] PackageData { get => this._PackageData; set => this._PackageData = value; }

        public override Int32 DataLength => this._PackageData.Length;

        public void AppendData(Byte[] Data) {
            this._PackageData = ByteHelper.MergeByteArray(this._PackageData, Data);
        }
    }
}
