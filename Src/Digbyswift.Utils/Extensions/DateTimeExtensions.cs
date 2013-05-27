using System;

namespace Digbyswift.Utils.Extensions
{

	public static class DateTimeExtensions
	{

		#region Methods: Add/subtract dates

		/// <summary>
        /// Returns <see cref="DateTime"/> increased by 24 hours ie Next Day.
        /// </summary>
        public static DateTime AddDay(this DateTime start)
        {
            return start.AddDays(1);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> decreased by 24h period ie Previous Day.
        /// </summary>
        public static DateTime SubtractDay(this DateTime start)
        {
            return start.AddDays(-1);
        }

		/// <summary>
		/// Adds a week (seven days) to the supplied <see cref="DateTime"/>
		/// </summary>
		public static DateTime AddWeek(this DateTime start)
		{
			return start.AddDays(7);
		}

		/// <summary>
		/// Adds a number of weeks to the supplied <see cref="DateTime"/>
		/// </summary>
		public static DateTime AddWeeks(this DateTime start, double value)
		{
			return start.AddDays(7 * value);
		}

		/// <summary>
        /// Returns first next occurrence of specified <see cref="DayOfWeek"/>.
        /// </summary>
        public static DateTime Next(this DateTime start, DayOfWeek day)
        {
            do
            {
                start = start.AddDay();
            }
            while (start.DayOfWeek != day);

            return start;
        }

        /// <summary>
        /// Returns first next occurrence of specified <see cref="DayOfWeek"/>.
        /// </summary>
        public static DateTime Previous(this DateTime start, DayOfWeek day)
        {
            do
            {
                start = start.SubtractDay();
            }
            while (start.DayOfWeek != day);

            return start;
        }

		/// <summary>
		/// Returns the next day of the supplied month, or if this has
		/// passed, the day in the next month. If the day (e.g. 31st doesn't
		/// exist, an ArgumentOutOfRangeException will be thrown)
		/// </summary>
		public static DateTime NextDayOfMonth(this DateTime start, int dayOfMonth)
		{
			if (start.Day < dayOfMonth)
			{
				if (DateTime.DaysInMonth(start.Year, start.Month) < dayOfMonth)
					throw new ArgumentOutOfRangeException("dayOfMonth");

				start.SetDay(dayOfMonth);
			}

			DateTime nextMonth = start.AddMonths(1);

			if (DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month) < dayOfMonth)
				throw new ArgumentOutOfRangeException("dayOfMonth");

			return nextMonth.SetDay(dayOfMonth);
		}

		public static bool TryNextDayOfMonth(this DateTime start, int dayOfMonth, out DateTime returnDate)
		{
			returnDate = DateTime.MinValue;

			if (start.Day < dayOfMonth)
			{
				if (DateTime.DaysInMonth(start.Year, start.Month) < dayOfMonth)
					return false;

				returnDate = start.SetDay(dayOfMonth);
				return true;
			}

			DateTime nextMonth = start.AddMonths(1);

			if (DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month) < dayOfMonth)
				return false;

			returnDate = nextMonth.SetDay(dayOfMonth);
			return true;
		}

		/// <summary>
		/// Sets the day of the <see cref="DateTime"/> to the first day in that month.
		/// </summary>
		/// <param name="current">The current <see cref="DateTime"/> to be changed.</param>
		/// <returns>given <see cref="DateTime"/> with the day part set to the first day in that month.</returns>
		public static DateTime FirstDayOfMonth(this DateTime current)
		{
			return current.SetDay(1);
		}

		/// <summary>
		/// Sets the day of the <see cref="DateTime"/> to the last day in that month.
		/// </summary>
		/// <param name="current">The current DateTime to be changed.</param>
		/// <returns>given <see cref="DateTime"/> with the day part set to the last day in that month.</returns>
		public static DateTime LastDayOfMonth(this DateTime current)
		{
			return current.SetDay(DateTime.DaysInMonth(current.Year, current.Month));
		}

		/// <summary>
		/// Adds the given number of business days to the <see cref="DateTime"/>.
		/// </summary>
		/// <param name="current">The date to be changed.</param>
		/// <param name="days">Number of business days to be added.</param>
		/// <returns>A <see cref="DateTime"/> increased by a given number of business days.</returns>
		public static DateTime AddBusinessDays(this DateTime current, int days)
		{
			var sign = Math.Sign(days);
			var unsignedDays = Math.Abs(days);
			for (var i = 0; i < unsignedDays; i++)
			{
				do
				{
					current = current.AddDays(sign);
				}
				while (current.DayOfWeek == DayOfWeek.Saturday ||
					   current.DayOfWeek == DayOfWeek.Sunday);
			}
			return current;
		}

		/// <summary>
		/// Subtracts the given number of business days to the <see cref="DateTime"/>.
		/// </summary>
		/// <param name="current">The date to be changed.</param>
		/// <param name="days">Number of business days to be subtracted.</param>
		/// <returns>A <see cref="DateTime"/> increased by a given number of business days.</returns>
		public static DateTime SubtractBusinessDays(this DateTime current, int days)
		{
			return AddBusinessDays(current, -days);
		}

		/// <summary>
        /// Returns the next day with zero time part.
        /// </summary>
        public static DateTime Midnight(this DateTime value)
        {
            return value.Date.AddDay();
        }

