using System;
using Core.Connection;
using Core.ObjectPool;
using Core.Packet;
using Microsoft.Extensions.ObjectPool;

namespace Core
{
    public abstract class Server
    {
        private Acceptor _acceptor;
        private DefaultObjectPool<ServerConnection> _connectionPool;
        private PacketFactory _packetFactory;

        public int PortNumber { get; private set; }

        public Server(int port, PacketFactory packetFactory)
        {
            PortNumber = port;

            _acceptor = new Acceptor(this);
            _connectionPool = new DefaultObjectPool<ServerConnection>(new ConnectionPooledObjectPolicy(this), 100);
            _packetFactory = packetFactory;
        }

        public void Run()
        {
            var backlog = 100;
            var concuurentAcceptCount = 10;

            _acceptor.Run(PortNumber, backlog, concuurentAcceptCount);
        }

        public virtual void OnJoinConnection(ServerConnection conn)
        {
        }

        public virtual void OnLeaveConnection(ServerConnection conn)
        {
            _connectionPool.Return(conn);
        }

        public abstract void PushPacket(NetPacket packet);

        internal ServerConnection AcquireConnection()
        {
            return _connectionPool.Get();
        }

        internal NetPacket? CreatePacket(PacketHeader header, byte[] packetBuffer)
        {
            if (_packetFactory is null)
                return null;

            return _packetFactory.Create(header, packetBuffer);
        }
    }
}
