using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Digbyswift.Utils.Extensions
{
	public static class EnumExtensions
	{

        /// <summary>
        /// Gets the string name of the enum value based upon the usage
        /// of the DisplayNameAttribute. If no attribute is used, the enum
        /// string value will be returned
        /// </summary>
        public static string GetDisplayName(this Enum @enum)
        {
            Type type = @enum.GetType();

            FieldInfo field = type.GetField(@enum.ToString());

            string displayName = @enum.ToString();
            foreach (DisplayNameAttribute attrib in field.GetCustomAttributes(typeof(DisplayNameAttribute), true))
            {
                displayName = attrib.DisplayName;
                break;
            }

            return displayName;
        }

        /// <summary>
        /// Gets the string name of the enum value based upon the usage of
        /// the DisplayNameAttribute. If no attribute is used, the defaultValue
        /// will be returned.
        /// </summary>
        public static string GetDisplayName(this Enum @enum, string defaultValue)
        {
            Type type = @enum.GetType();

            FieldInfo field = type.GetField(@enum.ToString());

            string displayName = defaultValue;
            foreach (DisplayNameAttribute attrib in field.GetCustomAttributes(typeof(DisplayNameAttribute), true))
            {
                displayName = attrib.DisplayName;
                break;
            }

            return displayName;
        }


    }
}