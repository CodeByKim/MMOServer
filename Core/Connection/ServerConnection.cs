using System;
using System.Collections.Generic;

using Core.Packet;

namespace Core.Connection
{
    public class ServerConnection : Connection
    {
        private Server _server;

        public ServerConnection(Server server)
        {
            _server = server;
        }

        protected override void OnExtractPacket(PacketHeader header, byte[] packetBuffer)
        {
            var packet = _server.CreatePacket(header, packetBuffer);

            // 임시
            _server.PushPacket(packet);
        }

        protected override void OnClose()
        {
            _server.OnLeaveConnection(this);
        }
    }
}
