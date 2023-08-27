using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Packet;

namespace Packet
{
    public class PktTestEcho : NetPacket
    {
        public string echoMessage;

        public PktTestEcho(short packetId)
            : base(packetId)
        {
            echoMessage = "";
        }

        public PktTestEcho(PacketHeader header, byte[] packetBuffer)
            : base(header, packetBuffer)
        {
            echoMessage = "";
        }

        public override void Deserialize()
        {
            echoMessage = GetString();
        }

        public override void Serialize()
        {
        }
    }
}
