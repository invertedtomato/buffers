using System;

namespace InvertedTomato.IO.Buffers.ByteBuffers {
    public class ByteBufferWriter {
        private const int GrowthRate = 2;

        private Buffer<byte> Buffer;

        public ByteBufferWriter(int initialCapacity) {
#if DEBUG
            if (initialCapacity < 1) {
                throw new ArgumentOutOfRangeException("Must be at least 1 byte.", "initialCapacity");
            }
#endif

            Buffer = new Buffer<byte>(initialCapacity);
        }

        public ByteBufferWriter WriteUInt8(byte value) {
            return Write(new byte[] { value });
        }

        public ByteBufferWriter WriteSInt8(sbyte value) {
            return Write(new byte[] { (byte)value });
        }

        public ByteBufferWriter WriteUInt16(ushort value) {
            return Write(BitConverter.GetBytes(value));
        }

        public ByteBufferWriter WriteSInt16(short value) {
            return Write(BitConverter.GetBytes(value));
        }

        public ByteBufferWriter WriteUInt32(uint value) {
            return Write(BitConverter.GetBytes(value));
        }

        public ByteBufferWriter WriteSInt32(int value) {
            return Write(BitConverter.GetBytes(value));
        }

        public ByteBufferWriter WriteUInt64(ulong value) {
            return Write(BitConverter.GetBytes(value));
        }

        public ByteBufferWriter WriteSInt64(long value) {
            return Write(BitConverter.GetBytes(value));
        }

        public ByteBufferWriter WriteFloat(float value) {
            return Write(BitConverter.GetBytes(value));
        }

        public ByteBufferWriter WriteDouble(double value) {
            return Write(BitConverter.GetBytes(value));
        }

        public ByteBufferWriter WriteBoolean(bool value) {
            return Write(new byte[] { value ? (byte)0xff : (byte)0x00 });
        }

        public ByteBufferWriter WriteGuid(Guid value) {
            return Write(value.ToByteArray());
        }

        public ByteBufferWriter Write(byte[] value) {
#if DEBUG
            if (null == value) {
                throw new ArgumentNullException("value");
            }
#endif

            // If buffer would overflow...
            if (Buffer.Writable < value.Length) {
                // Increase it's capacity
                Buffer = Buffer.Resize(Math.Max(Buffer.Capacity + value.Length, Buffer.Capacity * GrowthRate));
            }

            // Add to queue
            Buffer.EnqueueArray(value);

            return this;
        }

        public Buffer<byte> ToByteBuffer() {
            return Buffer;
        }
    }
}
