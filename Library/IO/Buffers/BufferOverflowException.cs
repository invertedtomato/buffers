using System;

namespace InvertedTomato.IO.Buffers {
	public class BufferOverflowException : Exception {
		public BufferOverflowException() { }
		public BufferOverflowException(String message) : base(message) { }
		public BufferOverflowException(String message, Exception inner) : base(message, inner) { }
	}
}