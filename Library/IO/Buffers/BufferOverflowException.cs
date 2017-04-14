using System;

namespace InvertedTomato.IO.Buffers {
    public class BufferOverflowException : System.Exception {
        public BufferOverflowException() { }
        public BufferOverflowException(string message) : base(message) { }
        public BufferOverflowException(string message, Exception inner) : base(message, inner) { }
    }
}