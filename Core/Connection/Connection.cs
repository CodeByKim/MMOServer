using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Core.Connection
{
    public abstract class Connection
    {
        public string Id { get; private set; }

        protected TcpSocket _socket;

        public Connection()
        {
            Id = Guid.NewGuid().ToString();

            _socket = new TcpSocket();
        }

        public void Send(string message)
        {
            var sendBuffer = Encoding.UTF8.GetBytes(message);
            _socket.Send(sendBuffer);
        }

        internal void Initialize(Socket socket)
        {
            _socket.Socket = socket;

            _socket.OnReceiveEventHandler = OnReceive;
            _socket.OnSendEventHandler = OnSend;
            _socket.OnCloseEventHandler = OnClose;
        }

        internal void Release()
        {
            _socket.Socket = null;
        }

        internal void DoReceive()
        {
            if (_socket is null)
                return;

            _socket.ReceiveAsync();
        }

        internal void OnReceive(byte[] recvBuffer)
        {
            // 패킷으로 만든다...
            var packet = Encoding.UTF8.GetString(recvBuffer);

            // 패킷 처리를 위임한다.
            OnPacketDispatch(packet);
        }

        internal void OnSend()
        {
        }

        protected abstract void OnPacketDispatch(string packet);

        protected abstract void OnClose();
    }
}