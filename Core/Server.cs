using System;
using Core.Connection;
using Core.ObjectPool;
using Microsoft.Extensions.ObjectPool;

namespace Core
{
    public class Server
    {
        private DefaultObjectPool<ServerConnection> _connectionPool;
        protected Acceptor _acceptor;

        public int PortNumber { get; private set; }

        public Server(int port)
        {
            _acceptor = new Acceptor(this);
            _connectionPool = new DefaultObjectPool<ServerConnection>(new ConnectionPooledObjectPolicy(this), 100);

            PortNumber = port;
        }

        public void Run()
        {
            var backlog = 100;
            var concuurentAcceptCount = 10;

            _acceptor.Run(PortNumber, backlog, concuurentAcceptCount);
        }

        internal ServerConnection AcquireConnection()
        {
            return _connectionPool.Get();
        }

        public virtual void OnJoinConnection(ServerConnection conn)
        {
        }

        public virtual void OnLeaveConnection(ServerConnection conn)
        {
            _connectionPool.Return(conn);
        }
    }
}
