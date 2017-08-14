using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InvertedTomato.IO.Buffers.ByteBuffers {
    [TestClass]
    public class ByteBufferReaderTests {
        [TestMethod]
        public void ReadUInt8() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x08 }));
            Assert.AreEqual(8, buff.ReadUInt8());
        }
        [TestMethod]
        public void ReadSInt8() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x08 }));
            Assert.AreEqual(8, buff.ReadSInt8());
        }

        [TestMethod]
        public void ReadUInt16() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x08,0x00 }));
            Assert.AreEqual(8, buff.ReadUInt16());
        }
        [TestMethod]
        public void ReadSInt16() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x08, 0x00 }));
            Assert.AreEqual(8, buff.ReadSInt16());
        }

        [TestMethod]
        public void ReadUInt32() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x08, 0x00,0x00,0x00 }));
            Assert.AreEqual((uint)8, buff.ReadUInt32());
        }
        [TestMethod]
        public void ReadSInt32() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x08, 0x00, 0x00, 0x00 }));
            Assert.AreEqual(8, buff.ReadSInt32());
        }

        [TestMethod]
        public void ReadUInt64() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
            Assert.AreEqual((ulong)8, buff.ReadUInt64());
        }
        [TestMethod]
        public void ReadSInt64() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
            Assert.AreEqual(8, buff.ReadSInt64());
        }

        [TestMethod]
        public void ReadFloat() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0xCD, 0xCC, 0x8C, 0x3F }));
            Assert.AreEqual((float)1.1, buff.ReadFloat());
        }
        [TestMethod]
        public void ReadDouble() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x9A,0x99, 0x99, 0x99, 0x99, 0x99, 0xF1, 0x3F }));
            Assert.AreEqual(1.1, buff.ReadDouble());
        }

        [TestMethod]
        public void ReadBoolean_False() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x00 }));
            Assert.AreEqual(false, buff.ReadBoolean());
        }
        [TestMethod]
        public void ReadBoolean_True1() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0xff }));
            Assert.AreEqual(true, buff.ReadBoolean());
        }
        [TestMethod]
        public void ReadBoolean_True2() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x01 }));
            Assert.AreEqual(true, buff.ReadBoolean());
        }

        [TestMethod]
        public void ReadGuid() {
            var buff = new ByteBufferReader(new Buffer<byte>(new byte[] { 0x01,0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 }));
            Assert.AreEqual(new Guid(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }), buff.ReadGuid());
        }
    }
}
