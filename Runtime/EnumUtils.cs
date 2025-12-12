using System;
using System.Collections.Generic;
using System.Linq;

namespace bnj.utility_toolkit.Runtime
{
    public static class EnumUtils
    {
        public static T ToEnum<T>(this int value) where T : Enum
        {
            // is this necessary when T is restricted?
            if (!typeof(T).IsEnum) return default(T);

            return (T)Enum.Parse(typeof(T), value.ToString(), true);
        }

        public static int ToInt<T>(this T value) where T : Enum
        {
            // is this necessary when T is restricted?
            if (!typeof(T).IsEnum) return default;

            return Convert.ToInt32(value);
        }

        public static void ForEach<T>(Action<T> action) where T : Enum
        {
            foreach (var entry in EnumUtils<T>.AllEnumValues)
                action?.Invoke(entry);
        }

        public static Dictionary<T, U> CreateDictionary<T, U>(U defaultValue = default) where T : Enum
        {
            var dictionary = new Dictionary<T, U>();
            ForEach<T>(x => dictionary.Add(x, defaultValue));
            return dictionary;
        }
    }

    public static class EnumUtils<T> where T : Enum
    {
        public static IEnumerable<T> AllEnumValues =>
            Enum.GetValues(typeof(T)).Cast<T>();

        public static T RandomEnum =>
            AllEnumValues.GetRandomElement();

        public static T RandomEnumWithExceptions(T[] exceptions) =>
            AllEnumValues.Where(x => !exceptions.Contains(x))
                .GetRandomElement();
    }
}