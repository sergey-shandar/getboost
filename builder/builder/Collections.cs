using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builder
{
    static class Collections
    {
        public static IEnumerable<T> New<T>(params T[] values)
        {
            return values;
        }

        public static T NewIfNull<T>(this T value)
            where T: class, new()
        {
            return value ?? new T();
        }

        public static string EmptyIfNull(this string value)
        {
            return value ?? string.Empty;
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> value)
        {
            return value ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> OneIfNull<T>(this IEnumerable<T> value)
            where T: new()
        {
            return value ?? new[] { new T() };
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> values)
        {
            var result = new HashSet<T>();
            foreach (var value in values)
            {
                result.Add(value);
            }
            return result;
        }

        public static Split SplitFirst(this string value, char c)
        {
            return new Split(value, value.IndexOf(c));
        }

        public static Value GetOrAddNew<Key, Value>(
            this IDictionary<Key, Value> dictionary, Key key)
            where Value: new()
        {
            Value value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = new Value();
                dictionary.Add(key, value);
            }
            return value;
        }
    }
}
