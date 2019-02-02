using System;
using System.IO;

namespace InvertedTomato.IO.Buffers {
	public static class StreamExtensions {
		public static void Write(this Stream target, ReadOnlyBuffer<Byte> buffer) {
#if DEBUG
			if (null == target) {
				throw new ArgumentNullException("target");
			}

			if (null == buffer) {
				throw new ArgumentNullException("buffer");
			}
#endif

			target.Write(buffer.GetUnderlying(), buffer.Start, buffer.Used);
		}

		public static Int32 Read(this Stream target, Buffer<Byte> buffer) {
			return target.Read(buffer, Int32.MaxValue);
		}

		public static Int32 Read(this Stream target, Buffer<Byte> buffer, Int32 count) {
#if DEBUG
			if (null == target) {
				throw new ArgumentNullException("target");
			}

			if (null == buffer) {
				throw new ArgumentNullException("buffer");
			}

			if (count < 0) {
				throw new ArgumentOutOfRangeException("count", "Must be at least 0.");
			}
#endif

			if (buffer.Writable < count) {
				count = buffer.Writable;
			}

			// Read into buffer
			var length = target.Read(buffer.GetUnderlying(), buffer.End, count);

			// Increment buffer length
			buffer.MoveEnd(length);

			// Return number of bytes read
			return length;
		}

		/*
		public static IAsyncResult BeginRead(this IStream target, Buffer<byte> buffer, AsyncCallback callback, object state) {
#if DEBUG
		    if (null == target) {
		        throw new ArgumentNullException("target");
		    }
		    if (null == buffer) {
		        throw new ArgumentNullException("buffer");
		    }
#endif

		    return target.BeginRead(buffer.GetUnderlying(), buffer.End, buffer.Available, callback, state);
		}

		public static IAsyncResult BeginRead(this IStream target, Buffer<byte> buffer, int maxCount, AsyncCallback callback, object state) {
#if DEBUG
		    if (null == target) {
		        throw new ArgumentNullException("target");
		    }
		    if (null == buffer) {
		        throw new ArgumentNullException("buffer");
		    }
#endif

		    return target.BeginRead(buffer.GetUnderlying(), buffer.End, Math.Min(buffer.Available, maxCount), callback, state);
		}*/
	}
}