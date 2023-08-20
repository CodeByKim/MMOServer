using System;
using System.Collections.Generic;
using Core;
using Core.Connection;
using Core.Packet;
using Packet;

public class GameServer : Server
{
    public GameServer(int port, PacketFactory packetFactory)
        : base(port, packetFactory)
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

    public override void PushPacket(NetPacket packet)
    {
        var pkt = packet as PktTestEcho;
        if (pkt is null)
            return;

        Console.WriteLine(pkt.echoMessage);
    }
}