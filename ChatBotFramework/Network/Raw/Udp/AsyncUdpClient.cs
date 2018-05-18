using ChatBotFramework.Network.Interface;
using System;
using System.Net;
using System.Net.Sockets;

namespace ChatBotFramework.Network.Raw.Udp {
    public class AsyncUdpClient : IAsyncNetworkClient {
        public TypeReceiveCallback ReceiveCallBack { get; set; }
        public TypeSendCallback SendCallBack { get; set; }

        private IPEndPoint RemoteEndPoint;
        private UdpClient Client;

        public AsyncUdpClient(IPAddress remoteIpAddress, int remotePort, int localPort) {
            this.RemoteEndPoint = new IPEndPoint(remoteIpAddress, remotePort);
            this.Client = new UdpClient(localPort);
        }

        public void SendData(IPackage Package) {
            this.Client.BeginSend(Package.PackageData, Package.DataLength, this.RemoteEndPoint, new AsyncCallback(this.DefaultSendCallback), null);
        }

        public void DefaultSendCallback(IAsyncResult asyncResult) {
            if (asyncResult.IsCompleted) {
                int sendSize = this.Client.EndSend(asyncResult);
                this.SendCallBack?.Invoke(sendSize);
            }
        }

        public void BeginReceiveData() {
            this.Client.BeginReceive(new AsyncCallback(this.DefaultReceiveCallback), null);
        }

        public void DefaultReceiveCallback(IAsyncResult asyncResult) {
            if (asyncResult.IsCompleted) {
                Byte[] ReceiveBytes = this.Client.EndReceive(asyncResult, ref this.RemoteEndPoint);
                if (this.ReceiveCallBack != null) {
                    this.ReceiveCallBack(ReceiveBytes);
                    BeginReceiveData();
                }
            }
        }

        public void Begin() {
            BeginReceiveData();
        }

        public void Stop() {
            this.Client.Close();
        }
    }
}
