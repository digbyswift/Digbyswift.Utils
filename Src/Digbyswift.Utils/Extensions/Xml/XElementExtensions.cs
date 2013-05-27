using System;
using System.Xml.Linq;

namespace Digbyswift.Utils.Extensions.Xml
{
	public static class XElementExtensions
	{

		public static T GetValueAs<T>(this XElement element)
		{
			if (element == null)
				return default(T);

			return (T)Convert.ChangeType(element.Value, typeof(T));
		}

	}
}