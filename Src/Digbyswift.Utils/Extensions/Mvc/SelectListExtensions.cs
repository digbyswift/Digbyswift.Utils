using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Digbyswift.Utils.Extensions.Mvc
{
	public static class SelectListExtensions
	{

        public static IEnumerable<SelectListItem> AddEmptyDefault(this IEnumerable<SelectListItem> list, string text)
		{
            return AddDefault(list, text, String.Empty);
		}

        public static IEnumerable<SelectListItem> AddDefault(this IEnumerable<SelectListItem> list, string text, string value)
		{
			if (list == null)
				throw new ArgumentNullException("list");

			var temp = new List<SelectListItem>(list);
			temp.Insert(0, new SelectListItem() { Text = text, Value = value });

            return temp;
		}

	}
}