using System.Collections.Generic;
using System.Linq;

namespace Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> iEnumerable)
        {
            return iEnumerable == null || iEnumerable.Any() == false;
        }
    }
}
