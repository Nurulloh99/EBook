using System.Collections.Concurrent;
using System.Text;
namespace EBook.Core.Helpers
{
    public delegate TSeed AggregatorDelegate<TSeed, T>(int index, TSeed seed, T current);
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            Guard.ParamNotNull(collection, "collection");
            Guard.ParamNotNull(action, "action");
            foreach (var item in collection)
                action(item);
            return collection;
        }
        public static ICollection<T> ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            Guard.ParamNotNull(collection, "collection");
            Guard.ParamNotNull(action, "action");
            foreach (var item in collection)
                action(item);
            return collection;
        }
        public static T[] ForEach<T>(this T[] collection, Action<T> action)
        {
            Guard.ParamNotNull(collection, "collection");
            Guard.ParamNotNull(action, "action");
            foreach (var item in collection)
                action(item);
            return collection;
        }
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            Guard.ParamNotNull(values, "values");
            if (collection == null)
                throw new NullReferenceException();
            foreach (var item in values)
                collection.Add(item);
        }
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> values, Func<T, T, bool> comparer)
        {
            foreach (var item in values)
            {
                var target = collection.FirstOrDefault(x => values.Any(y => comparer(x, y)));
                if (target != null)
                    collection.Remove(target);
            }
        }
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            foreach (var item in values)
            {
                collection.Remove(item);
            }
        }
        public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            collection.Where(predicate).ToList().Try(x => collection.RemoveRange(x));
        }
        public static void AddIfNotExists<T>(this ICollection<T> collection, T value)
        {
            if (!collection.Contains(value))
                collection.Add(value);
        }
        public static void AddIfNotExists<T>(this ICollection<T> collection, T value, IEqualityComparer<T> comparer)
        {
            if (!collection.Contains(value, comparer))
                collection.Add(value);
        }
        public static void AddIfNotExists<T>(this ICollection<T> collection, T value, Func<T, T, bool> comparer)
        {
            if (!collection.Any(x => comparer(x, value)))
                collection.Add(value);
        }

        public static T GetValueSafe<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key)
        {
            if (key != null && dictionary.ContainsKey(key))
                return dictionary[key];
            return default(T);
        }
        public static int Replace<T>(this IList<T> collection, T value, Func<T, T, bool> equalityPredicate)
        {
            var totalCount = 0;
            for (var i = 0; i < collection.Count; i++)
                if (equalityPredicate(collection[i], value))
                {
                    collection[i] = value;
                    totalCount++;
                }
            return totalCount;
        }
        public static void RemoveAll<T>(this IList<T> collection, Func<T, bool> predicate)
        {
            if (collection == null)
                throw new NullReferenceException();
            Guard.ParamNotNull(predicate, "predicate");
            var i = 0;
            while (i < collection.Count)
                if (predicate(collection[i]))
                    collection.RemoveAt(i);
                else
                    i++;
        }
        public static void AddOrReplace<T>(this IList<T> collection, T value, Func<T, T, bool> equalityPredicate)
        {
            if (!collection.Any(x => equalityPredicate(x, value)))
                collection.Add(value);
            else
                collection.Replace(value, equalityPredicate);
        }
        public static List<T> Distinct<T>(this IEnumerable<T> collection, Func<T, T, bool> comparer)
        {
            var list = new List<T>();
            foreach (var item in collection)
                if (!list.Any(x => comparer(x, item)))
                    list.Add(item);
            return list;
        }
        public static void AddRange<T>(this ConcurrentBag<T> collection, IEnumerable<T> another)
        {
            foreach (var item in another)
                collection.Add(item);
        }

        public static bool AllIsTrue(this IEnumerable<bool> collection)
        {
            return !collection.Any(x => !x);
        }
        public static void AddAllTo<T>(this IEnumerable<T> source, ICollection<T> container)
        {
            if (source == null)
                throw new NullReferenceException();
            Guard.ParamNotNull(container, "container");
            container.AddRange(source);
        }
        public static ICollection<T> Except<T, TSecond>(this IEnumerable<T> minuend, IEnumerable<TSecond> subtrahend, Func<T, TSecond, bool> comparer)
        {
            return minuend.Where(x => !subtrahend.Any(y => comparer(x, y))).ToList();
        }
        public static T TryGetValue<T, TKey>(this ConcurrentDictionary<TKey, T> dictionary, TKey key)
        {
            T value;
            if (dictionary.TryGetValue(key, out value))
                return value;
            return default(T);
        }
        public static ICollection<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var indexes = Enumerable.Range(0, source.Count()).ToList();
            var result = new List<T>();
            var rnd = new Random();
            while (indexes.Any())
            {
                var index = rnd.Next(0, indexes.Count);
                var targetIndex = indexes[index];
                indexes.RemoveAt(index);
                result.Add(source.ElementAt(targetIndex));
            }
            return result;
        }

        public static String Reduce<T>(this IEnumerable<T> source, Func<T, String> selector, String separator)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < source.Count(); i++)
            {
                sb.Append(selector(source.ElementAt(i)));
                if (i < source.Count() - 1)
                    sb.Append(separator);
            }
            return sb.ToString();
        }

        public static ICollection<KeyValuePair<TKey, TValue>> Add<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> collection, TKey key, TValue value)
        {
            if (collection == null)
                throw new NullReferenceException();
            collection.Add(new KeyValuePair<TKey, TValue>(key, value));
            return collection;
        }
    }
}