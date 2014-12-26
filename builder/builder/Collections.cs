using System.Collections.Generic;
using Framework.G1;

namespace builder
{
    static class Collections
    {
        public static IEnumerable<T> OneIfAbsent<T>(
            this Optional.Class<IEnumerable<T>> value)
            where T: new()
        {
            return value.Cast().Select(v => v, () => new[] { new T() });
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
