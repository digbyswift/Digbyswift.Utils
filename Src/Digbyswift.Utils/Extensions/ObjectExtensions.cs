using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Digbyswift.Utils.Extensions
{
	public static class ObjectExtensions
	{

		/// <summary>
		/// Get a string representation of an object's property (field) using runtime reflection 
		/// </summary>
		public static string GetReflectedPropertyValue(this object subject, string field)
		{
			object reflectedValue = subject.GetType().GetProperty(field).GetValue(subject, null);
			return reflectedValue != null ? reflectedValue.ToString() : "";
		}

		/// <summary>
		/// Determines if an object has a property with the given fieldname
		/// </summary>
		public static bool HasProperty(this object subject, string field)
		{
			Type targetType = subject is Type ? (Type)subject : subject.GetType();
			List<PropertyInfo> props = new List<PropertyInfo>();
			props.AddRange(targetType.GetProperties());
			return props.Any(item => item.Name.Equals(field, StringComparison.OrdinalIgnoreCase));
		}

	}
}