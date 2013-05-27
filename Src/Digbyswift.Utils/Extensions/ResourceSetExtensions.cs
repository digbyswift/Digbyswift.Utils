using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace Digbyswift.Utils.Extensions
{
	public static class ResourceSetExtensions
	{

		public static IDictionary<object, object> ToDictionary(this ResourceSet value)
		{
			if (value == null)
				return null;

			return value.Cast<DictionaryEntry>().ToDictionary(x => x.Key, x => x.Value);
		}

	}
}