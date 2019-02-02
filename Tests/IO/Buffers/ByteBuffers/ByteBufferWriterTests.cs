using System;

namespace InvertedTomato.IO.Buffers.ByteBuffers {
	[TestClass]
	public class ByteBufferWriterTests {
		[TestMethod]
		public void WriteUInt8() {
			var buff = new ByteBufferWriter(1);
			buff.WriteUInt8(8);
			Assert.AreEqual("08", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteSInt8() {
			var buff = new ByteBufferWriter(1);
			buff.WriteSInt8(8);
			Assert.AreEqual("08", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteUInt16() {
			var buff = new ByteBufferWriter(1);
			buff.WriteUInt16(8);
			Assert.AreEqual("0800", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteSInt16() {
			var buff = new ByteBufferWriter(1);
			buff.WriteSInt16(8);
			Assert.AreEqual("0800", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteUInt32() {
			var buff = new ByteBufferWriter(1);
			buff.WriteUInt32(8);
			Assert.AreEqual("08000000", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteSInt32() {
			var buff = new ByteBufferWriter(1);
			buff.WriteSInt32(8);
			Assert.AreEqual("08000000", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteUInt64() {
			var buff = new ByteBufferWriter(1);
			buff.WriteUInt64(8);
			Assert.AreEqual("0800000000000000", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteSInt64() {
			var buff = new ByteBufferWriter(1);
			buff.WriteSInt64(8);
			Assert.AreEqual("0800000000000000", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteFloat() {
			var buff = new ByteBufferWriter(1);
			buff.WriteFloat((Single) 1.1);
			Assert.AreEqual("CDCC8C3F", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteDouble() {
			var buff = new ByteBufferWriter(1);
			buff.WriteDouble(1.1);
			Assert.AreEqual("9A9999999999F13F", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteBoolean_False() {
			var buff = new ByteBufferWriter(1);
			buff.WriteBoolean(false);
			Assert.AreEqual("00", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteBoolean_True() {
			var buff = new ByteBufferWriter(1);
			buff.WriteBoolean(true);
			Assert.AreEqual("FF", buff.ToByteBuffer().ToArray().ToHexString());
		}

		[TestMethod]
		public void WriteGuid() {
			var buff = new ByteBufferWriter(1);
			buff.WriteGuid(new Guid(new Byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16}));
			Assert.AreEqual("0102030405060708090A0B0C0D0E0F10", buff.ToByteBuffer().ToArray().ToHexString());
		}
	}
}