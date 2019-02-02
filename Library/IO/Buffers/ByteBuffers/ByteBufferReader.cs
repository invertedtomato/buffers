using System;

namespace InvertedTomato.IO.Buffers.ByteBuffers {
	public class ByteBufferReader {
		private readonly Buffer<Byte> Buffer;

		public ByteBufferReader(Buffer<Byte> buffer) {
#if DEBUG
			if (null == buffer) {
				throw new ArgumentNullException("buffer");
			}
#endif
			Buffer = buffer;
		}


		public Byte ReadUInt8() {
			return Buffer.Dequeue();
		}

		public SByte ReadSInt8() {
			return (SByte) Buffer.Dequeue();
		}

		public UInt16 ReadUInt16() {
			var ret = Buffer.DequeueBuffer(2);
			return BitConverter.ToUInt16(ret.GetUnderlying(), ret.Start);
		}

		public Int16 ReadSInt16() {
			var ret = Buffer.DequeueBuffer(2);
			return BitConverter.ToInt16(ret.GetUnderlying(), ret.Start);
		}

		public UInt32 ReadUInt32() {
			var ret = Buffer.DequeueBuffer(4);
			return BitConverter.ToUInt32(ret.GetUnderlying(), ret.Start);
		}

		public Int32 ReadSInt32() {
			var ret = Buffer.DequeueBuffer(4);
			return BitConverter.ToInt32(ret.GetUnderlying(), ret.Start);
		}

		public UInt64 ReadUInt64() {
			var ret = Buffer.DequeueBuffer(8);
			return BitConverter.ToUInt64(ret.GetUnderlying(), ret.Start);
		}

		public Int64 ReadSInt64() {
			var ret = Buffer.DequeueBuffer(8);
			return BitConverter.ToInt64(ret.GetUnderlying(), ret.Start);
		}

		public Single ReadFloat() {
			var ret = Buffer.DequeueBuffer(4);
			return BitConverter.ToSingle(ret.GetUnderlying(), ret.Start);
		}

		public Double ReadDouble() {
			var ret = Buffer.DequeueBuffer(8);
			return BitConverter.ToDouble(ret.GetUnderlying(), ret.Start);
		}

		public Boolean ReadBoolean() {
			return Buffer.Dequeue() > 0;
		}

		public Guid ReadGuid() {
			return new Guid(Buffer.DequeueArray(16));
		}

		public Byte[] Read(Int32 length) {
#if DEBUG
			if (length < 0) {
				throw new ArgumentOutOfRangeException("length");
			}
#endif

			return Buffer.DequeueArray(length);
		}
	}
}