using System.Collections.Generic;
using System.Linq;

namespace bnj.utility_toolkit.Runtime
{
    /// <summary>Extension methods on <see cref="IEnumerable{T}"/> for shuffling and random element selection.</summary>
    public static class CollectionUtils
    {
        /// <summary>Returns the collection in a randomised order.</summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection) =>
            collection.OrderBy(x => RandomUtils.RandomInt);

        /// <summary>Returns a single randomly selected element, or <see langword="default"/> if the collection is empty.</summary>
        public static T GetRandomElement<T>(this IEnumerable<T> collection) =>
            collection.Shuffle().FirstOrDefault();

        /// <summary>Returns <paramref name="amount"/> randomly selected elements.</summary>
        public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> collection, int amount) =>
            collection.Shuffle().Take(amount);
    }
}