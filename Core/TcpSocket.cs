using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Core
{
    public class TcpSocket
    {
        private Socket _socket;
        private SocketAsyncEventArgs _recvArgs;
        private SocketAsyncEventArgs _sendArgs;
        private int _recvBufferSize;

        public TcpSocket()
            : this(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
        }

        public TcpSocket(Socket socket)
        {
            _socket = socket;

            _recvBufferSize = 1024;
            _recvArgs = new SocketAsyncEventArgs();
            _recvArgs.SetBuffer(GetRecvBuffer(_recvBufferSize), 0, _recvBufferSize);
            _recvArgs.Completed += OnReceiveCompleted;

            _sendArgs = new SocketAsyncEventArgs();
            _sendArgs.Completed += OnSendCompleted;
        }

        public void Send(string message)
        {
            var sendBuffer = Encoding.UTF8.GetBytes(message);
            _sendArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);

            var pending = _socket.SendAsync(_recvArgs);
            if (!pending)
                OnSendCompleted(null, _recvArgs);
        }

        internal void Listen(int port, int backlog)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
            _socket.Listen(backlog);
        }

        internal void AcceptAsnyc(int concurrentCount, Action<SocketError, Socket> onAccept)
        {
            for (int i = 0; i < concurrentCount; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += OnAcceotCompleted;
                args.UserToken = onAccept;

                TryAccept(args);
            }
        }

        internal void ReceiveAsync()
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

        private void TryAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;
            var pending = _socket.AcceptAsync(args);
            if (!pending)
                OnAcceotCompleted(null, args);
        }

        private void OnAcceotCompleted(object? sender, SocketAsyncEventArgs args)
        {
            var onAccept = args.UserToken as Action<SocketError, Socket>;
            if (onAccept is null)
                return;

            onAccept(args.SocketError, args.AcceptSocket);
            TryAccept(args);
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
            Console.WriteLine("hello client");

            Send(recvMessage);

            ReceiveAsync();
        }

        private void CloseSocket()
        {
            //_server.OnLeaveConnection(this);
            _socket.Close();
        }
    }
}
