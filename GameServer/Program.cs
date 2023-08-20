using System;
using System.Text;
using Core.Buffer;
using Packet;

public class Program
{
    public static void Main(string[] args)
    {
        var port = 8888;
        var server = new GameServer(port, new MMOGamePacketFactory());
        server.Run();

        Console.ReadLine();
        Console.WriteLine("end of program...");
    }
}