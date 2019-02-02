using System;

namespace InvertedTomato.IO.Bits.Tests {
	[TestClass]
	public class BitsTests {
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ParseString_Bad1() {
			BitOperation.ParseToBytes("0");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ParseString_Bad2() {
			BitOperation.ParseToBytes("00000000a");
		}

		[TestMethod]
		public void ToString_0() {
			Assert.AreEqual("0", BitOperation.ToString(0));
		}

		[TestMethod]
		public void ToString_1() {
			Assert.AreEqual("1", BitOperation.ToString(1));
		}

		[TestMethod]
		public void ToString_2() {
			Assert.AreEqual("10", BitOperation.ToString(2));
		}

		[TestMethod]
		public void ToString_255() {
			Assert.AreEqual("11111111", BitOperation.ToString(255));
		}

		[TestMethod]
		public void ToString_256() {
			Assert.AreEqual("1 00000000", BitOperation.ToString(256));
		}

		[TestMethod]
		public void ToString_256_16() {
			Assert.AreEqual("00000001 00000000", BitOperation.ToString(256, 16));
		}

		[TestMethod]
		public void ToString_65535() {
			Assert.AreEqual("11111111 11111111", BitOperation.ToString(65535));
		}

		[TestMethod]
		public void ToString_Max() {
			Assert.AreEqual("11111111 11111111 11111111 11111111 11111111 11111111 11111111 11111111", BitOperation.ToString(UInt64.MaxValue));
		}

		[TestMethod]
		public void CountUsed_0() {
			Assert.AreEqual(1, BitOperation.CountUsed(0));
		}

		[TestMethod]
		public void CountUsed_1() {
			Assert.AreEqual(1, BitOperation.CountUsed(1));
		}

		[TestMethod]
		public void CountUsed_3() {
			Assert.AreEqual(2, BitOperation.CountUsed(3));
		}


		[TestMethod]
		public void Push_0_1_1() {
			Assert.AreEqual((UInt64) 1, BitOperation.Push(0, 1, 1));
		}

		[TestMethod]
		public void Push_1_1_1() {
			Assert.AreEqual((UInt64) 3, BitOperation.Push(1, 1, 1));
		}

		[TestMethod]
		public void Push_1_3_1() {
			Assert.AreEqual((UInt64) 3, BitOperation.Push(1, 3, 1));
		}

		[TestMethod]
		public void Push_1_3_2() {
			Assert.AreEqual((UInt64) 7, BitOperation.Push(1, 3, 2));
		}

		[TestMethod]
		public void Pop_0_1() {
			Assert.AreEqual((UInt64) 0, BitOperation.Pop(0, 1));
		}

		[TestMethod]
		public void Pop_3_1() {
			Assert.AreEqual((UInt64) 1, BitOperation.Pop(3, 1));
		}

		[TestMethod]
		public void ParseToBytes_() {
			var result = BitOperation.ParseToBytes("");
			Assert.AreEqual(0, result.Length);
		}

		[TestMethod]
		public void ParseToBytes_00000000() {
			var result = BitOperation.ParseToBytes("00000000");
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(Byte.MinValue, result[0]);
		}

		[TestMethod]
		public void ParseToBytes_________() {
			var result = BitOperation.ParseToBytes("________");
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(Byte.MinValue, result[0]);
		}

		[TestMethod]
		public void ParseToBytes_11111111() {
			var result = BitOperation.ParseToBytes("11111111");
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(Byte.MaxValue, result[0]);
		}

		[TestMethod]
		public void ParseToBytes_11111111_00000000() {
			var result = BitOperation.ParseToBytes("11111111 00000000");
			Assert.AreEqual(2, result.Length);
			Assert.AreEqual(Byte.MaxValue, result[0]);
			Assert.AreEqual(Byte.MinValue, result[1]);
		}
	}
}