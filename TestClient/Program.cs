using Core.Connection;
using Packet;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TestClient
{
    private string _ip;
    private int _port;

    private Connector _connector;

    public TestClient(string ip, int port)
    {
        _ip = ip;
        _port = port;

        _connector = new Connector();
        _connector.OnDisconnectEventHandler = OnDisconnected;
    }

    public void Run()
    {
        _connector.ConnectAsync(_ip, _port, () =>
        {
            Console.WriteLine("Connect !");

            var packet = new PktEchoTest();
            packet.echoMessage = "한글 테스트";

            while (true)
            {
                _connector.Send(packet);

                Thread.Sleep(200);
            }
        });
    }

    public void OnDisconnected()
    {
        Console.WriteLine("Disconnected...");
    }
}

public class Program
{
    static void Main(string[] args)
    {
        var ip = "127.0.0.1";
        var port = 8888;

        var test = new TestClient(ip, port);
        test.Run();

        Console.ReadLine();
    }
}