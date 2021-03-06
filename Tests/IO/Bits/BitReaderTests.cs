﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InvertedTomato.IO.Bits.Tests {
	[TestClass]
	public class BitReaderTests {
		private IEnumerable<String> Read(String value, Byte length) {
			using (var stream = new MemoryStream(BitOperation.ParseToBytes(value))) {
				using (var reader = new BitReader(stream)) {
					for (var i = 0; i < value.Replace(" ", "").Length / length; i++) {
						yield return BitOperation.ToString(reader.Read(length), length);
					}
				}
			}
		}

		private Boolean PeakBit(String value) {
			using (var stream = new MemoryStream(BitOperation.ParseToBytes(value))) {
				using (var reader = new BitReader(stream)) {
					return reader.PeakBit();
				}
			}
		}

		[TestMethod]
		public void PeakBit_0() {
			Assert.AreEqual(false, PeakBit("00000000"));
		}

		[TestMethod]
		public void PeakBit_1() {
			Assert.AreEqual(true, PeakBit("10000000"));
		}

		[TestMethod]
		public void Read_1() {
			var result = Read("01000000 10000000", 1);
			Assert.AreEqual("0", result.ElementAt(0));
			Assert.AreEqual("1", result.ElementAt(1));
			Assert.AreEqual("0", result.ElementAt(2));
			Assert.AreEqual("0", result.ElementAt(3));
			Assert.AreEqual("0", result.ElementAt(4));
			Assert.AreEqual("0", result.ElementAt(5));
			Assert.AreEqual("0", result.ElementAt(6));
			Assert.AreEqual("0", result.ElementAt(7));

			Assert.AreEqual("1", result.ElementAt(8));
			Assert.AreEqual("0", result.ElementAt(9));
			Assert.AreEqual("0", result.ElementAt(10));
			Assert.AreEqual("0", result.ElementAt(11));
			Assert.AreEqual("0", result.ElementAt(12));
			Assert.AreEqual("0", result.ElementAt(13));
			Assert.AreEqual("0", result.ElementAt(14));
			Assert.AreEqual("0", result.ElementAt(15));
		}

		[TestMethod]
		public void Read_5() {
			var result = Read("01000000 10000000 00000000", 5);
			Assert.AreEqual("01000", result.ElementAt(0));
			Assert.AreEqual("00010", result.ElementAt(1));
			Assert.AreEqual("00000", result.ElementAt(3));
		}

		[TestMethod]
		public void Read_12() {
			var result = Read("01000000 10000000 00000000", 12);
			Assert.AreEqual("0100 00001000", result.ElementAt(0));
			Assert.AreEqual("0000 00000000", result.ElementAt(1));
		}

		[TestMethod]
		[ExpectedException(typeof(EndOfStreamException))]
		public void Read_EndOfStream() {
			using (var stream = new MemoryStream(BitOperation.ParseToBytes("00000000"))) {
				using (var reader = new BitReader(stream)) {
					reader.Read(6);
					reader.Read(6);
				}
			}
		}

		[TestMethod]
		public void ReadWrite() {
			using (var stream = new MemoryStream()) {
				using (var writer = new BitWriter(stream)) {
					for (UInt64 i = 1; i < UInt64.MaxValue / 2; i *= 2) {
						writer.Write(i, BitOperation.CountUsed(i));
					}
				}

				stream.Position = 0;

				using (var reader = new BitReader(stream)) {
					for (UInt64 i = 1; i < UInt64.MaxValue / 2; i *= 2) {
						Assert.AreEqual(i, reader.Read(BitOperation.CountUsed(i)));
					}
				}
			}
		}
	}
}