using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<Tuple<T, T, T>> SelectCurrentPreviousNext<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break; // Empty source

                T previous = default;
                T current = enumerator.Current;
                T next;

                while (enumerator.MoveNext())
                {
                    next = enumerator.Current;
                    yield return Tuple.Create(previous, current, next);
                    previous = current;
                    current = next;
                }

                // Handle the last element (no "next" element)
                yield return Tuple.Create(previous, current, default(T));
            }
        }
    }
}
