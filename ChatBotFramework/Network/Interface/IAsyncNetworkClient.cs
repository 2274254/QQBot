namespace ChatBotFramework.Network.Interface {
    public delegate void TypeReceiveCallback(byte[] Data);
    public delegate void TypeSendCallback(int SendSize);

    public interface IAsyncNetworkClient {
        TypeReceiveCallback ReceiveCallBack { set; get; }
        TypeSendCallback SendCallBack { set; get; }

        void SendData(IPackage Package);
        void Begin();
        void Stop();
    }
}
