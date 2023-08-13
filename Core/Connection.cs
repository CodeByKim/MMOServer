using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;

namespace Core
{
    public class Connection
    {
        public string Id { get; private set; }

        private Socket _socket;
        private SocketAsyncEventArgs _recvArgs;
        private SocketAsyncEventArgs _sendArgs;
        private int _recvBufferSize;

        public Connection()
            : this(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
        }

        public Connection(Socket socket)
        {
            Id = Guid.NewGuid().ToString();

            _socket = socket;

            _recvBufferSize = 1024;
            _recvArgs = new SocketAsyncEventArgs();
            _recvArgs.SetBuffer(GetRecvBuffer(_recvBufferSize), 0, _recvBufferSize);
            _recvArgs.Completed += OnReceiveCompleted;

            _sendArgs = new SocketAsyncEventArgs();
            _sendArgs.Completed += OnSendCompleted;
        }

        public void StartReceive()
        {
            _recvArgs.SetBuffer(GetRecvBuffer(_recvBufferSize), 0, _recvBufferSize);

            var pending = _socket.ReceiveAsync(_recvArgs);
            if (!pending)
                OnReceiveCompleted(null, _recvArgs);
        }

        private byte[] GetRecvBuffer(int size)
        {
            return new byte[size];
        }

        private void OnSendCompleted(object? sender, SocketAsyncEventArgs args)
        {
            
        }

        private void OnReceiveCompleted(object? sender, SocketAsyncEventArgs args)
        {
            var bytesTransferred = args.BytesTransferred;
            var socketError = args.SocketError;
            if (socketError != SocketError.Success || bytesTransferred == 0)
            {
                CloseSocket();
                return;
            }

            var recvBuffer = args.Buffer;
            if (recvBuffer is null)
                return;

            var recvMessage = Encoding.UTF8.GetString(recvBuffer);
            Console.WriteLine(recvMessage);

            StartReceive();
        }

        private void CloseSocket()
        {
            _socket.Close();
        }
    }
}