
using System;

namespace Digbyswift.Utils.Extensions
{
	public static class NumericExtensions
	{

		public enum SizeUnit
		{
			Bytes = 0,
			Kilobytes = 1,
			Megabytes = 2
		}

        /// <summary>
        /// This checks that value is in the range, non-inclusively, i.e. greater
        /// than the min value and less than the max value
        /// </summary>
        public static bool IsBetween(this int value, int minValue, int maxValue)
        {
            return (minValue < value && value < maxValue);
        }

        /// <summary>
        /// This checks that value is in the specified range
        /// </summary>
        public static bool IsBetween(this int value, int minValue, int maxValue, bool isInclusive)
        {
            if (!isInclusive)
                return IsBetween(value, minValue, maxValue);

            return (minValue <= value && value <= maxValue);
        }

	}
}