using Core.Packet;
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

        protected override NetPacket? Create(PacketHeader header)
        {
            var packetId = header.PacketId;
            switch (packetId)
            {
                case 1: return new PktTestEcho(header);
            }

            return null;
        }
    }
}
