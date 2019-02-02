using System;
using System.Collections;
using System.Collections.Generic;

namespace InvertedTomato.IO.Buffers {
	public class BufferEnumerator<T> : IEnumerator<T> {
		private readonly ReadOnlyBuffer<T> Buffer;
		private Int32 Position = -1;

		public BufferEnumerator(ReadOnlyBuffer<T> buffer) {
#if DEBUG
			if (null == buffer) {
				throw new ArgumentNullException("buffer");
			}
#endif

			// Store
			Buffer = buffer;
		}

		public Boolean IsDisposed { get; private set; }

		public T Current {
			get {
#if DEBUG
				if (Position == -1) {
					throw new InvalidOperationException("Not yet moved.");
				}

				if (Position > Buffer.Used) {
					throw new InvalidOperationException("Moved beyond end of set.");
				}
#endif
				return Buffer.Peek(Position);
			}
		}

		Object IEnumerator.Current {
			get {
#if DEBUG
				if (Position == -1) {
					throw new InvalidOperationException("Not yet moved.");
				}

				if (Position > Buffer.Used) {
					throw new InvalidOperationException("Moved beyond end of set.");
				}
#endif
				return Buffer.Peek(Position);
			}
		}

		public Boolean MoveNext() {
			return ++Position < Buffer.Used;
		}

		public void Reset() {
			Position = -1;
		}

		public void Dispose() {
			Dispose(true);
		}

		protected virtual void Dispose(Boolean disposing) {
			if (IsDisposed) {
				return;
			}

			IsDisposed = true;

			if (disposing) {
				// Dispose managed state (managed objects).
			}
		}
	}
}