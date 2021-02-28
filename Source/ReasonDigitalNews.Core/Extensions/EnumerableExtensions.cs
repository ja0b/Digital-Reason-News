using System.Collections.Generic;
using System.Linq;

namespace ReasonDigitalNews.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool HasAny<T>(this IEnumerable<T> items)
            => items != null && items.Any();
    }
}