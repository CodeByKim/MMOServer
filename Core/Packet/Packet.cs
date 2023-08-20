using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packet
{
    public struct PacketHeader
    {
        public static readonly short HeaderSize = 4;

        public short Length { get; set; }
        public short PacketId { get; set; }

        public PacketHeader(short packetId)
        {
            PacketId = packetId;
        }

        public PacketHeader(byte[] buffer)
        {
            Length = BitConverter.ToInt16(buffer, 0);
            PacketId = BitConverter.ToInt16(buffer, sizeof(short));
        }
    }

    public abstract class NetPacket
    {
        public PacketHeader _header;
        public byte[] buffer;   // 임시

        public NetPacket(short packetId)
        {
            _header = new PacketHeader(packetId);
            buffer = new byte[1024];
        }

        public NetPacket(PacketHeader header)
        {
            _header = header;
        }

        protected void SerializeHeader()
        {
            var length = BitConverter.GetBytes(_header.Length);
            var packetId = BitConverter.GetBytes(_header.PacketId);

            Array.Copy(length, 0, buffer, 0, sizeof(short));
            Array.Copy(packetId, 0, buffer, sizeof(short), sizeof(short));
        }

        public abstract void Serialize();
        public abstract void Deserialize(byte[] packetBuffer);
    }
}
