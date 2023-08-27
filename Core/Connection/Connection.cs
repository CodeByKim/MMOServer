using Core.Buffer;
using Core.Packet;
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

        public void Send(NetPacket packet)
        {
            packet.Serialize();

            var packetSize = packet._header.Length;
            var sendBuffer = new byte[packetSize];
            Array.Copy(packet.Buffer, 0, sendBuffer, 0, packetSize);

            //_socket.Send(packet.buffer);
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

        internal void OnReceive(RingBuffer receiveBuffer, int bytesTransferred)
        {
            while (receiveBuffer.UseSize > 0)
            {
                if (receiveBuffer.UseSize < PacketHeader.HeaderSize)
                    return;

                var headerBuffer = receiveBuffer.Peek(PacketHeader.HeaderSize);
                if (headerBuffer is null)
                {
                    _socket.CloseSocket();
                    return;
                }

                var packetHeader = new PacketHeader(headerBuffer);
                if ( receiveBuffer.UseSize < packetHeader.Length)
                    return;

                var packetBuffer = receiveBuffer.Dequeue(packetHeader.Length);
                if (packetBuffer is null)
                {
                    _socket.CloseSocket();
                    return;
                }

                OnExtractPacket(packetHeader, packetBuffer);
            }
        }

        internal void OnSend()
        {
        }

        protected abstract void OnExtractPacket(PacketHeader header, byte[] packetBuffer);

        protected abstract void OnClose();
    }
}