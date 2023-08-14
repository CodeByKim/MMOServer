using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Core
{
    public class Connection
    {
        public string Id { get; private set; }

        private TcpSocket _socket;

        public Connection()
        {
            Id = Guid.NewGuid().ToString();

            _socket = new TcpSocket();
        }

        public Connection(Socket socket)
        {
            Id = Guid.NewGuid().ToString();

            _socket = new TcpSocket(socket);
        }

        internal void DoReceive()
        {
            _socket.ReceiveAsync();
        }
    }
}