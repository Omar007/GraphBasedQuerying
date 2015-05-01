using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqStatistics
{
    public static partial class EnumerableStats
    {
        public static decimal? UpperQuartile(this IEnumerable<decimal?> source)
        {
            var values = source.Coalesce();

            if (values.Any())
                return values.UpperQuartile();

            return null;
        }
        public static decimal UpperQuartile(this IEnumerable<decimal> source)
        {
            var median = source.Median();
            return source.Where(x => x > median).Median();
        }
        public static double? UpperQuartile(this IEnumerable<double?> source)
        {
            var values = source.Coalesce();

            if (values.Any())
                return values.UpperQuartile();

            return null;
        }
        public static double UpperQuartile(this IEnumerable<double> source)
        {
            var median = source.Median();
            return source.Where(x => x > median).Median();
        }
        public static float? UpperQuartile(this IEnumerable<float?> source)
        {
            var values = source.Coalesce();

            if (values.Any())
                return values.UpperQuartile();

            return null;
        }
        public static float UpperQuartile(this IEnumerable<float> source)
        {
            var median = source.Median();
            return source.Where(x => x > median).Median();
        }
        public static double? UpperQuartile(this IEnumerable<int?> source)
        {
            var values = source.Coalesce();

            if (values.Any())
                return values.UpperQuartile();

            return null;
        }
        public static double UpperQuartile(this IEnumerable<int> source)
        {
            var median = source.Median();
            return source.Where(x => x > median).Median();
        }
        public static double? UpperQuartile(this IEnumerable<long?> source)
        {
            var values = source.Coalesce();

            if (values.Any())
                return values.UpperQuartile();

            return null;
        }
        public static double UpperQuartile(this IEnumerable<long> source)
        {
            var median = source.Median();
            return source.Where(x => x > median).Median();
        }

        public static decimal? UpperQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select(selector).UpperQuartile();
        }
        public static decimal UpperQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).UpperQuartile();
        }
        public static double? UpperQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select(selector).UpperQuartile();
        }
        public static double UpperQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select(selector).UpperQuartile();
        }
        public static float? UpperQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select(selector).UpperQuartile();
        }
        public static float UpperQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select(selector).UpperQuartile();
        }
        public static double? UpperQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select(selector).UpperQuartile();
        }
        public static double UpperQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).UpperQuartile();
        }
        public static double? UpperQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select(selector).UpperQuartile();
        }
        public static double UpperQuartile<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select(selector).UpperQuartile();
        }
    }
}
