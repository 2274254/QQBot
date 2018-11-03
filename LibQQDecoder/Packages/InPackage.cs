using System;

namespace LibQQDecoder.Packages {
    public abstract class InPackage : Package {
        public override Byte[] PackageData { get => this._PackageData; set => this._PackageData = value; }
        public override Int32 DataLength => this._PackageData.Length;

        public InPackage(Byte[] Data) {
            this._PackageData = Data;
        }
    }
}
