using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Buffer
{
    public class RingBuffer
    {
        public int UseSize { get; private set; }
        public int FreeSize => _buffer.Length - UseSize;

        private byte[] _buffer;
        private int _bufferFront;
        private int _bufferRear;
        private int _bufferEnd;

        public RingBuffer(int bufferSize)
        {
            _buffer = new byte[bufferSize];
            _bufferFront = 0;
            _bufferRear = 0;
            _bufferEnd = bufferSize;
            UseSize = 0;
        }

        public bool Enqueue(byte[] srcData)
        {
            var srcDataSize = srcData.Length;
            if (srcDataSize > FreeSize)
                return false;

            if (srcDataSize <= _bufferEnd - _bufferRear)
            {
                Array.Copy(srcData, 0, _buffer, _bufferRear, srcDataSize);
                _bufferRear += srcDataSize;
            }
            else
            {
                int rearRemainSize = _bufferEnd - _bufferRear;
                Array.Copy(srcData, 0, _buffer, _bufferRear, rearRemainSize);
                _bufferRear = 0;

                int remainDataSize = srcDataSize - rearRemainSize;
                Array.Copy(srcData, rearRemainSize, _buffer, _bufferRear, remainDataSize);
                _bufferRear += remainDataSize;
            }

            UseSize += srcDataSize;
            return true;
        }

        public byte[]? Dequeue(int size)
        {
            if (size > UseSize)
                return null;

            var data = new byte[size];
            if (size <= _bufferEnd - _bufferFront)
            {
                Array.Copy(_buffer, _bufferFront, data, 0, size);
                _bufferFront += size;
            }
            else
            {
                int frontDataSize = _bufferEnd - _bufferFront;
                Array.Copy(_buffer, _bufferFront, data, 0, frontDataSize);
                _bufferFront = 0;

                int remainDataSize = size - frontDataSize;
                Array.Copy(_buffer, _bufferFront, data, frontDataSize, remainDataSize);
                _bufferFront += remainDataSize;
            }

            UseSize -= size;
            return data;
        }

        public bool IsEmpty()
        {
            return UseSize == 0;
        }

        public void Clear()
        {
            Array.Clear(_buffer);
        }
    }
}
