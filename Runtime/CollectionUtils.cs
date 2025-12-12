using System.Collections.Generic;
using System.Linq;

namespace bnj.utility_toolkit.Runtime
{
    public static class CollectionUtils
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection) =>
            collection.OrderBy(x => RandomUtils.RandomInt);

        public static T GetRandomElement<T>(this IEnumerable<T> collection) =>
            collection.Shuffle().FirstOrDefault();

        public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> collection, int amount) =>
            collection.Shuffle().Take(amount);
    }
}