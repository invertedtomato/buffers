using System;

namespace InvertedTomato.IO.Bits.Tests.BitOperations {
	[TestClass]
	public class ByteArrayExtensionsTests {
		[TestMethod]
		public void ToBinaryString_0() {
			Assert.AreEqual("00000000", new Byte[] {0}.ToBinaryString());
		}

		[TestMethod]
		public void ToBinaryString_1() {
			Assert.AreEqual("00000001", new Byte[] {1}.ToBinaryString());
		}

		[TestMethod]
		public void ToBinaryString_255() {
			Assert.AreEqual("11111111", new Byte[] {255}.ToBinaryString());
		}

		[TestMethod]
		public void ToBinaryString_0_255() {
			Assert.AreEqual("00000000 11111111", new Byte[] {0, 255}.ToBinaryString());
		}
	}
}