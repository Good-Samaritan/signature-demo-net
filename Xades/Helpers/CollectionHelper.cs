using System.Collections;
using System.Linq;

namespace Xades.Helpers
{
    public static class CollectionHelper
    {
        public static bool IsNotEmpty(this IEnumerable enumerable)
        {
            return enumerable != null && enumerable.OfType<object>().Any();
        }
    }
}