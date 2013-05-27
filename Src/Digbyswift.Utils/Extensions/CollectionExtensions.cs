using System;
using System.Collections.Generic;
using System.Linq;

namespace Digbyswift.Utils.Extensions
{
	public static class CollectionExtensions
	{

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
		{
			if (value == null)
				return true;

			return !value.Any();
		}

		public static string AsString<T>(this IEnumerable<T> value)
		{
			if (value == null)
				return null;

			return value.AsString(",");
		}

		public static string AsString<T>(this IEnumerable<T> value, string delimiter)
		{
			if (value == null)
				return null;

			return String.Join(delimiter, value.Select(x => x.ToString()).ToArray());
		}

        public static IOrderedEnumerable<string> OrderByNatural(this IEnumerable<string> value)
		{
            return value.OrderBy(item => item, new NaturalComparer(true));
		}

        public static IOrderedEnumerable<string> OrderByNaturalDescending(this IEnumerable<string> value)
		{
			return value.OrderBy(item => item, new NaturalComparer(false));
		}

	}
}