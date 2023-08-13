using Core;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Program
{
    static void Main(string[] args)
    {
        var ip = IPAddress.Parse("127.0.0.1");
        var portNumber = 8888;

        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(ip, portNumber));

        var sendMessage = "hello server";
        var sendBuffer = Encoding.UTF8.GetBytes(sendMessage);
        var recvBuffer = new byte[1024];

        while (true)
        {
            socket.Send(sendBuffer);

            Array.Clear(recvBuffer);
            socket.Receive(recvBuffer);
            var recvMessage = Encoding.UTF8.GetString(recvBuffer);
            Console.WriteLine(recvMessage);

            Thread.Sleep(200);
        }
    }
}