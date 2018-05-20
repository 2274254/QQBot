using ChatBotFramework.Network.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotFramework.Network.Abstract {
    abstract class APackage : IPackage {
        public Byte[] PackageData { get; set; }
        public Int32 DataLength => this.PackageData.Length;
    }
}
