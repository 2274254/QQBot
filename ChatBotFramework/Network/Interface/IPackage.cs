using System;

namespace ChatBotFramework.Network.Interface {
    public interface IPackage {
        Byte[] PackageData { get; set; }
        Int32 DataLength { get; }
    }
}