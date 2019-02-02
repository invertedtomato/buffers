using System;
using System.IO;

namespace InvertedTomato.IO.Bits.Tests {
	[TestClass]
	public class BitWriterTests {
		private String WriteBits(params Boolean[] values) {
			using (var stream = new MemoryStream()) {
				using (var writer = new BitWriter(stream)) {
					foreach (var value in values) {
						writer.Write((UInt64) (value ? 1 : 0), 1);
					}
				}

				return stream.ToArray().ToBinaryString();
			}
		}


		private String Write(UInt64 value1, Byte length1, UInt64 value2 = 0, Byte length2 = 0) {
			using (var stream = new MemoryStream()) {
				using (var writer = new BitWriter(stream)) {
					writer.Write(value1, length1);
					if (length2 > 0) {
						writer.Write(value2, length2);
					}
				}

				return stream.ToArray().ToBinaryString();
			}
		}

		[TestMethod]
		public void Write_1() {
			Assert.AreEqual("10000000", WriteBits(true));
		}

		[TestMethod]
		public void Write_0() {
			Assert.AreEqual("00000000", WriteBits(false));
		}

		[TestMethod]
		public void Write_111111111() {
			Assert.AreEqual("11111111 10000000", WriteBits(true, true, true, true, true, true, true, true, true));
		}

		[TestMethod]
		public void Write_000000000() {
			Assert.AreEqual("00000000 00000000", WriteBits(false, false, false, false, false, false, false, false, false));
		}

		[TestMethod]
		public void Write_1L1() {
			Assert.AreEqual("10000000", Write(1, 1));
		}

		[TestMethod]
		public void Write_1L8() {
			Assert.AreEqual("00000001", Write(1, 8));
		}

		[TestMethod]
		public void Write_1L8_1L4() {
			Assert.AreEqual("00000001 00010000", Write(1, 8, 1, 4));
		}

		[TestMethod]
		public void Write_255L2_1L8() {
			Assert.AreEqual("11000000 01000000", Write(255, 2, 1, 8));
		}

		[TestMethod]
		public void Write_1L0() {
			Assert.AreEqual("", Write(1, 0));
		}

		[TestMethod]
		public void Write_1L9() {
			Assert.AreEqual("00000000 10000000", Write(1, 9));
		}

		[TestMethod]
		public void Write_255L9() {
			Assert.AreEqual("01111111 10000000", Write(255, 9));
		}
	}
}