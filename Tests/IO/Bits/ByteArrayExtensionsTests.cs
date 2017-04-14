using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvertedTomato.IO.Bits;

namespace InvertedTomato.IO.Bits.Tests.BitOperations {
    [TestClass]
    public class ByteArrayExtensionsTests {
        [TestMethod]
        public void ToBinaryString_0() {
            Assert.AreEqual("00000000", new byte[] { 0 }.ToBinaryString());
        }
        [TestMethod]
        public void ToBinaryString_1() {
            Assert.AreEqual("00000001", new byte[] { 1 }.ToBinaryString());
        }
        [TestMethod]
        public void ToBinaryString_255() {
            Assert.AreEqual("11111111", new byte[] { 255 }.ToBinaryString());
        }
        [TestMethod]
        public void ToBinaryString_0_255() {
            Assert.AreEqual("00000000 11111111", new byte[] { 0, 255 }.ToBinaryString());
        }
    }
}
