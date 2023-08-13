using System;
using System.Collections.Generic;

namespace Core
{
    public abstract class Server
    {
        protected Acceptor _acceptor;

        public int PortNumber { get; private set; }

        public Server(int port)
        {
            PortNumber = port;
            _acceptor = new Acceptor(this);
        }

        public void Run()
        {
            var backlog = 100;
            var acceptCount = 10;
            _acceptor.Run(PortNumber, backlog, acceptCount);
        }

        public abstract void OnJoinConnection(Connection conn);
        public abstract void OnLeaveConnection(Connection conn);
    }
}
