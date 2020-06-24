using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System
{
    public static class HalfExtensions
    {
        public static Half ReadHalf(this BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            return Half.ToHalf(reader.ReadBytes(2), 0);
        }

        public static void Write(this BinaryWriter writer, Half value)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            writer.Write(Half.GetBytes(value));
        }

        public static void Write(this TextWriter writer, Half value)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            writer.Write(value);
        }

        public static void WriteLine(this TextWriter writer, Half value)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            writer.WriteLine(value);
        }

        public static Half Average(this IEnumerable<Half> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            var count = 0;
            var total = (Half)0f;
            foreach (var value in source)
            {
                total += value;
                count++;
            }
            if (count == 0) throw new InvalidOperationException();
            return total / count;
        }

        public static Half? Average(this IEnumerable<Half?> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            var count = 0;
            var total = (Half)0;
            foreach (var value in source)
            {
                if (!value.HasValue) continue;

                total += value.Value;
                count++;
            }
            if (count == 0) return null;
            return total / count;
        }

        public static Half Average<TSource>(this IEnumerable<TSource> source,
                                            Func<TSource, Half> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            var count = 0;
            var total = (Half)0f;
            foreach (var value in source)
            {
                total += selector(value);
                count++;
            }
            if (count == 0) throw new InvalidOperationException();
            return total / count;
        }

        public static Half? Average<TSource>(this IEnumerable<TSource> source,
                                             Func<TSource, Half?> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            var count = 0;
            var total = (Half)0;
            foreach (var tValue in source)
            {
                var value = selector(tValue);
                if (!value.HasValue) continue;

                total += value.Value;
                count++;
            }
            if (count == 0) return null;
            return total / count;
        }

        public static Half Max(this IEnumerable<Half> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            var result = (Half?)null;
            foreach (var value in source)
                if (!result.HasValue ||
                    result < value)
                    result = value;

            if (result.HasValue) return result.Value;
            throw new InvalidOperationException();
        }

        public static Half? Max(this IEnumerable<Half?> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            var result = (Half?)null;
            foreach (var value in source)
                if (!result.HasValue ||
                    (value.HasValue &&
                     result < value))
                    result = value;

            return result;
        }

        public static Half Max<TSource>(this IEnumerable<TSource> source,
                                        Func<TSource, Half> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            var result = (Half?)null;
            foreach (var value in source)
            {
                var selection = selector(value);
                if (!result.HasValue ||
                    result < selection)
                    result = selection;
            }

            if (result.HasValue) return result.Value;
            throw new InvalidOperationException();
        }

        public static Half? Max<TSource>(this IEnumerable<TSource> source,
                                        Func<TSource, Half?> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            var result = (Half?)null;
            foreach (var value in source)
            {
                var selection = selector(value);
                if (!result.HasValue ||
                    (selection.HasValue &&
                     result < selection))
                    result = selection;
            }

            return result;
        }

        public static Half Min(this IEnumerable<Half> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            var result = (Half?)null;
            foreach (var value in source)
                if (!result.HasValue ||
                    result > value)
                    result = value;

            if (result.HasValue) return result.Value;
            throw new InvalidOperationException();
        }

        public static Half? Min(this IEnumerable<Half?> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            var result = (Half?)null;
            foreach (var value in source)
                if (!result.HasValue ||
                    (value.HasValue &&
                     result > value))
                    result = value;

            return result;
        }

        public static Half Min<TSource>(this IEnumerable<TSource> source,
                                        Func<TSource, Half> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            var result = (Half?)null;
            foreach (var value in source)
            {
                var selection = selector(value);
                if (!result.HasValue ||
                    result > selection)
                    result = selection;
            }

            if (result.HasValue) return result.Value;
            throw new InvalidOperationException();
        }

        public static Half? Min<TSource>(this IEnumerable<TSource> source,
                                        Func<TSource, Half?> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            var result = (Half?)null;
            foreach (var value in source)
            {
                var selection = selector(value);
                if (!result.HasValue ||
                    (selection.HasValue &&
                     result > selection))
                    result = selection;
            }

            return result;
        }

        public static Half Sum(this IEnumerable<Half> source) =>
            source.Aggregate((result, value) => result += value);

        public static Half? Sum(this IEnumerable<Half?> source) =>
            source.Aggregate((result, value) =>
                (result + value) ?? result ?? value);

        public static Half Sum<TSource>(this IEnumerable<TSource> source,
                                        Func<TSource, Half> selector) =>
            source.Aggregate((Half)0, (result, value) =>
                result += selector(value));

        public static Half? Sum<TSource>(this IEnumerable<TSource> source,
                                         Func<TSource, Half?> selector) =>
            source.Aggregate((Half?)0, (result, value) =>
            {
                var selection = selector(value);
                return (result + selection) ?? result ?? selection;
            });
    }
}
