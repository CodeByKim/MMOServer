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

        public PktTestEcho() : base(1)
        {
            echoMessage = "";
        }

        public PktTestEcho(PacketHeader header) : base(header)
        {
            echoMessage = "";
        }

        public override void Deserialize(byte[] packetBuffer)
        {
            // 여기서 디시리얼라이즈를 잘못했네
            var startIndex = PacketHeader.HeaderSize;
            var stringLength = BitConverter.ToInt32(packetBuffer, startIndex);
            startIndex += sizeof(int);

            echoMessage = Encoding.UTF8.GetString(packetBuffer, startIndex, stringLength);
        }

        public override void Serialize()
        {
        }
    }
}
