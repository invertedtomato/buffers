using System;
using System.Linq;

namespace InvertedTomato.IO.Bits {
    public static class ByteArrayExtensions {
        /// <summary>
        /// Convert to binary string
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToBinaryString(this byte[] target) {
            if (null == target) {
                throw new ArgumentNullException("target");
            }

            return string.Join(" ", target.Select(a => Convert.ToString(a, 2).PadLeft(8, '0')));
        }
    }
}
