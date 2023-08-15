using System;
using System.Text;
using Core.Buffer;

public class Program
{
    public static void RingBufferTest()
    {
        var testData = Encoding.UTF8.GetBytes("Hello World");
        RingBuffer buffer = new RingBuffer(12);

        while (true)
        {
            var result = buffer.Enqueue(testData);
            if (!result)
            {
                Console.WriteLine("fail enqueue");
                break;
            }

            var data = buffer.Dequeue(testData.Length);
            if (data is null)
            {
                Console.WriteLine("fail dequeue");
                break;
            }

            Console.WriteLine(Encoding.UTF8.GetString(data));

            Thread.Sleep(50);
        }
    }

    public static void Main(string[] args)
    {
        var port = 8888;
        var server = new GameServer(port);
        server.Run();

        Console.ReadLine();
        Console.WriteLine("end of program...");
    }
}