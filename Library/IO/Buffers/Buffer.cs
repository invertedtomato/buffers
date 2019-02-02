using System;

namespace InvertedTomato.IO.Buffers {
	public class Buffer<T> : ReadOnlyBuffer<T> {
		private const Double GrowthRate = 0.5;

        /// <summary>
        ///     Create a new buffer initialized to the given length.
        /// </summary>
        /// <param name="capacity"></param>
        public Buffer(Int32 capacity) : base(new T[capacity], 0, 0) {
#if DEBUG
			if (capacity < 0) {
				throw new ArgumentOutOfRangeException("Must be at least 0.", "maxCapacity");
			}
#endif
		}

        /// <summary>
        ///     Create a buffer from a preexisting array.
        /// </summary>
        /// <param name="underlying"></param>
        public Buffer(T[] underlying) : base(underlying, 0, underlying.Length) { }

        /// <summary>
        ///     Create a buffer from a preexisting array.
        /// </summary>
        /// <param name="underlying"></param>
        /// <param name="count"></param>
        public Buffer(T[] underlying, Int32 count) : base(underlying, 0, count) { }

        /// <summary>
        ///     Create a buffer from a preexisting array.
        /// </summary>
        /// <param name="underlying"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public Buffer(T[] underlying, Int32 offset, Int32 count) : base(underlying, offset, count) { }

        /// <summary>
        ///     Automatically resize when the buffer has insufficent capacity.
        /// </summary>
        public Boolean AutoGrow { get; set; }

        /// <summary>
        ///     An index within the current value that is currently in use (ignorable).
        /// </summary>
        public Int32 SubOffset { get; set; }

        /// <summary>
        ///     Add a value to the buffer and increment End.
        /// </summary>
        /// <param name="value"></param>
        public void Enqueue(T value) {
			if (Writable < 1) {
				if (AutoGrow) {
					GrowExponential(1);
				} else {
					throw new BufferOverflowException("Insufficient space in buffer. " + Writable + " available, but " + 1 + " needed. Consider enabling auto-grow.");
				}
			}

			Underlying[End++] = value;
		}

        /// <summary>
        ///     Append a new buffer to this buffer and increment End.
        /// </summary>
        /// <param name="buffer"></param>
        public void EnqueueBuffer(ReadOnlyBuffer<T> buffer) {
#if DEBUG
			if (null == buffer) {
				throw new ArgumentNullException("buffer");
			}
#endif
			if (buffer.Readable > Writable) {
				if (AutoGrow) {
					GrowExponential(buffer.Readable);
				} else {
					throw new BufferOverflowException("Insufficient space in buffer. " + Writable + " available, but " + buffer.Readable + " needed. Consider enabling auto-grow.");
				}
			}

			Array.Copy(buffer.GetUnderlying(), buffer.Start, Underlying, End, buffer.Readable);
			End += buffer.Readable;
		}

        /// <summary>
        ///     Append an array of values.
        /// </summary>
        /// <param name="values"></param>
        public void EnqueueArray(T[] values, Int32 offset = 0, Int32 count = -1) {
#if DEBUG
			if (null == values) {
				throw new ArgumentNullException("buffer");
			}

			if (offset < 0 || offset > values.Length) {
				throw new ArgumentOutOfRangeException("Must be at least 0 and no more than the underlying length.", "offset");
			}

			if (count < -1) {
				throw new ArgumentOutOfRangeException("Must be at least 0.", "count");
			}
#endif

			if (values.Length > Writable) {
				if (AutoGrow) {
					GrowExponential(values.Length);
				} else {
					throw new BufferOverflowException("Insufficient space in buffer. " + Writable + " available, but " + values.Length + " needed. Consider enabling auto-grow.");
				}
			}

			if (count == -1) {
				count = values.Length - offset;
			}

			Array.Copy(values, offset, Underlying, End, count);
			End += count;
		}

        /// <summary>
        ///     Replace a value.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Replace(Int32 index, T value) {
#if DEBUG
			if (index < Start || index >= End) {
				throw new IndexOutOfRangeException("Index outside of used range (" + Start + " to " + End + ").");
			}
#endif

			Underlying[index] = value;
		}

        /// <summary>
        ///     Return the next value from the buffer and increment Start.
        /// </summary>
        /// <returns></returns>
        public T Dequeue() {
#if DEBUG
			if (!IsReadable) {
				throw new BufferOverflowException("Buffer is empty.");
			}
#endif

			return Underlying[Start++];
		}

        /// <summary>
        ///     Dequeue a number of values as a buffer.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Buffer<T> DequeueBuffer(Int32 count) {
#if DEBUG
			if (count < 0) {
				throw new ArgumentOutOfRangeException("Must be at least 0.");
			}

			if (Readable < count) {
				throw new BufferOverflowException("Buffer does not contain requested number of values (requested: " + count + ", used: " + Readable + ").");
			}
#endif

			var ret = new Buffer<T>(Underlying, Start, count);
			Start += count;

			return ret;
		}

        /// <summary>
        ///     Dequeue a number of values as an array.
        /// </summary>
        public T[] DequeueArray(Int32 count) {
			return DequeueBuffer(count).ToArray();
		}

        /// <summary>
        ///     Get the next value from the buffer using try pattern.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public Boolean TryDequeue(out T output) {
			if (IsReadable) {
				output = Underlying[Start++];
				return true;
			}

			output = default(T);
			return false;
		}

        /// <summary>
        ///     Reset the pointers ready for buffer re-use.
        /// </summary>
        /// <returns></returns>
        public void Reset() {
			SubOffset = 0;
			Start = 0;
			End = 0;
		}

        /// <summary>
        ///     Move the start pointer.
        /// </summary>
        public void MoveStart(Int32 offset) {
			// Calculate the proposed new location
			var pos = Start + offset;

#if DEBUG
			if (pos < 0 || pos > End) {
				throw new OverflowException("New start would be " + pos + " which is out of range.");
			}
#endif
			// Update position
			Start = pos;
		}

        /// <summary>
        ///     Increment end by given offset.
        /// </summary>
        /// <param name="offset"></param>
        public void MoveEnd(Int32 offset) {
			var pos = End + offset;
#if DEBUG
			if (pos < Start || pos > Capacity) {
				throw new OverflowException("New end would be " + pos + " which is out of range.");
			}
#endif

			// Update position
			End = pos;
		}

		public ReadOnlyBuffer<T> AsReadOnly() {
			return this;
		}

        /// <summary>
        ///     Increases the number of writable slots exponentially, with the option of specifying a absolute minimum increase.
        /// </summary>
        public void GrowExponential(Int32 min = 0) {
			// Calculate new size
			var increase = (Int32) Math.Max(Underlying.Length * GrowthRate, min);

			// Grow
			Grow(increase);
		}

        /// <summary>
        ///     Increase the number of writable slots by a given value.
        /// </summary>
        public void Grow(Int32 increase) {
#if DEBUG
			if (increase < 0) {
				throw new ArgumentException("Must be at least 0.", "increase");
			}
#endif

			// Create new underlying
			var underlying = new T[Underlying.Length + increase];

			// Copy all values (not just used ones, in case someone moves a pointer backwards)
			Array.Copy(Underlying, 0, underlying, 0, Underlying.Length);

			// Replace underlying
			Underlying = underlying;
		}
	}
}