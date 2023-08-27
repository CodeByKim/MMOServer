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

        public PktTestEchoResult() : base(PacketId.PktTestEchoResult)
        {
            echoMessage = "";
        }

        public PktTestEchoResult(PacketHeader header) : base(header, null)
        {
            echoMessage = "";
        }

        public override void Deserialize()
        {
        }

        public override void Serialize()
        {
        }
    }
}
