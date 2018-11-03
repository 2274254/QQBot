using System;
using System.Collections.Generic;
using System.Text;

namespace LibAsyncUDP.Interfaces {
    public interface IPackage {
        Byte[] PackageData { get;  set; }
        Int32 DataLength { get; }
    }
}
