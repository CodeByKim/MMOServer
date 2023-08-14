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
            _server = server;
        }

        public void Run(int port, int backlog, int acceptCount)
        {
            _socket.Listen(port, backlog);

            _socket.AcceptAsnyc(10, (socketError, socket) =>
            {
                if (socketError != SocketError.Success)
                    return;

                if (socket is null)
                    return;

                Connection conn = new Connection(socket);
                _server.OnJoinConnection(conn);
                conn.DoReceive();
            });
        }
    }
}
