﻿using System;
using System.Collections.Generic;
using InvertedTomato.IO.Buffers;

namespace InvertedTomato.Tests {
	[TestClass]
	public class BufferTests {
		[TestMethod]
		public void Init_MaxCapacity() {
			var buffer = new Buffer<Byte>(2);

			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(0, buffer.End);
			Assert.AreEqual(0, buffer.Readable);
			Assert.AreEqual(2, buffer.Writable);
			Assert.AreEqual(2, buffer.Capacity);
			Assert.AreEqual(true, buffer.IsWritable);
			Assert.AreEqual(false, buffer.IsReadable);
		}

		[TestMethod]
		public void Init_Array() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});

			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(2, buffer.End);
		}

		[TestMethod]
		public void Init_ArraySubset() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 1);

			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(1, buffer.End);
			Assert.AreEqual(2, buffer.Capacity);
		}

		[TestMethod]
		public void Init_ArraySuperset() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 4);

			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(2, buffer.End);
			Assert.AreEqual(4, buffer.Capacity);
		}

		[TestMethod]
		public void Enqueue() {
			var buffer = new Buffer<Byte>(2);

			// First enqueue
			buffer.Enqueue(1);

			// Check underlying
			var underlying = buffer.GetUnderlying();
			Assert.AreEqual(2, underlying.Length);
			Assert.AreEqual(1, underlying[0]);
			Assert.AreEqual(0, underlying[1]);

			// Get others
			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(1, buffer.End);
			Assert.AreEqual(1, buffer.Readable);
			Assert.AreEqual(1, buffer.Writable);
			Assert.AreEqual(2, buffer.Capacity);
			Assert.AreEqual(true, buffer.IsReadable);
			Assert.AreEqual(true, buffer.IsWritable);

			// Second enqueue
			buffer.Enqueue(1);

			// Check underlying
			underlying = buffer.GetUnderlying();
			Assert.AreEqual(2, underlying.Length);
			Assert.AreEqual(1, underlying[0]);
			Assert.AreEqual(1, underlying[1]);

			// Check others
			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(2, buffer.End);
			Assert.AreEqual(2, buffer.Readable);
			Assert.AreEqual(0, buffer.Writable);
			Assert.AreEqual(2, buffer.Capacity);
			Assert.AreEqual(false, buffer.IsWritable);
			Assert.AreEqual(true, buffer.IsReadable);
		}

		[TestMethod]
		public void EnqueueBuffer() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 10);

			buffer.EnqueueBuffer(buffer);
			Assert.AreEqual(4, buffer.Readable);
			var result = buffer.ToArray();
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(2, result[1]);
			Assert.AreEqual(1, result[2]);
			Assert.AreEqual(2, result[3]);
		}

		[TestMethod]
		public void EnqueueBuffer_ExactSpace() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 4);

			buffer.EnqueueBuffer(buffer);
			Assert.AreEqual(4, buffer.Readable);
			var result = buffer.ToArray();
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(2, result[1]);
			Assert.AreEqual(1, result[2]);
			Assert.AreEqual(2, result[3]);
		}

		[TestMethod]
		public void Enqueue_Array() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 10);

			buffer.EnqueueArray(new Byte[] {1, 2});


			Assert.AreEqual(4, buffer.Readable);

			// Check underlying
			var underlying = buffer.GetUnderlying();
			Assert.AreEqual(10, underlying.Length);
			Assert.AreEqual(1, underlying[0]);
			Assert.AreEqual(2, underlying[1]);
			Assert.AreEqual(1, underlying[2]);
			Assert.AreEqual(2, underlying[3]);
		}

		[TestMethod]
		public void Enqueue_Array_Offset() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 10);

			buffer.EnqueueArray(new Byte[] {1, 2, 3, 4}, 2);


			Assert.AreEqual(4, buffer.Readable);

			// Check underlying
			var underlying = buffer.GetUnderlying();
			Assert.AreEqual(10, underlying.Length);
			Assert.AreEqual(1, underlying[0]);
			Assert.AreEqual(2, underlying[1]);
			Assert.AreEqual(3, underlying[2]);
			Assert.AreEqual(4, underlying[3]);
		}

		[TestMethod]
		public void Enqueue_Array_OffsetCount() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 10);

			buffer.EnqueueArray(new Byte[] {1, 2, 3, 4}, 2, 1);


			Assert.AreEqual(3, buffer.Readable);

			// Check underlying
			var underlying = buffer.GetUnderlying();
			Assert.AreEqual(10, underlying.Length);
			Assert.AreEqual(1, underlying[0]);
			Assert.AreEqual(2, underlying[1]);
			Assert.AreEqual(3, underlying[2]);
		}

		[TestMethod]
		[ExpectedException(typeof(BufferOverflowException))]
		public void Enqueue_Overflow() {
			var buffer = new Buffer<Byte>(2);
			buffer.Enqueue(1);
			buffer.Enqueue(2);
			buffer.Enqueue(3);
		}

		[TestMethod]
		public void Enqueue_AutoGrow() {
			var buffer = new Buffer<Byte>(2);
			buffer.AutoGrow = true;
			buffer.Enqueue(1);
			buffer.Enqueue(2);
			buffer.Enqueue(3);
		}


		[TestMethod]
		public void Dequeue() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});

			Assert.AreEqual(1, buffer.Dequeue());
			Assert.AreEqual(1, buffer.Start);
			Assert.AreEqual(2, buffer.End);

			Assert.AreEqual(2, buffer.Dequeue());
			Assert.AreEqual(2, buffer.Start);
			Assert.AreEqual(2, buffer.End);
		}

		[TestMethod]
		public void DequeueBuffer() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});

			var extracted = buffer.DequeueBuffer(1);
			Assert.AreEqual(1, extracted.Readable);
			Assert.AreEqual(1, extracted.Dequeue());
			Assert.AreEqual(1, buffer.Start);
			Assert.AreEqual(2, buffer.End);

			Assert.AreEqual(2, buffer.Dequeue());
			Assert.AreEqual(2, buffer.Start);
			Assert.AreEqual(2, buffer.End);
		}

		[TestMethod]
		[ExpectedException(typeof(BufferOverflowException))]
		public void Dequeue_Overflow() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});

			Assert.AreEqual(1, buffer.Dequeue());
			Assert.AreEqual(2, buffer.Dequeue());
			buffer.Dequeue();
		}

		[TestMethod]
		public void TryDequeue() {
			Byte output;

			var buffer = new Buffer<Byte>(new Byte[] {1, 2});

			Assert.AreEqual(true, buffer.TryDequeue(out output));
			Assert.AreEqual(1, output);
			Assert.AreEqual(1, buffer.Start);
			Assert.AreEqual(2, buffer.End);

			Assert.AreEqual(true, buffer.TryDequeue(out output));
			Assert.AreEqual(2, output);
			Assert.AreEqual(2, buffer.Start);
			Assert.AreEqual(2, buffer.End);

			Assert.AreEqual(false, buffer.TryDequeue(out output));
			Assert.AreEqual(0, output);
			Assert.AreEqual(2, buffer.Start);
			Assert.AreEqual(2, buffer.End);

			Assert.AreEqual(false, buffer.TryDequeue(out output));
			Assert.AreEqual(0, output);
			Assert.AreEqual(2, buffer.Start);
			Assert.AreEqual(2, buffer.End);
		}

		[TestMethod]
		public void Peek() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});

			Assert.AreEqual(1, buffer.Peek());
			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(2, buffer.End);

			Assert.AreEqual(1, buffer.Peek());
			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(2, buffer.End);
		}

		[TestMethod]
		[ExpectedException(typeof(BufferOverflowException))]
		public void Peek_Overflow() {
			var buffer = new Buffer<Byte>(0);

			buffer.Peek();
		}

		[TestMethod]
		public void TryPeek() {
			Byte value;

			var buffer = new Buffer<Byte>(new Byte[] {1, 2});

			Assert.AreEqual(true, buffer.TryPeek(out value));
			Assert.AreEqual(1, value);
			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(2, buffer.End);

			Assert.AreEqual(true, buffer.TryPeek(out value));
			Assert.AreEqual(1, value);
			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(2, buffer.End);
		}

		[TestMethod]
		public void TryPeek_Overflow() {
			Byte value;

			var buffer = new Buffer<Byte>(0);

			Assert.AreEqual(false, buffer.TryPeek(out value));
		}

		[TestMethod]
		public void Peek_Absolute() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});

			Assert.AreEqual(1, buffer.Peek(0));
			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(2, buffer.End);

			Assert.AreEqual(2, buffer.Peek(1));
			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(2, buffer.End);
		}

		[TestMethod]
		[ExpectedException(typeof(BufferOverflowException))]
		public void Peek_Absolute_Overflow() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});
			buffer.Peek(3);
		}

		[TestMethod]
		public void PeekBuffer() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2, 3}, 1, 2);
			var peeked = buffer.PeekBuffer(2).ToArray();
			Assert.AreEqual(2, peeked.Length);
			Assert.AreEqual(2, peeked[0]);
			Assert.AreEqual(3, peeked[1]);
		}

		[TestMethod]
		[ExpectedException(typeof(BufferOverflowException))]
		public void PeekBuffer_Overflow() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2, 3}, 1, 2);
			buffer.PeekBuffer(3);
		}

		[TestMethod]
		public void Trim_Start() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});
			var buffer2 = buffer.Trim(8);
			Assert.AreEqual(buffer.Start, buffer2.Start);
			Assert.AreEqual(buffer.End, buffer2.End);
			Assert.AreEqual(buffer.Peek(), buffer2.Peek());
		}

		[TestMethod]
		public void Trim_StartPlusOne() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});
			buffer.Dequeue();

			var buffer2 = buffer.Trim(8);
			Assert.AreEqual(0, buffer2.Start);
			Assert.AreEqual(1, buffer2.End);
			Assert.AreEqual(buffer.Peek(), buffer2.Peek());
		}

		[TestMethod]
		public void Trim_AlmostOverflow() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});
			buffer.Trim(2);
		}

		[TestMethod]
		[ExpectedException(typeof(BufferOverflowException))]
		public void Trim_Overflow() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});
			buffer.Trim(1);
		}

		[TestMethod]
		public void Enumerate() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2});

			var enumerator = ((IEnumerable<Byte>) buffer).GetEnumerator();
			Assert.AreEqual(true, enumerator.MoveNext());
			Assert.AreEqual(1, enumerator.Current);
			Assert.AreEqual(true, enumerator.MoveNext());
			Assert.AreEqual(2, enumerator.Current);
			Assert.AreEqual(false, enumerator.MoveNext());
		}


		[TestMethod]
		public void MoveEnd() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 4);
			buffer.MoveEnd(2);
			Assert.AreEqual(4, buffer.End);
			buffer.MoveEnd(-2);
			Assert.AreEqual(2, buffer.End);
			buffer.MoveEnd(0);
			Assert.AreEqual(2, buffer.End);
		}

		[TestMethod]
		[ExpectedException(typeof(OverflowException))]
		public void MoveEnd_TooHigh() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 4);
			buffer.MoveEnd(3);
		}

		[TestMethod]
		[ExpectedException(typeof(OverflowException))]
		public void MoveEnd_TooLow() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 4);
			buffer.MoveEnd(-3);
		}

		[TestMethod]
		public void MoveStart() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 4);
			buffer.MoveStart(2);
			Assert.AreEqual(2, buffer.Start);
			buffer.MoveStart(-2);
			Assert.AreEqual(0, buffer.Start);
			buffer.MoveStart(0);
			Assert.AreEqual(0, buffer.Start);
		}

		[TestMethod]
		[ExpectedException(typeof(OverflowException))]
		public void MoveStart_TooHigh() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 4);
			buffer.MoveStart(3);
		}

		[TestMethod]
		[ExpectedException(typeof(OverflowException))]
		public void MoveStart_TooLow() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2}, 4);
			buffer.MoveStart(-1);
		}

		[TestMethod]
		public void ToArray() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2, 3});
			buffer.Dequeue();

			var result = buffer.ToArray();
			Assert.AreEqual(2, result.Length);
			Assert.AreEqual(2, result[0]);
			Assert.AreEqual(3, result[1]);
		}

		[TestMethod]
		public void Reset() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2, 3}, 4);
			buffer.Dequeue();
			buffer.Reset();

			Assert.AreEqual(0, buffer.Start);
			Assert.AreEqual(0, buffer.End);
		}

		[TestMethod]
		public void ToString_ByteArray() {
			var buffer = new Buffer<Byte>(new Byte[] {1, 2, 3}, 1, 2);
			Assert.AreEqual("02-03", buffer.ToString());
		}


		[TestMethod]
		public void ToString_IntArray() {
			var buffer = new Buffer<Int32>(new[] {1, 2, 3}, 1, 2);
			Assert.AreEqual("2-3", buffer.ToString());
		}

		[TestMethod]
		public void Replace() {
			var buffer = new Buffer<Int32>(new[] {1, 2, 3}, 1, 2);
			buffer.Replace(1, 5);

			var result = buffer.ToArray();
			Assert.AreEqual(2, result.Length);
			Assert.AreEqual(5, result[0]);
			Assert.AreEqual(3, result[1]);
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void Replace_IndexTooLow() {
			var buffer = new Buffer<Int32>(new[] {1, 2, 3}, 1, 2);
			buffer.Replace(0, 5);
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void Replace_IndexTooHigh() {
			var buffer = new Buffer<Int32>(new[] {1, 2, 3}, 1, 2);
			buffer.Replace(3, 5);
		}


		[TestMethod]
		public void Grow() {
			var buffer = new Buffer<Byte>(2);
			buffer.GrowExponential();
			Assert.AreEqual(buffer.Capacity, 3);

			buffer.GrowExponential(20);
			Assert.AreEqual(buffer.Capacity, 23);
		}
	}
}