        /// <summary>
        /// Returns original <see cref="DateTime"/> value with time part set to Noon (12:00:00h).
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> find Noon for.</param>
        /// <returns>A <see cref="DateTime"/> value with time part set to Noon (12:00:00h).</returns>
        public static DateTime Noon(this DateTime value)
        {
            return value.SetTime(12, 0, 0, 0);
        }

		#endregion

		#region Methods: Set date, time, datepart

		/// <summary>
        /// Returns <see cref="DateTime"/> with changed Year part.
        /// </summary>
        public static DateTime SetDate(this DateTime value, int year)
        {
            return SetYear(value, year);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year and Month part.
        /// </summary>
        public static DateTime SetDate(this DateTime value, int year, int month)
        {
            return new DateTime(year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year, Month and Day part.
        /// </summary>
        public static DateTime SetDate(this DateTime value, int year, int month, int day)
        {
            return new DateTime(year, month, day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year part.
        /// </summary>
        public static DateTime SetYear(this DateTime value, int year)
        {
            return new DateTime(year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Month part.
        /// </summary>
        public static DateTime SetMonth(this DateTime value, int month)
        {
            return new DateTime(value.Year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Day part.
        /// </summary>
        public static DateTime SetDay(this DateTime value, int day)
        {
            return new DateTime(value.Year, value.Month, day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

		/// <summary>
		/// Returns the original <see cref="DateTime"/> with Hour part changed to supplied hour parameter.
		/// </summary>
		public static DateTime SetTime(this DateTime originalDate, int hour)
		{
			return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, originalDate.Minute, originalDate.Second, originalDate.Millisecond);
		}

		/// <summary>
		/// Returns the original <see cref="DateTime"/> with Hour and Minute parts changed to supplied hour and minute parameters.
		/// </summary>
		public static DateTime SetTime(this DateTime originalDate, int hour, int minute)
		{
			return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, originalDate.Second, originalDate.Millisecond);
		}

		/// <summary>
		/// Returns the original <see cref="DateTime"/> with Hour, Minute and Second parts changed to supplied hour, minute and second parameters.
		/// </summary>
		public static DateTime SetTime(this DateTime originalDate, int hour, int minute, int second)
		{
			return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, second, originalDate.Millisecond);
		}

		/// <summary>
		/// Returns the original <see cref="DateTime"/> with Hour, Minute, Second and Millisecond parts changed to supplied hour, minute, second and millisecond parameters.
		/// </summary>
		public static DateTime SetTime(this DateTime originalDate, int hour, int minute, int second, int millisecond)
		{
			return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, second, millisecond);
		}

		/// <summary>
		/// Returns <see cref="DateTime"/> with changed Hour part.
		/// </summary>
		public static DateTime SetHour(this DateTime originalDate, int hour)
		{
			return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, originalDate.Minute, originalDate.Second, originalDate.Millisecond);
		}

		/// <summary>
		/// Returns <see cref="DateTime"/> with changed Minute part.
		/// </summary>
		public static DateTime SetMinute(this DateTime originalDate, int minute)
		{
			return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, minute, originalDate.Second, originalDate.Millisecond);
		}

		/// <summary>
		/// Returns <see cref="DateTime"/> with changed Second part.
		/// </summary>
		public static DateTime SetSecond(this DateTime originalDate, int second)
		{
			return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, originalDate.Minute, second, originalDate.Millisecond);
		}

		/// <summary>
		/// Returns <see cref="DateTime"/> with changed Millisecond part.
		/// </summary>
		public static DateTime SetMillisecond(this DateTime originalDate, int millisecond)
		{
			return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, originalDate.Minute, originalDate.Second, millisecond);
		}

		#endregion

		#region Methods: Comparisons

		/// <summary>
        /// Determines whether the specified <see cref="DateTime"/> is before then current value.
        /// </summary>
        public static bool IsBefore(this DateTime current, DateTime toCompareWith)
        {
            return current < toCompareWith;
        }

        /// <summary>
        /// Determines whether the specified <see cref="DateTime"/> value is After then current value.
        /// </summary>
        public static bool IsAfter(this DateTime current, DateTime toCompareWith)
        {
            return current > toCompareWith;
        }

        /// <summary>
        /// Determine if a <see cref="DateTime"/> is in the future.
        /// </summary>
        /// <param name="dateTime">The date to be checked.</param>
        /// <returns><c>true</c> if <paramref name="dateTime"/> is in the future; otherwise <c>false</c>.</returns>
        public static bool IsInFuture(this DateTime dateTime)
        {
            return dateTime > DateTime.UtcNow;
        }

        /// <summary>
        /// Determine if a <see cref="DateTime"/> is in the past.
        /// </summary>
        /// <param name="dateTime">The date to be checked.</param>
        /// <returns><c>true</c> if <paramref name="dateTime"/> is in the past; otherwise <c>false</c>.</returns>
        public static bool IsInPast(this DateTime dateTime)
        {
            return dateTime < DateTime.UtcNow;
		}

		#endregion

		#region Methods: Other

		/// <summary>
		/// Returns an English-language suffix (e.g. st, nd or th) for the supplied <see cref="System.DateTime"/>. 
		/// This should not be used when working with Globalization-based projects.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string DaySuffix(this DateTime value)
		{
			string suffix;

			#region Get suffix

			switch (value.Day)
			{
				case 1:
				case 21:
				case 31:
					suffix = "st";
					break;

				case 2:
				case 22:
					suffix = "nd";
					break;

				case 3:
				case 23:
					suffix = "rd";
					break;

				default:
					suffix = "th";
					break;
			}

			#endregion

			return suffix;
		}


		#endregion

	}
}