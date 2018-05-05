using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Network.Interface {
    public interface IPackage {
         byte[] PackageData { get; set; }
         int DataLength { get; }
    }
}