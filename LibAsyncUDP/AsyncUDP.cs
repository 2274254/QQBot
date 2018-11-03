using LibAsyncUDP.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;

namespace LibAsyncUDP {
    public delegate void TypeReceiveCallback(Byte[] Data);
    public delegate void TypeSendCallback(Int32 SendSize);
    public class AsyncUDPClient {
        public TypeReceiveCallback ReceiveCallBack { get; set; }
        public TypeSendCallback SendCallBack { get; set; }

        private IPEndPoint RemoteEndPoint;
        private readonly UdpClient Client;
        private Boolean IsClosed = false;

        public AsyncUDPClient(IPAddress remoteIpAddress, Int32 remotePort, Int32 localPort) {
            this.RemoteEndPoint = new IPEndPoint(remoteIpAddress, remotePort);
            this.Client = new UdpClient(localPort);
        }

        public void SendData(IPackage Package) {
            this.Client.BeginSend(Package.PackageData, Package.DataLength, this.RemoteEndPoint, new AsyncCallback(this.DefaultSendCallback), null);
        }

        public void DefaultSendCallback(IAsyncResult asyncResult) {
            if (asyncResult.IsCompleted) {
                var sendSize = this.Client.EndSend(asyncResult);
                this.SendCallBack?.Invoke(sendSize);
            }
        }

        public void BeginReceiveData() {
            if (!this.IsClosed) {
                this.Client.BeginReceive(new AsyncCallback(this.DefaultReceiveCallback), null);
            }
        }

        public void DefaultReceiveCallback(IAsyncResult asyncResult) {
            if (asyncResult.IsCompleted) {
                var ReceiveBytes = this.Client.EndReceive(asyncResult, ref this.RemoteEndPoint);
                if (this.ReceiveCallBack != null) {
                    this.ReceiveCallBack(ReceiveBytes);
                    this.BeginReceiveData();
                }
            }
        }

        public void Begin() {
            this.BeginReceiveData();
        }

        public void Stop() {
            this.IsClosed = true;

            try {
                this.Client.Close();
            } catch (Exception) {
            }

            try {
                this.Client.Dispose();
            } catch (Exception) {
            }
        }
    }
}
