using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Core.Connection
{
    public class TcpSocket
    {
        private SocketAsyncEventArgs _recvArgs;
        private SocketAsyncEventArgs _sendArgs;
        private int _recvBufferSize;

        internal Action? OnCloseEventHandler { get; set; }
        internal Action<byte[]>? OnReceiveEventHandler { get; set; }
        internal Action? OnSendEventHandler { get; set; }

        internal Socket? Socket { get; set; }

        public TcpSocket()
        {
            _recvBufferSize = 1024;
            _recvArgs = new SocketAsyncEventArgs();
            _recvArgs.SetBuffer(GetRecvBuffer(_recvBufferSize), 0, _recvBufferSize);
            _recvArgs.Completed += OnReceiveCompleted;

            _sendArgs = new SocketAsyncEventArgs();
            _sendArgs.Completed += OnSendCompleted;
        }

        public void Send(byte[] sendBuffer)
        {
            if (Socket is null)
                return;

            _sendArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);

            var pending = Socket.SendAsync(_sendArgs);
            if (!pending)
                OnSendCompleted(null, _sendArgs);
        }

        internal void ReceiveAsync()
        {
            if (Socket is null)
                return;

            _recvArgs.SetBuffer(GetRecvBuffer(_recvBufferSize), 0, _recvBufferSize);

            var pending = Socket.ReceiveAsync(_recvArgs);
            if (!pending)
                OnReceiveCompleted(null, _recvArgs);
        }

        private byte[] GetRecvBuffer(int size)
        {
            return new byte[size];
        }

        private void OnSendCompleted(object? sender, SocketAsyncEventArgs args)
        {
            if (OnSendEventHandler != null)
                OnSendEventHandler();
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

            if (OnReceiveEventHandler != null)
                OnReceiveEventHandler(recvBuffer);

            ReceiveAsync();
        }

        private void CloseSocket()
        {
            if (Socket is null)
                return;

            Socket.Close();

            if (OnCloseEventHandler != null)
                OnCloseEventHandler();
        }
    }
}
