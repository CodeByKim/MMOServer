using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using Core.Packet;

namespace Core.Connection
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
            var socketError = args.SocketError;
            if (socketError != SocketError.Success)
            {
                Console.WriteLine(socketError);
                return;
            }

            IsConnected = true;
            var onConnect = args.UserToken as Action;
            if (onConnect != null)
                onConnect();
        }

        protected override void OnExtractPacket(PacketHeader header, byte[] packetBuffer)
        {
            //Console.WriteLine(packet);
        }

        protected override void OnClose()
        {
            IsConnected = false;

            if (OnDisconnectEventHandler != null)
                OnDisconnectEventHandler();
        }
    }
}
