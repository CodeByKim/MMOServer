using Core.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet
{
    public class PktEchoTest : NetPacket
    {
        public string echoMessage;

        public PktEchoTest() : base(PacketId.PktTestEcho)
        {
            echoMessage = "";
        }

        public override void Deserialize()
        {
        }

        public override void Serialize()
        {
            SetString(echoMessage);

            SerializeHeader();
        }
    }
}
