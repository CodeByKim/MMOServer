using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Core
{
    public class Connector : Connection
    {
        public bool IsConnected { get; set; }

        public Action? OnDisconnectEventHandler { get; set; }

        public Connector()
        {
            IsConnected = false;
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Initialize(socket);
        }

        public void ConnectAsync(string ip, int port, Action onConnect)
        {
            var socket = _socket.Socket;
            if (socket is null)
                return;

            var args = new SocketAsyncEventArgs();
            args.UserToken = onConnect;
            args.Completed += OnConnectCompleted;

            var ipAddress = IPAddress.Parse(ip);
            args.RemoteEndPoint = new IPEndPoint(ipAddress, port);
            var pending = socket.ConnectAsync(args);
            if (!pending)
                OnConnectCompleted(null, args);
        }

        private void OnConnectCompleted(object? sender, SocketAsyncEventArgs args)
        {
            IsConnected = true;
            var onConnect = args.UserToken as Action;
            if (onConnect != null)
                onConnect();
        }

        protected override void OnPacketDispatch(string packet)
        {
            Console.WriteLine(packet);
        }

        protected override void OnClose()
        {
            IsConnected = false;

            if (OnDisconnectEventHandler != null)
                OnDisconnectEventHandler();
        }
    }
}
