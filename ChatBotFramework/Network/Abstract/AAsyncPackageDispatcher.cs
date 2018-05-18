using ChatBotFramework.Network.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotFramework.Network.Abstract {
    abstract class AAsyncPackageDispatcher : IAsyncPackageDispatcher {
        public void DispatchPackage(Byte[] Data) {
            throw new NotImplementedException();
        }
    }
}
