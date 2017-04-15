﻿using System;

namespace InvertedTomato.IO.Buffers {
    public class Buffer<T> : ReadOnlyBuffer<T> {
        /// <summary>
        /// An index within the current value that is currently in use (ignorable).
        /// </summary>
        public int SubOffset { get; protected set; }

        /// <summary>
        /// Create a new buffer initialized to the given length.
        /// </summary>
        /// <param name="maxCapacity"></param>
        public Buffer(int maxCapacity) : base(new T[maxCapacity], 0, 0) {
#if DEBUG
            if (maxCapacity < 0) {
                throw new ArgumentOutOfRangeException("Must be at least 0.", "maxCapacity");
            }
#endif
        }

        /// <summary>
        /// Create a buffer from a preexisting array.
        /// </summary>
        /// <param name="underlying"></param>
        public Buffer(T[] underlying) : base(underlying, 0, underlying.Length) { }

        /// <summary>
        /// Create a buffer from a preexisting array.
        /// </summary>
        /// <param name="underlying"></param>
        /// <param name="count"></param>
        public Buffer(T[] underlying, int count) : base(underlying, 0, count) { }

        /// <summary>
        /// Create a buffer from a preexisting array.
        /// </summary>
        /// <param name="underlying"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public Buffer(T[] underlying, int offset, int count) : base(underlying, offset, count) { }

        /// <summary>
        /// Add a value to the buffer and increment End.
        /// </summary>
        /// <param name="value"></param>
        public void Enqueue(T value) {
#if DEBUG
            if (IsFull) {
                throw new BufferOverflowException("Buffer is already full.");
            }
#endif

            Underlying[End++] = value;
        }

        /// <summary>
        /// Append a new buffer to this buffer and increment End.
        /// </summary>
        /// <param name="buffer"></param>
        public void EnqueueBuffer(ReadOnlyBuffer<T> buffer) {
#if DEBUG
            if (null == buffer) {
                throw new ArgumentNullException("buffer");
            }
            if (buffer.Used > Available) {
                throw new BufferOverflowException("Insufficient space in buffer. " + Available + " available, but " + buffer.Used + " needed.");
            }
#endif

            Array.Copy(buffer.GetUnderlying(), buffer.Start, Underlying, End, buffer.Used);
            End += buffer.Used;
        }

        /// <summary>
        /// Append an array of values.
        /// </summary>
        /// <param name="values"></param>
        public void EnqueueArray(T[] values) {
#if DEBUG
            if (null == values) {
                throw new ArgumentNullException("buffer");
            }
            if (values.Length > Available) {
                throw new BufferOverflowException("Insufficient space in buffer. " + Available + " available, but " + values.Length + " needed.");
            }
#endif

            Array.Copy(values, 0, Underlying, End, values.Length);
            End += values.Length;
        }

        /// <summary>
        /// Replace a value.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Replace(int index, T value) {
#if DEBUG
            if (index < Start || index >= End) {
                throw new IndexOutOfRangeException("Index outside of used range (" + Start + " to " + End + ").");
            }
#endif

            Underlying[index] = value;
        }

        /// <summary>
        /// Return the next value from the buffer and increment Start.
        /// </summary>
        /// <returns></returns>
        public T Dequeue() {
#if DEBUG
            if (IsEmpty) {
                throw new BufferOverflowException("Buffer is empty.");
            }
#endif
            
            return Underlying[Start++];
        }

        /// <summary>
        /// Dequeue a number of values as a buffer.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Buffer<T> DequeueBuffer(int count) {
#if DEBUG
            if (count < 0) {
                throw new ArgumentOutOfRangeException("Must be at least 0.");
            }
            if (Used < count) {
                throw new BufferOverflowException("Buffer does not contain requested number of values (requested: " + count + ", used: " + Used + ").");
            }
#endif
            
            var ret = new Buffer<T>(Underlying, Start, count);
            Start += count;

            return ret;
        }

        /// <summary>
        /// Get the next value from the buffer using try pattern.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool TryDequeue(out T output) {
            if (IsEmpty) {
                output = default(T);
                return false;
            } else {
                output = Underlying[Start++];
                return true;
            }
        }

        /// <summary>
        /// Reset the pointers ready for buffer re-use.
        /// </summary>
        /// <returns></returns>
        public void Reset() {
            SubOffset = 0;
            Start = 0;
            End = 0;
        }

        /// <summary>
        /// Move the start pointer.
        /// </summary>
        public void MoveStart(int offset) {
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
        /// Increment end by given offset. 
        /// </summary>
        /// <param name="offset"></param>
        public void MoveEnd(int offset) {
            var pos = End + offset;
#if DEBUG
            if (pos < Start || pos > MaxCapacity) {
                throw new OverflowException("New end would be " + pos + " which is out of range.");
            }
#endif

            // Update position
            End = pos;
        }

        /// <summary>
        /// Increment bit by one.
        /// </summary>
        public void IncrementSubOffset() { IncrementSubOffset(1); }

        /// <summary>
        /// Increment bit by given offset.
        /// </summary>
        /// <param name="offset"></param>
        public void IncrementSubOffset(int offset) {
            var bit = offset + SubOffset;
#if DEBUG
            if (offset < 0) {
                throw new ArgumentOutOfRangeException("offset", "Must be at least 0.");
            }
#endif

            SubOffset = bit;
        }

        public ReadOnlyBuffer<T> AsReadOnly() {
            return this;
        }
    }
}
