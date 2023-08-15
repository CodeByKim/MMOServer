using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Core
{
    public class Acceptor
    {
        private TcpSocket _socket;
        private Server _server;

        public Acceptor(Server server)
        {
            _socket = new TcpSocket();
            _socket.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server = server;
        }

        public void Run(int port, int backlog, int concurrentCount = 1)
        {
            _socket.Listen(port, backlog);

            _socket.AcceptAsnyc(10, (socketError, socket) =>
            {
                if (socketError != SocketError.Success)
                    return;

                if (socket is null)
                    return;

                var conn = _server.AcquireConnection();
                conn.Initialize(socket);
                _server.OnJoinConnection(conn);

                conn.DoReceive();
            });
        }
    }
}
