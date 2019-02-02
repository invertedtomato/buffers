using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace InvertedTomato.IO.Buffers {
	public class ReadOnlyBuffer<T> : IEnumerable<T> {
        /// <summary>
        ///     The underlying buffer array.
        /// </summary>
        protected T[] Underlying;


        /// <summary>
        ///     Create a buffer from a preexisting array.
        /// </summary>
        /// <param name="underlying"></param>
        public ReadOnlyBuffer(T[] underlying) : this(underlying, 0, underlying.Length) { }

        /// <summary>
        ///     Create a buffer from a preexisting array.
        /// </summary>
        /// <param name="underlying"></param>
        /// <param name="count"></param>
        public ReadOnlyBuffer(T[] underlying, Int32 count) : this(underlying, 0, count) { }

        /// <summary>
        ///     Create a buffer from a preexisting array.
        /// </summary>
        /// <param name="underlying"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public ReadOnlyBuffer(T[] underlying, Int32 offset, Int32 count) {
#if DEBUG
			if (null == underlying) {
				throw new ArgumentNullException("value");
			}

			if (offset < 0 || offset > underlying.Length) {
				throw new ArgumentOutOfRangeException("Must be at least 0 and no more than the underlying length.", "offset");
			}

			if (count < 0) {
				throw new ArgumentOutOfRangeException("Must be at least 0.", "count");
			}
#endif

			// Store
			if (count <= underlying.Length) {
				Underlying = underlying;
				Start = offset;
				End = Start + count;
			} else {
				Underlying = new T[count];
				Array.Copy(underlying, offset, Underlying, 0, underlying.Length);
				Start = 0;
				End = underlying.Length;
			}
		}

        /// <summary>
        ///     The position of the first used byte.
        /// </summary>
        public Int32 Start { get; protected set; }

        /// <summary>
        ///     The position of the last used byte.
        /// </summary>
        public Int32 End { get; protected set; }

        /// <summary>
        ///     The number of values in the buffer.
        /// </summary>
        public Int32 Readable => End - Start;

		[Obsolete("Use 'Readable' instead.")] public Int32 Used => Readable;

        /// <summary>
        ///     The number of additional values that could be added to the buffer.
        /// </summary>
        public Int32 Writable => Underlying.Length - End;

		[Obsolete("Use 'Writable' instead.")] public Int32 Available => Writable;

        /// <summary>
        ///     The maximum number of values that could be added under optimal circumstances.
        /// </summary>
        public Int32 Capacity => Underlying.Length;

        /// <summary>
        ///     The maximum number of values that could be added in optimal circumstances.
        /// </summary>
        [Obsolete("Use 'Capacity' instead.")]
		public Int32 MaxCapacity => Capacity;

        /// <summary>
        ///     Is there at least one value available for reading.
        /// </summary>
        public Boolean IsReadable => Readable > 0;

        /// <summary>
        ///     Is there at least one value available for writing.
        /// </summary>
        public Boolean IsWritable => Writable > 0;

        /// <summary>
        ///     Is the buffer unable to accept any additional values.
        /// </summary>
        [Obsolete("Use '!IsWritable' instead.")]
		public Boolean IsFull => Writable == 0;

        /// <summary>
        ///     Is the buffer empty and unable to provide any more values.
        /// </summary>
        [Obsolete("Use '!IsReadable' instead.")]
		public Boolean IsEmpty => Readable == 0;

		IEnumerator<T> IEnumerable<T>.GetEnumerator() {
			return new BufferEnumerator<T>(this);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return new BufferEnumerator<T>(this);
		}


        /// <summary>
        ///     Return the next value from the buffer without incrementing Start.
        /// </summary>
        /// <returns></returns>
        public T Peek() {
#if DEBUG
			if (!IsReadable) {
				throw new BufferOverflowException("Buffer is empty.");
			}
#endif

			return Underlying[Start];
		}

		public ReadOnlyBuffer<T> PeekBuffer(Int32 count) {
#if DEBUG
			if (count < 0) {
				throw new ArgumentOutOfRangeException("Count must be at least 0.");
			}

			if (Start + count > End) {
				throw new BufferOverflowException("Insufficient values in buffer.");
			}
#endif

			return new ReadOnlyBuffer<T>(Underlying, Start, count);
		}

        /// <summary>
        ///     Get the next value from the buffer using try pattern, without moving Start.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public Boolean TryPeek(out T output) {
			if (IsReadable) {
				output = Underlying[Start];
				return true;
			}

			output = default(T);
			return false;
		}

        /// <summary>
        ///     Return a specific value from the buffer without changing Start.
        /// </summary>
        /// <param name="position">Offset from Start.</param>
        /// <returns></returns>
        public T Peek(Int32 position) {
#if DEBUG
			if (position < 0 || position >= Readable) {
				throw new BufferOverflowException("No value in given position.");
			}
#endif

			return Underlying[Start + position];
		}

        /// <summary>
        ///     Return a resized copy of the buffer.
        /// </summary>
        /// <param name="maxCapacity"></param>
        /// <returns></returns>
        [Obsolete("Use AutoGrow or Trim instead.")]
		public Buffer<T> Resize(Int32 maxCapacity) {
			return Trim(maxCapacity);
		}

        /// <summary>
        ///     Create a new trimmed buffer with read values remove, and with a specified number of writable slots available.
        /// </summary>
        public Buffer<T> Trim(Int32 writable) {
			// Check capacity is sufficient
			if (writable < Readable) {
				throw new BufferOverflowException("Length is smaller than the number of used bytes (" + Readable + ").");
			}

			// Create new underlying
			var underlying = new T[writable];
			Array.Copy(Underlying, Start, underlying, 0, Readable);

			// Return new buffer
			return new Buffer<T>(underlying, Readable);
		}

        /// <summary>
        ///     Return the underlying buffer array. USE WITH CAUTION.
        /// </summary>
        /// <returns></returns>
        public T[] GetUnderlying() {
			return Underlying;
		}

        /// <summary>
        ///     Extract data as array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray() {
			var underlying = new T[Readable];
			Array.Copy(Underlying, Start, underlying, 0, Readable);

			return underlying;
		}

		public override String ToString() {
			var byteArray = Underlying as Byte[];
			if (null != byteArray) {
				return BitConverter.ToString(byteArray, Start, Readable);
			}

			var sb = new StringBuilder();
			for (var i = Start; i < End; i++) {
				if (sb.Length > 0) {
					sb.Append("-");
				}

				sb.Append(Underlying[i]);
			}

			return sb.ToString();
		}
	}
}