using System;

namespace LibAsyncUDP.Interfaces {
    public interface IPackage {
        Byte[] PackageData { get; }
        Int64 DataLength { get; }
    }
}
