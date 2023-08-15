using Core;
using System;
using System.Collections.Generic;

class GameServer : Server
{
    public GameServer(int port) : base(port)
    {
        Console.WriteLine("initialize server...");
    }

    public override void OnJoinConnection(ServerConnection conn)
    {
        Console.WriteLine("join new connection : {0}", conn.Id);
    }

    public override void OnLeaveConnection(ServerConnection conn)
    {
        Console.WriteLine("leave connection : {0}", conn.Id);
    }
}