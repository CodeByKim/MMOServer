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

        public PktEchoTest() : base(1)
        {
            echoMessage = "";
    }

        public override void Deserialize(byte[] packetBuffer)
        {
        }

        public override void Serialize()
        {
            var utf8String = Encoding.UTF8.GetBytes(echoMessage);
            var stringLength = BitConverter.GetBytes(utf8String.Length);

            var bufferIndex = (int)PacketHeader.HeaderSize;
            Array.Copy(stringLength, 0, buffer, bufferIndex, stringLength.Length);
            bufferIndex += stringLength.Length;

            Array.Copy(utf8String, 0, buffer, bufferIndex, utf8String.Length);
            bufferIndex += utf8String.Length;

            // 헤더 채우기
            _header.Length = (short)bufferIndex;
            SerializeHeader();
        }
    }
}
