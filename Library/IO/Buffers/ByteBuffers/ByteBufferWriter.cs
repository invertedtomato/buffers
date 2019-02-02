using System;

namespace InvertedTomato.IO.Buffers.ByteBuffers {
	public class ByteBufferWriter {
		private const Int32 GrowthRate = 2;

		private Buffer<Byte> Buffer;

		public ByteBufferWriter(Int32 initialCapacity) {
#if DEBUG
			if (initialCapacity < 1) {
				throw new ArgumentOutOfRangeException("Must be at least 1 byte.", "initialCapacity");
			}
#endif

			Buffer = new Buffer<Byte>(initialCapacity);
		}

		public ByteBufferWriter WriteUInt8(Byte value) {
			return Write(new[] {value});
		}

		public ByteBufferWriter WriteSInt8(SByte value) {
			return Write(new[] {(Byte) value});
		}

		public ByteBufferWriter WriteUInt16(UInt16 value) {
			return Write(BitConverter.GetBytes(value));
		}

		public ByteBufferWriter WriteSInt16(Int16 value) {
			return Write(BitConverter.GetBytes(value));
		}

		public ByteBufferWriter WriteUInt32(UInt32 value) {
			return Write(BitConverter.GetBytes(value));
		}

		public ByteBufferWriter WriteSInt32(Int32 value) {
			return Write(BitConverter.GetBytes(value));
		}

		public ByteBufferWriter WriteUInt64(UInt64 value) {
			return Write(BitConverter.GetBytes(value));
		}

		public ByteBufferWriter WriteSInt64(Int64 value) {
			return Write(BitConverter.GetBytes(value));
		}

		public ByteBufferWriter WriteFloat(Single value) {
			return Write(BitConverter.GetBytes(value));
		}

		public ByteBufferWriter WriteDouble(Double value) {
			return Write(BitConverter.GetBytes(value));
		}

		public ByteBufferWriter WriteBoolean(Boolean value) {
			return Write(new[] {value ? (Byte) 0xff : (Byte) 0x00});
		}

		public ByteBufferWriter WriteGuid(Guid value) {
			return Write(value.ToByteArray());
		}

		public ByteBufferWriter Write(Byte[] value) {
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

		public Buffer<Byte> ToByteBuffer() {
			return Buffer;
		}
	}
}