namespace ChatBotFramework.Network.Interface {
    public interface IAsyncPackageDispatcher {
        void DispatchPackage(byte[] Data);
    }
}
