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
            Length = HeaderSize;
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
        public byte[] Buffer => _buffer;

        public PacketHeader _header;
        protected byte[] _buffer;
        protected int _bufferIndex;

        public NetPacket(short packetId)
        {
            _header = new PacketHeader(packetId);
            _buffer = new byte[1024];       // 임시
            _bufferIndex = PacketHeader.HeaderSize;
        }

        public NetPacket(PacketHeader header, byte[] packetBuffer)
        {
            _header = header;
            _buffer = packetBuffer;
            _bufferIndex = PacketHeader.HeaderSize;
        }

        protected void SerializeHeader()
        {
            var packetLength = PacketHeader.HeaderSize + _bufferIndex;
            _header.Length = (short)packetLength;
            var packetId = _header.PacketId;

            var lengthBytes = BitConverter.GetBytes(packetLength);
            var packetIdBytes = BitConverter.GetBytes(packetId);

            Array.Copy(lengthBytes, 0, _buffer, 0, sizeof(short));
            Array.Copy(packetIdBytes, 0, _buffer, sizeof(short), sizeof(short));
        }

        protected string GetString()
        {
            var stringLength = BitConverter.ToInt32(_buffer, _bufferIndex);
            _bufferIndex += sizeof(int);

            var data = Encoding.UTF8.GetString(_buffer, _bufferIndex, stringLength);
            _bufferIndex += stringLength;

            return data;
        }

        protected int GetInt()
        {
            var data = BitConverter.ToInt32(_buffer, _bufferIndex);
            _bufferIndex += sizeof(int);

            return data;
        }

        protected short GetShort()
        {
            var data = BitConverter.ToInt16(_buffer, _bufferIndex);
            _bufferIndex += sizeof(short);

            return data;
        }

        protected float GetFloat()
        {
            var data = BitConverter.ToSingle(_buffer, _bufferIndex);
            _bufferIndex += sizeof(float);

            return data;
        }

        protected void SetString(string data)
        {
            var utf8String = Encoding.UTF8.GetBytes(data);
            var stringLength = BitConverter.GetBytes(utf8String.Length);

            Array.Copy(stringLength, 0, _buffer, _bufferIndex, stringLength.Length);
            _bufferIndex += stringLength.Length;

            Array.Copy(utf8String, 0, _buffer, _bufferIndex, utf8String.Length);
            _bufferIndex += utf8String.Length;
        }

        protected void SetInt(int data)
        {
            var bytes = BitConverter.GetBytes(data);
            Array.Copy(bytes, 0, _buffer, _bufferIndex, sizeof(int));
        }

        public abstract void Serialize();
        public abstract void Deserialize();

    }
}
