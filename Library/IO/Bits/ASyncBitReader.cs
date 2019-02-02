using System;

namespace InvertedTomato.IO.Bits {
    /// <summary>
    ///     Asynchronous bit reader (experimental).
    /// </summary>
    public class ASyncBitReader {
        /// <summary>
        ///     The method to callback when we have reached the desired number of bits. Also returns number of bits to fetch next.
        /// </summary>
        private readonly Func<UInt64, Int32, Int32> Output;

        /// <summary>
        ///     Number of bits wanted by the receiver.
        /// </summary>
        private Int32 BitsWanted;

        /// <summary>
        ///     Buffer of current bits.
        /// </summary>
        private UInt64 Buffer;

        /// <summary>
        ///     Number of bits currently in the buffer.
        /// </summary>
        private Int32 Level;

        /// <summary>
        ///     Standard instantiation.
        /// </summary>
        /// <param name="output">Callback to output results to. Return the number of bits to read next.</param>
        public ASyncBitReader(Func<UInt64, Int32, Int32> output) {
			if (null == output) {
				throw new ArgumentNullException(nameof(output));
			}

			// Store
			Output = output;

			// Seed callback
			CallbackWrap(0, 0);
		}

        /// <summary>
        ///     Inject a number of bytes.
        /// </summary>
        /// <param name="buffer"></param>
        public void Insert(Byte[] buffer) {
			if (null == buffer) {
				throw new ArgumentNullException(nameof(buffer));
			}

			// For each byte
			foreach (var b in buffer) {
				// Check for buffer overflow
				if (Level + 8 > 64) {
					// TODO: Is this an issue?
					throw new OverflowException("Max of 64 bits can fit in buffer. Attempted to exceed by " + (64 - Level + 8) + " bits.");
				}

				// Load byte onto buffer
				Buffer = (Buffer << 8) | b;
				Level += 8;

				while (Level >= BitsWanted) {
					// Right align value
					var value = Buffer >> (Level - BitsWanted);

					// Remove unwanted prefix bits
					value &= UInt64.MaxValue >> (64 - BitsWanted);

					// Reduce buffer usage counter
					Level -= BitsWanted;

					// Callback value
					CallbackWrap(value, BitsWanted);
				}
			}
		}

        /// <summary>
        ///     Flush remainder of current byte.
        /// </summary>
        public void FlushByte() {
			Level -= Level % 8;
		}

		private void CallbackWrap(UInt64 value, Int32 count) {
			// Return value
			BitsWanted = Output(value, count);

			// Check sane number of bits requested next
			if (BitsWanted < 0 || BitsWanted > 64) {
				throw new ArgumentOutOfRangeException("BitsWanted must be between 1 and 64, not " + BitsWanted + ".");
			}
		}
	}
}