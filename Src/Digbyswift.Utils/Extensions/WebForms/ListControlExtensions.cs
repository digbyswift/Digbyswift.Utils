using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace Digbyswift.Utils.Extensions.WebForms
{
	public static class ListControlExtensions
	{

		public static ListControl AddEmptyOption(this ListControl list, string text)
		{
			return AddDefaultOption(list, text, String.Empty);
		}

		public static ListControl AddDefaultOption(this ListControl list, string text, string value)
		{
			if (list == null)
				throw new ArgumentNullException("list");

			list.Items.Insert(0, new ListItem() { Text = text, Value = value });

			return list;
		}

		
		/// <summary>
		/// Performs a bind and sets DataValueField and DataTextField to Key and Value respectively
		/// </summary>
		public static void DataBindFromDictionary(this ListControl value, IDictionary<object, string> source)
		{
			if (value == null)
				return;

			value.DataSource = source;
			value.DataTextField = "Value";
			value.DataValueField = "Key";
			value.DataBind();
		}

		/// <summary>
		/// Performs a bind and sets DataValueField and DataTextField to Key and Value respectively
		/// </summary>
		public static void DataBindFromDictionary(this ListControl value, IDictionary<int, string> source)
		{
			if (value == null)
				return;

			value.DataSource = source;
			value.DataTextField = "Value";
			value.DataValueField = "Key";
			value.DataBind();
		}

		// Converts an enum into a dictionary and binds it to the DataValueField and DataTextField properties
		public static void BindFromEnum<TEnum>(this ListControl value, int? selectedValue)
		{
			if (value == null)
				return;

			var dict = Enums.Helper.ToDictionary<TEnum>();
			value.DataBindFromDictionary(dict);
		}

	}
}