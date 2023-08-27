using Core.Packet;
using Packet;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace Packet
{
    public class MMOGamePacketFactory : PacketFactory
    {
        public MMOGamePacketFactory()
        {
        }

        protected override NetPacket? Create(PacketHeader header, byte[] packetBuffer)
        {
            var packetId = header.PacketId;
            switch (packetId)
            {
                case PacketId.PktTestEcho: return new PktTestEcho(header, packetBuffer);
            }

            return null;
        }
    }
}
