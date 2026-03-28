using System;
using System.Collections.Generic;
using System.Linq;

namespace bnj.utility_toolkit.Runtime
{
    /// <summary>
    /// Helpers for converting between enum values and integers, iterating all values, and creating enum-keyed dictionaries.
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>Converts an <see langword="int"/> to the corresponding <typeparamref name="T"/> enum value.</summary>
        public static T ToEnum<T>(this int value) where T : Enum
        {
            // is this necessary when T is restricted?
            if (!typeof(T).IsEnum) return default(T);

            return (T)Enum.Parse(typeof(T), value.ToString(), true);
        }

        /// <summary>Converts a <typeparamref name="T"/> enum value to its underlying <see langword="int"/>.</summary>
        public static int ToInt<T>(this T value) where T : Enum
        {
            // is this necessary when T is restricted?
            if (!typeof(T).IsEnum) return default;

            return Convert.ToInt32(value);
        }

        /// <summary>Invokes <paramref name="action"/> for every value of <typeparamref name="T"/>.</summary>
        public static void ForEach<T>(Action<T> action) where T : Enum
        {
            foreach (var entry in EnumUtils<T>.AllEnumValues)
                action?.Invoke(entry);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}"/> with one entry per enum value of <typeparamref name="T"/>,
        /// all initialised to <paramref name="defaultValue"/>.
        /// </summary>
        public static Dictionary<T, U> CreateDictionary<T, U>(U defaultValue = default) where T : Enum
        {
            var dictionary = new Dictionary<T, U>();
            ForEach<T>(x => dictionary.Add(x, defaultValue));
            return dictionary;
        }
    }

    /// <summary>
    /// Generic enum utilities with compile-time type parameter.
    /// Provides cached value enumeration and random selection.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    public static class EnumUtils<T> where T : Enum
    {
        /// <summary>All values of <typeparamref name="T"/>.</summary>
        public static IEnumerable<T> AllEnumValues =>
            Enum.GetValues(typeof(T)).Cast<T>();

        /// <summary>A randomly selected value of <typeparamref name="T"/>.</summary>
        public static T RandomEnum =>
            AllEnumValues.GetRandomElement();

        /// <summary>A randomly selected value of <typeparamref name="T"/>, excluding the values in <paramref name="exceptions"/>.</summary>
        public static T RandomEnumWithExceptions(T[] exceptions) =>
            AllEnumValues.Where(x => !exceptions.Contains(x))
                .GetRandomElement();
    }
}