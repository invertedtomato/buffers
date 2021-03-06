﻿using System;
using System.Collections.Generic;

namespace InvertedTomato.IO.Bits.Tests {
	[TestClass]
	public class ASyncBitReaderTests {
		[TestMethod]
		public void Read_1Byte_1Bit() {
			var position = 0;
			var results = new List<UInt64> {0, 0, 0, 0, 0, 0, 0, 0, 1};

			var reader = new ASyncBitReader((value, count) => {
				var expected = results[position++];

				Assert.AreEqual(expected, value, "Position #" + (position - 1));

				return 1;
			});

			reader.Insert(new Byte[] {1});
		}

		[TestMethod]
		public void Read_2Byte_1Bit() {
			var position = 0;
			var results = new List<UInt64> {0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0};

			var reader = new ASyncBitReader((value, count) => {
				var expected = results[position++];

				Assert.AreEqual(expected, value, "Position #" + (position - 1));

				return 1;
			});

			reader.Insert(new Byte[] {1, 2});
		}

		[TestMethod]
		public void Read_2Byte_3Bit() {
			var position = 0;
			var results = new List<UInt64> {0, 0, 0, 2, 0, 1};

			var reader = new ASyncBitReader((value, count) => {
				var expected = results[position++];

				Assert.AreEqual(expected, value, "Position #" + (position - 1));

				return 3;
			});

			reader.Insert(new Byte[] {1, 2});
		}
	}
}