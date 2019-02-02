using System;

namespace InvertedTomato.IO.Buffers {
	[Obsolete]
	public class BitBuffer {
        /// <summary>
        ///     The underlying buffer.
        /// </summary>
        private Byte Buffer;

        /// <summary>
        ///     The position in the underlying buffer.
        /// </summary>
        private Int32 Offset;

		public BitBuffer() { }

		public BitBuffer(Byte buffer, Int32 offset) {
#if DEBUG
			if (offset < 0 || offset > 8) {
				throw new ArgumentOutOfRangeException("Offset must be between 0 and 8 inclusive.", "offset");
			}
#endif

			// Store
			Buffer = buffer;
			Offset = offset;
		}

        /// <summary>
        ///     If the buffer is full.
        /// </summary>
        public Boolean IsFull => Offset == 8;

        /// <summary>
        ///     If the buffer contains anything.
        /// </summary>
        public Boolean IsDirty => Offset > 0;

		public static implicit operator Byte(BitBuffer value) {
			return value.Buffer;
		}

		public Boolean Append(Boolean value) {
#if DEBUG
			// Check if buffer is full
			if (Offset == 8) {
				throw new OverflowException("Buffer full");
			}
#endif
			// Write bit
			if (value) {
				Buffer |= (Byte) (1 << (7 - Offset));
			}

			// Increment offset;
			Offset++;

			return Offset == 8;
		}

		public Byte Clear() {
			// Copy value
			var value = Buffer;

			// Reset value
			Buffer = 0;

			// Reset offset
			Offset = 0;

			// Return value
			return value;
		}
	}
}