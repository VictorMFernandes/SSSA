using System;
using System.Collections.Generic;
using System.Linq;

namespace SSSA.Helper.Extensions
{
    public static class IEnumerableExtensions
    {
        public static TSource Higher<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            source.OrderByDescending(keySelector).FirstOrDefault();

        public static TSource Lower<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            source.OrderBy(keySelector).FirstOrDefault();
    }
}
