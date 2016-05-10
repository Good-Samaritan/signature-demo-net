using System;

namespace Xades.Helpers
{
    public static class ArrayHelper
    {
        public static bool AreEquals(byte[] first, byte[] second)
        {
            return AreEquals(first, second, (x, y) => x.Equals(y));
        }

        public static bool AreEquals(string[] first, string[] second)
        {
            return AreEquals(first, second, (x, y) => string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase));
        }

        private static bool AreEquals<T>(T[] first, T[] second, Func<T, T, bool> comparator)
        {
            if (first == second)
            {
                return true;
            }
            if (first.Length != second.Length)
            {
                return false;
            }
            for (var i = 0; i < first.Length; i++)
            {
                var equals = comparator.Invoke(first[i], second[i]);
                if (!equals)
                {
                    return false;
                }
            }
            return true;
        }
    }
}