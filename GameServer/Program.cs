using System;

public class Program
{
    public static void Main(string[] args)
    {
        var port = 8888;
        GameServer server = new GameServer(port);
        server.Run();

        Console.ReadLine();
        Console.WriteLine("end of program...");
    }
}