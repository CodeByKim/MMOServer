using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Core.Connection
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

        public void Run(int port, int backlog, int concurrentCount = 1)
        {
            Listen(port, backlog);

            AcceptAsnyc(10, (socketError, socket) =>
            {
                if (socketError != SocketError.Success)
                    return;

                var conn = _server.AcquireConnection();
                conn.Initialize(socket);
                _server.OnJoinConnection(conn);

                conn.DoReceive();
            });
        }

        private void Listen(int port, int backlog)
        {
            if (_socket is null)
                return;

            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
            _socket.Listen(backlog);
        }

        private void AcceptAsnyc(int concurrentCount, Action<SocketError, Socket> onAccept)
        {
            for (int i = 0; i < concurrentCount; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += OnAcceotCompleted;
                args.UserToken = onAccept;

                TryAccept(args);
            }
        }

        private void OnAcceotCompleted(object? sender, SocketAsyncEventArgs args)
        {
            var onAccept = args.UserToken as Action<SocketError, Socket>;
            if (onAccept is null)
                return;

            if (args.AcceptSocket is null)
                return;

            onAccept(args.SocketError, args.AcceptSocket);
            TryAccept(args);
        }

        private void TryAccept(SocketAsyncEventArgs args)
        {
            if (_socket is null)
                return;

            args.AcceptSocket = null;
            var pending = _socket.AcceptAsync(args);
            if (!pending)
                OnAcceotCompleted(null, args);
        }
    }
}
