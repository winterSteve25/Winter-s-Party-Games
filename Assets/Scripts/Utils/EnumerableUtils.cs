using System;
using System.Collections.Generic;

namespace Utils
{
    public static class EnumerableUtils
    {
        public static IEnumerable<T> Collect<T>(Func<T> provider, int repeated)
        {
            for (var i = 0; i < repeated; i++)
            {
                yield return provider();
            }
        }
    }
}