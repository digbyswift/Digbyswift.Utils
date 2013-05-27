using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI.WebControls;

namespace Digbyswift.Utils.Extensions.WebForms
{
	public static class ListItemCollectionExtensions
	{

		/// <summary>
		/// Returns the selected ListItem objects from the ListItemCollection
		/// </summary>
		public static IEnumerable<ListItem> SelectedItems(this ListItemCollection value)
		{
			if (value.Count == 0)
				return new Collection<ListItem>();

			return value.Cast<ListItem>().Where(x => x.Selected);
		}

		/// <summary>
		/// Returns the selected ListItem's values from the ListItemCollection
		/// </summary>
		public static IEnumerable<string> SelectedValues(this ListItemCollection value)
		{
			var items = value.SelectedItems();

			if (items.Count() == 0)
				return new Collection<string>();

			return items.Select(x => x.Value);
		}

		/// <summary>
		/// Returns the selected ListItem's values from the ListItemCollection. This
		/// will attempt to convert the values to a generic type
		/// </summary>
		public static IEnumerable<T> SelectedValues<T>(this ListItemCollection value)
		{
			var values = value.SelectedValues();

			if (values.Count() == 0)
				return new Collection<T>();

			return values.Select(x => (T)Convert.ChangeType(x, typeof(T)));
		}

		/// <summary>
		/// Returns the ListItems from the ListItemCollection that aren't selected.
		/// </summary>
		public static IEnumerable<ListItem> NonSelectedItems(this ListItemCollection value)
		{
			if (value.Count == 0)
				return new Collection<ListItem>();

			return value.Cast<ListItem>().Where(x => !x.Selected);
		}

		/// <summary>
		/// Returns the values from the ListItemCollection for the items that aren't 
		/// selected.
		/// </summary>
		public static IEnumerable<string> NonSelectedValues(this ListItemCollection value)
		{
			var items = value.NonSelectedItems();

			if (items.Count() == 0)
				return new Collection<string>();

			return items.Select(x => x.Value);
		}

		/// <summary>
		/// Returns the values from the ListItemCollection for the items that aren't 
		/// selected. This will attempt to convert the values to a generic type
		/// </summary>
		public static IEnumerable<T> NonSelectedValues<T>(this ListItemCollection value)
		{
			var values = value.NonSelectedValues();

			if (values.Count() == 0)
				return new Collection<T>();

			return values.Select(x => (T)Convert.ChangeType(x, typeof(T)));
		}

		/// <summary>
		/// Set the selected ListItem based upon the value passed
		/// </summary>
		public static void SetSelected<T>(this ListItemCollection value, T selectedValue)
		{
			SetSelected(value, new [] { selectedValue });
		}

		/// <summary>
		/// Set the selected ListItem based upon the values passed. If the value
		/// doesn't exist no item will become selected.
		/// </summary>
		public static void SetSelected<T>(this ListItemCollection value, params T[] selectedValues)
		{
			var stringValues = selectedValues.Select(x => x.ToString());

			foreach (ListItem listItem in value)
			{
				if (stringValues.Contains(listItem.Value))
				{
					listItem.Selected = true;
					break;
				}
			}
		}

	}
}