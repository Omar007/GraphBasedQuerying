using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqStatistics
{
    public static partial class EnumerableStats
    {
        public static decimal? LowerQuartile(this IEnumerable<decimal?> source)
        {
            var values = source.Coalesce();

            if (values.Any())
                return values.LowerQuartile();

            return null;
        }
        public static decimal LowerQuartile(this IEnumerable<decimal> source)
        {
            var median = source.Median();
            return source.Where(x => x < median).Median();
        }
        public static double? LowerQuartile(this IEnumerable<double?> source)
        {
            var values = source.Coalesce();

            if (values.Any())
                return values.LowerQuartile();

            return null;
        }
        public static double LowerQuartile(this IEnumerable<double> source)
        {
            var median = source.Median();
            return source.Where(x => x < median).Median();
        }
        public static float? LowerQuartile(this IEnumerable<float?> source)
        {
            var values = source.Coalesce();

            if (values.Any())
                return values.LowerQuartile();

            return null;
        }
        public static float LowerQuartile(this IEnumerable<float> source)
        {
            var median = source.Median();
            return source.Where(x => x < median).Median();
        }
        public static double? LowerQuartile(this IEnumerable<int?> source)
        {
            var values = source.Coalesce();

            if (values.Any())
                return values.LowerQuartile();

            return null;
        }
        public static double LowerQuartile(this IEnumerable<int> source)
        {
            var median = source.Median();
            return source.Where(x => x < median).Median();
        }
        public static double? LowerQuartile(this IEnumerable<long?> source)
        {
            var values = source.Coalesce();

            if (values.Any())
                return values.LowerQuartile();

            return null;
        }
        public static double LowerQuartile(this IEnumerable<long> source)
        {
            var median = source.Median();
            return source.Where(x => x < median).Median();
        }
        
        public static decimal? LowerQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select(selector).LowerQuartile();
        }
        public static decimal LowerQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).LowerQuartile();
        }
        public static double? LowerQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select(selector).LowerQuartile();
        }
        public static double LowerQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select(selector).LowerQuartile();
        }
        public static float? LowerQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select(selector).LowerQuartile();
        }
        public static float LowerQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select(selector).LowerQuartile();
        }
        public static double? LowerQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select(selector).LowerQuartile();
        }
        public static double LowerQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).LowerQuartile();
        }
        public static double? LowerQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select(selector).LowerQuartile();
        }
        public static double LowerQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select(selector).LowerQuartile();
        }
    }
}
