using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Packet;

namespace Packet
{
    public class PktTestEchoResult : NetPacket
    {
        public string echoMessage;

        public PktTestEchoResult() : base(2)
        {
            echoMessage = "";
        }

        public PktTestEchoResult(PacketHeader header) : base(header)
        {
            echoMessage = "";
        }

        public override void Deserialize(byte[] packetBuffer)
        {
        }

        public override void Serialize()
        {
        }
    }
}
