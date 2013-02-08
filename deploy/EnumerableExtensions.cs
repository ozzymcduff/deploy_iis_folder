using System;
using System.Collections.Generic;
using System.Linq;

namespace deploy
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return null==enumerable || !enumerable.Any();
        }
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var obj in enumerable)
            {
                action(obj);
            }
        }
    }
}