using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packet
{
    public abstract class PacketFactory
    {
        public PacketFactory()
        {
        }

        public NetPacket? CreatePacket(PacketHeader header, byte[] packetBuffer)
        {
            var id = header.PacketId;

            var packet = Create(header, packetBuffer);
            if (packet is null)
                return null;

            packet.Deserialize();
            return packet;
        }

        protected abstract NetPacket? Create(PacketHeader header, byte[] packetBuffer);
    }
}
