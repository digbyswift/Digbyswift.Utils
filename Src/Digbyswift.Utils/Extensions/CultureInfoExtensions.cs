using System.Globalization;

namespace Digbyswift.Utils.Extensions
{
	public static class CultureInfoExtensions
	{

		public static string SimpleDisplayName(this CultureInfo value)
		{
			return SimpleDisplayName(value, false);
		}

		public static string SimpleDisplayName(this CultureInfo value, bool useNative)
		{
			if (value == null)
				return null;

			var workingDisplayName = useNative ? value.NativeName : value.DisplayName;
			var parentheseIndex = workingDisplayName.IndexOfAny(new[] {'(', ')'});

			if (parentheseIndex == -1)
				return workingDisplayName.ToTitleCaseInvariant();

			return workingDisplayName
						.Substring(0, parentheseIndex)
						.ToTitleCaseInvariant();
		}

	}
}