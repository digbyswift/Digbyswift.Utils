using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Digbyswift.Utils.Extensions
{
	public static class StringExtensions
	{

        private static readonly char[] ReservedRegexChars = { '[', '\\', '^', '$', '.', '|', '*', '+', '?', '(', ')' };
		private static readonly Regex NonUrlFriendyRegex = new Regex(@"([^a-z0-9\-]+)", RegexOptions.IgnoreCase);
		private static readonly Regex UrlUnfriendlyCharRegex = new Regex(@"([’']+)");


		#region Methods: ToTitleCase

		/// <summary>
		/// Creates a new string with Title Case (ie "hEllO wORLd" becomes  "Hello World")
		/// </summary>
		/// <param name="value">The string to convert</param>
		/// <returns>The string in title case</returns>
		public static string ToTitleCase(this string value)
		{
			return ToTitleCase(value, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Creates a new string with Title Case (ie "hEllO wORLd" becomes  "Hello World")
		/// </summary>
		/// <param name="value">The string to convert</param>
		/// <param name="cultureInfo">The culture to use when creating title case</param>
		/// <returns>The string in title case</returns>
		public static string ToTitleCase(this string value, CultureInfo cultureInfo)
		{
            if (value == null)
                return null;

			return cultureInfo.TextInfo.ToTitleCase(value);
		}

		/// <summary>
		/// Creates a new string with Title Case (ie "hEllO wORLd" becomes  "Hello World") using the Invariant Culture
		/// </summary>
		/// <param name="value">The string to convert</param>
		/// <returns>The string in title case</returns>
		public static string ToTitleCaseInvariant(this string value)
		{
			return ToTitleCase(value, CultureInfo.InvariantCulture);
		}

		#endregion

		#region Methods: Substring, Trim, Replace

		/// <summary>
		/// This will perform a substring on the string but will ensures that
		/// the string returned is not broken within a word. If no word boundaries
		/// occur within the length required, the incomplete substring will be
		/// returned
		/// </summary>
		public static string SubPhrase(this string value, int length)
		{
			return SubPhrase(value, 0, length);
		}

		/// <summary>
		/// This will perform a substring on the string but will ensures that
		/// the string returned is not broken within a word. No word boundaries
		/// occur within the length required, the incomplete substring will be
		/// returned
		/// </summary>
		public static string SubPhrase(this string value, int startIndex, int length)
		{
			return SubPhrase(value, startIndex, length, ' ');
		}

		/// <summary>
		/// This will perform a substring on the string but will ensures that
		/// the string returned is only broken at the specified delimiter. If no
		/// delimiter occurs within the length required, the incomplete substring
		/// will be returned
		/// </summary>
		public static string SubPhrase(this string value, int startIndex, int length, char delimiter)
		{
			if (value == null)
				return null;

			if (value.Length < length)
				length = value.Length;

			string workingString = value.Substring(startIndex, length);
			int lastindex = workingString.LastIndexOf(delimiter);

			if (lastindex == -1)
				lastindex = workingString.Length;

			return workingString.Substring(0, lastindex);
		}


		/// <summary>
		/// This will perform a substring on the string but will ensures that
		/// the string returned is not broken proir to a full stop being found. If
		/// no sentence boundaries occur within the length required, the
		/// incomplete substring will be returned
		/// </summary>
		public static string SubSentence(this string value, int startIndex, int length)
		{
			return SubPhrase(value, startIndex, length, '.');
		}
	
		/// <summary>
		/// Will remove all excess whitespace from within a string
		/// and also trim trailing whitespace
		/// </summary>
		public static string TrimWithin(this string value)
		{
			if (value == null)
				return null;

			return new Regex(@"\s+").Replace(value, " ").Trim();
		}

        /// <summary>
        /// Will trim a string and return null if the trimmed string is empty
        /// </summary>
        public static string TrimToNull(this string value)
		{
			return TrimToDefault(value, null);
		}

        /// <summary>
        /// Will trim a string and return the default value if the trimmed string is empty
        /// </summary>
		public static string TrimToDefault(this string value, string defaultValue)
		{
			if (value == null)
				return null;

			if (value.Trim().Equals(String.Empty))
				return defaultValue;

			return value.Trim();
		}

		public static string ReplaceExcess(this string value, char characterToReplace, char characterToReplaceWith)
		{
			if (value == null)
				return null;

            string workingCharacterToReplace = characterToReplace.ToString();

            if (ReservedRegexChars.Contains(characterToReplace))
            {
                workingCharacterToReplace = String.Format(@"\{0}", characterToReplace);
            }

            var reExcessHyphens = new Regex(String.Format("{0}+", workingCharacterToReplace));
			return reExcessHyphens.Replace(value, characterToReplaceWith.ToString());
		}

        /// <summary>
        /// Will strip all non alpha-numeric characters from the input. This can be
        /// useful for normalizing strings for comparison purposes
        /// </summary>
        public static string StripNonAlphaNumeric(this string input)
        {
            return StripNonAlphaNumeric(input, false);
        }

        /// <summary>
        /// Will strip all non alpha-numeric characters from the input. This can be
        /// useful for normalizing strings for comparison purposes
        /// </summary>
        public static string StripNonAlphaNumeric(this string input, bool replaceWithWhitespace)
        {
            if (input == null)
                return null;

            string replacementString = String.Empty;

            if (replaceWithWhitespace)
                replacementString = " ";

            return Regex.Replace(input, "[^a-z0-9]", replacementString, RegexOptions.IgnoreCase);
        }

        public static string StripMarkup(this string input)
        {
            return Regex.Replace(input, "</?[a-z][a-z0-9]*[^<>]*>|<!--.*?-->", "");
        }

		#endregion

		#region Methods: Url

		/// <summary>
		/// Checks wither the string can be used within a URL
		/// </summary>
		public static bool IsUrlFriendly(this string value)
		{
			return !NonUrlFriendyRegex.IsMatch(value);
		}

		/// <summary>
		/// Converts the string into another string that can be used within a URL
		/// </summary>
		public static string ToUrlFriendly(this string value)
		{
			if (value == null)
				return null;

			string workingString = value;

			// Remove excess whitespace
			workingString = workingString.TrimWithin();

			// Removes characters that will misformat the output string
			workingString = UrlUnfriendlyCharRegex.Replace(workingString, String.Empty);
			
			// Replace non URL-friendly characters
			workingString = NonUrlFriendyRegex.Replace(workingString, "-");

			return workingString.ReplaceExcess('-', '-').Trim('-');
		}

		#endregion

		#region Methods: Encoding

		/// <summary>
		/// Base64-encodes a string
		/// </summary>
		public static string EncodeBase64(this string value)
		{
			var bytes = Encoding.ASCII.GetBytes(value);

			return Convert.ToBase64String(bytes);
		}

		/// <summary>
		/// Decodes a Base64-encoded string
		/// </summary>
		public static string DecodeBase64(this string value)
		{
			return Encoding.ASCII.GetString(Convert.FromBase64String(value));
		}

		#endregion

		#region Methods: Coalesce

		public static string Coalesce(this string value, params string[] testValues)
		{
			if (value != null)
				return value;

			return testValues.FirstOrDefault(x => x != null);
		}

		public static string CoalesceNullOrEmpty(this string value, params string[] testValues)
		{
			if (!value.IsNullOrEmpty())
				return value;

			return testValues.FirstOrDefault(x => !x.IsNullOrEmpty());
		}

		#endregion

	}
}