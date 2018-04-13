using System;
using System.Net;
using System.Net.Sockets;
using QQBot.Packages;

namespace QQBot.Network {
    public delegate bool TypeReceiveCallback(byte[] data);
    public delegate void TypeSendCallback(int sendSize);

    public class UdpState {
        public UdpClient Client;
        public IPEndPoint EndPoint;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[ BufferSize ];

        public UdpState(UdpClient client, IPEndPoint endPoint) {
            Client = client;
            EndPoint = endPoint;
        }
    }

    public class AsyncUdpClient {
        private UdpState receiveUdpState;
        private UdpState sendUdpState;
        public TypeReceiveCallback ReceiveCallBack { set; private get; } = null;
        public TypeSendCallback SendCallBack { set; private get; } = null;

        /// <summary>
        /// <param name="remoteIpAddress">远程服务器地址</param>
        /// <param name="remotePort">远程端口</param>
        /// <param name="localPort">本地端口</param>
        /// </summary>
        public AsyncUdpClient(IPAddress remoteIpAddress, int remotePort, int localPort) {
            receiveUdpState = new UdpState(new UdpClient(), new IPEndPoint(remoteIpAddress, remotePort));
            sendUdpState = new UdpState(new UdpClient(new IPEndPoint(IPAddress.Any, localPort)), new IPEndPoint(IPAddress.Any, localPort));

            sendUdpState.Client.Connect(sendUdpState.EndPoint);
            BeginReceiveData();
        }

        public void SendData(OutPackage package) {
            sendUdpState.Client.BeginSend(package.PackageData, package.DataLength, new AsyncCallback(DefaultSendCallback), sendUdpState);
        }

        public void DefaultSendCallback(IAsyncResult asyncResult) {
            UdpState state = asyncResult.AsyncState as UdpState;
            if (asyncResult.IsCompleted) {
                int sendSize = state.Client.EndSend(asyncResult);
                SendCallBack?.Invoke(sendSize);
            }
        }

        public void BeginReceiveData() {
            receiveUdpState.Client.BeginReceive(new AsyncCallback(DefaultReceiveCallback), receiveUdpState);
        }

        public void DefaultReceiveCallback(IAsyncResult asyncResult) {
            UdpState state = asyncResult.AsyncState as UdpState;
            if (asyncResult.IsCompleted) {
                Byte[] receiveBytes = state.Client.EndReceive(asyncResult, ref state.EndPoint);
                if (ReceiveCallBack == null || !ReceiveCallBack(receiveBytes)) {
                    BeginReceiveData();
                }
            }
        }
    }
}