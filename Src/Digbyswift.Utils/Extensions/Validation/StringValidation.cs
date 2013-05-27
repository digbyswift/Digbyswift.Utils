using System.Text.RegularExpressions;

namespace Digbyswift.Utils.Extensions.Validation
{

	public static class StringValidation
	{

		#region Regular expression patterns

		/// <summary>
		/// A regular expression for keyword lists
		/// </summary>
		public static string CommaSeparatedKeywordPattern = @"^([a-zA-Z0-9\s]+)(,\s*[a-zA-Z0-9\s]+)*$";

		/// <summary>
		/// A regular expression for email addresses
		/// </summary>
		public const string EmailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

		/// <summary>
		/// A loose regular expression for UK National Insurance Numbers
		/// </summary>
		public const string NinLoosePattern = @"^[a-zA-Z]{1}\-?[a-zA-Z]{1}\-?[0-9]{2}\-?[0-9]{2}\-?[0-9]{2}\-?[a-zA-Z]{0,1}$";

		/// <summary>
		/// A strict regular expression for UK National Insurance Numbers
		/// </summary>
		public const string NinStrictPattern = @"^[A-CEGHJ-PR-TW-Z]{1}\-?[A-CEGHJ-NPR-TW-Z]{1}\-?[0-9]{2}\-?[0-9]{2}\-?[0-9]{2}\-?[A-DFM]{0,1}$";

		/// <summary>
		/// A regular expression for UK post codes
		/// </summary>
		public const string UkPostCodePattern = @"^(GIR ?0AA|(?:[A-PR-UWYZ](?:\d|\d{2}|[A-HK-Y]\d|[A-HK-Y]\d\d|\d[A-HJKSTUW]|[A-HK-Y]\d[ABEHMNPRV-Y])) ?\d[ABD-HJLNP-UW-Z]{2})$";

		/// <summary>
		/// A regular expression for US/Canadian zip codes
		/// </summary>
		public const string UsCanadianZipCodePattern = @"^((\d{5}-\d{4})|(\d{5})|([A-Z]\d[A-Z]\s?\d[A-Z]\d))$";

		/// <summary>
		/// A regular expression for US zip codes
		/// </summary>
		public const string UsZipCodePattern = @"^\d{5}(-\d{4})?$";

		/// <summary>
		/// A regular expression for numerics
		/// </summary>
		public const string WholeNumericPattern = @"^\d+$";

		/// <summary>
		/// A regular expression for numerics
		/// </summary>
		public const string FloatingNumericPattern = @"^\d*\.\d+$";

		/// <summary>
		/// A regular expression for numerics
		/// </summary>
		public const string Ipv4Pattern = @"((?<login>\w+):(?<password>\w+)@)?(?<ip>\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3})(:(?<port>\d+))?";

		#endregion

		#region Regular expressions

		private static readonly Regex CommaSeparatedKeywordRegex = new Regex(CommaSeparatedKeywordPattern, RegexOptions.IgnoreCase);
		private static readonly Regex EmailRegex = new Regex(EmailPattern, RegexOptions.IgnoreCase);
		private static readonly Regex NinStrictRegex = new Regex(NinStrictPattern, RegexOptions.IgnoreCase);
		private static readonly Regex NinLooseRegex = new Regex(NinLoosePattern, RegexOptions.IgnoreCase);
		private static readonly Regex UkPostCodeRegex = new Regex(UkPostCodePattern, RegexOptions.IgnoreCase);
		private static readonly Regex UsCanadianZipCodeRegex = new Regex(UsCanadianZipCodePattern, RegexOptions.IgnoreCase);
		private static readonly Regex UsZipCodeRegex = new Regex(UsZipCodePattern, RegexOptions.IgnoreCase);
		private static readonly Regex WholeNumberRegex = new Regex(WholeNumericPattern);
		private static readonly Regex FloatingNumberRegex = new Regex(FloatingNumericPattern);
		private static readonly Regex Ipv4Regex = new Regex(Ipv4Pattern);

		#endregion

		#region Methods: Validation

		/// <summary>
		/// Determines whether the specified value is a valid list of comma separated
		/// keywords. This will return true for a single alphanumeric value, or a comma
		/// delimited list of alphanumeric values
		/// </summary>
		/// <param name="value">The value to be tested.</param>
		public static bool IsCommaSeparated(this string value)
		{
			return CommaSeparatedKeywordRegex.IsMatch(value);
		}

		/// <summary>
		/// Determines whether the specified value is a valid email
		/// </summary>
		/// <param name="value">The value to be tested.</param>
		public static bool IsEmail(this string value)
		{
			return EmailRegex.IsMatch(value);
		}

		/// <summary>
		/// Determines whether a value is a valid national insurance
		/// </summary>
		/// <param name="value">The value to check.</param>
		public static bool IsNin(this string value)
		{
			return IsNin(value, true);
		}

		/// <summary>
		/// Determines whether a value is a valid national insurance
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <param name="useStrict">If set to <c>true</c> a strict National
		/// Insurance Number regular expression will be used.</param>
		public static bool IsNin(this string value, bool useStrict)
		{
			return useStrict
						? NinStrictRegex.IsMatch(value)
						: NinLooseRegex.IsMatch(value);
		}

		/// <summary>
		/// Determines whether a value is a valid UK post code
		/// </summary>
		/// <param name="value">The value to check.</param>
		public static bool IsUkPostCode(this string value)
		{
			return UkPostCodeRegex.IsMatch(value);
		}

		/// <summary>
		/// Determines whether a value is a valid US zip code
		/// </summary>
		/// <param name="value">The value to check.</param>
		public static bool IsUsZipCode(this string value)
		{
			return UsZipCodeRegex.IsMatch(value);
		}

		/// <summary>
		/// Determines whether a value is a valid US/Canadian zip code
		/// </summary>
		/// <param name="value">The value to check.</param>
		public static bool IsUsCandianZipCode(this string value)
		{
			return UsCanadianZipCodeRegex.IsMatch(value);
		}

		/// <summary>
		/// Determines whether the specified value is a valid numeric
		/// </summary>
		/// <param name="value">The value to be tested.</param>
		public static bool IsWholeNumber(this string value)
		{
			return WholeNumberRegex.IsMatch(value);
		}

		/// <summary>
		/// Determines whether the specified value is a valid numeric. This will
		/// return true if the number is whole or floating.
		/// </summary>
		/// <param name="value">The value to be tested.</param>
		public static bool IsFloatingNumber(this string value)
		{
			return IsWholeNumber(value) || FloatingNumberRegex.IsMatch(value);
		}

		/// <summary>
		/// Determines whether the specified value is a valid numeric
		/// </summary>
		/// <param name="value">The value to be tested.</param>
		public static bool IsIPv4(this string value)
		{
			return Ipv4Regex.IsMatch(value);
		}

		#endregion

	}

}
