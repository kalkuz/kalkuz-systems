using System;
using System.Collections.Generic;

namespace KalkuzSystems.Utility.Generics
{
    public static class LinqExtensions
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof (source));
            if (predicate == null) throw new ArgumentNullException(nameof (predicate));
            
            foreach (TSource item in source)
            {
                predicate.Invoke(item);
            }
        }
    }
}