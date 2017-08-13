using System;

namespace InvertedTomato.IO.Buffers.ByteBuffers {
    public class ByteBufferReader {
        private Buffer<byte> Buffer;

        public ByteBufferReader(Buffer<byte> buffer) {
#if DEBUG
            if (null == buffer) {
                throw new ArgumentNullException("buffer");
            }
#endif
            Buffer = buffer;
        }


        public byte ReadUInt8() {
            return Buffer.Dequeue();
        }

        public sbyte ReadSInt8() {
            return (sbyte)Buffer.Dequeue();
        }

        public ushort ReadUInt16() {
            var ret = Buffer.DequeueBuffer(2);
            return BitConverter.ToUInt16(ret.GetUnderlying(), ret.Start);
        }

        public short ReadSInt16() {
            var ret = Buffer.DequeueBuffer(2);
            return BitConverter.ToInt16(ret.GetUnderlying(), ret.Start);
        }

        public uint ReadUInt32() {
            var ret = Buffer.DequeueBuffer(4);
            return BitConverter.ToUInt32(ret.GetUnderlying(), ret.Start);
        }

        public int ReadSInt32() {
            var ret = Buffer.DequeueBuffer(4);
            return BitConverter.ToInt32(ret.GetUnderlying(), ret.Start);
        }

        public ulong ReadUInt64() {
            var ret = Buffer.DequeueBuffer(8);
            return BitConverter.ToUInt64(ret.GetUnderlying(), ret.Start);
        }

        public long ReadSInt64() {
            var ret = Buffer.DequeueBuffer(8);
            return BitConverter.ToInt64(ret.GetUnderlying(), ret.Start);
        }

        public float ReadFloat() {
            var ret = Buffer.DequeueBuffer(4);
            return BitConverter.ToSingle(ret.GetUnderlying(), ret.Start);
        }

        public double ReadDouble() {
            var ret = Buffer.DequeueBuffer(8);
            return BitConverter.ToDouble(ret.GetUnderlying(), ret.Start);
        }

        public bool ReadBoolean() {
            return Buffer.Dequeue() > 0;
        }

        public Guid ReadGuid() {
            return new Guid(Buffer.DequeueArray(16));
        }

        public byte[] Read(int length) {
#if DEBUG
            if (length < 0) {
                throw new ArgumentOutOfRangeException("length");
            }
#endif

            return Buffer.DequeueArray(length);
        }
    }
}
