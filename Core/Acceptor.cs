using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Core
{
    public class Acceptor
    {
        private Socket _socket;
        private Server _server;

        public Acceptor(Server server)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server = server;
        }

        public void Run(int port, int backlog, int acceptCount)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
            _socket.Listen(backlog);

            for (int i = 0; i < acceptCount; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += OnAcceotCompleted;

                TryAccept(args);
            }
        }

        private void TryAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;
            var pending = _socket.AcceptAsync(args);
            if (!pending)
                OnAcceotCompleted(null, args);
        }

        private void OnAcceotCompleted(object? sender, SocketAsyncEventArgs args)
        {
            var socketError = args.SocketError;
            if (socketError != SocketError.Success)
                return;

            var socket = args.AcceptSocket;
            if (socket is null)
                return;

            Connection conn = new Connection(socket);
            _server.OnJoinConnection(conn);
            conn.StartReceive();

            TryAccept(args);
        }
    }
}
