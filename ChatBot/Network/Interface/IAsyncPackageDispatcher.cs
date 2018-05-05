using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Network.Interface {
    public interface IAsyncPackageDispatcher {
        void DispatchPackage(byte[] Data);
    }
}
