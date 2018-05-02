using System;
using System.Net;
using System.Net.Sockets;
using QQBot.Packages;

namespace QQBot.Network {
    public delegate bool TypeReceiveCallback(byte[] data);
    public delegate void TypeSendCallback(int sendSize);

    public class AsyncUdpClient {
        public TypeReceiveCallback ReceiveCallBack { set; private get; } = null;
        public TypeSendCallback SendCallBack { set; private get; } = null;


        private IPEndPoint RemoteEndPoint;
        private UdpClient Client;

        public AsyncUdpClient(IPAddress remoteIpAddress, int remotePort, int localPort) {
            this.RemoteEndPoint = new IPEndPoint(remoteIpAddress, remotePort);
            this.Client = new UdpClient(0);
            BeginReceiveData();
        }

        public void SendData(OutPackage package) {
            this.Client.BeginSend(package.PackageData, package.DataLength, this.RemoteEndPoint, new AsyncCallback(this.DefaultSendCallback), null);
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
                Byte[] receiveBytes = this.Client.EndReceive(asyncResult, ref this.RemoteEndPoint);
                if (this.ReceiveCallBack == null) {
                    this.ReceiveCallBack(receiveBytes);
                    BeginReceiveData();
                }
            }
        }
    }
}