using System;
using System.Collections.Generic;
using System.Linq;
using Digbyswift.Utils.Extensions;

namespace Digbyswift.Utils.Enums
{
    public class Helper
    {

        public static IDictionary<int, string> ToDictionary<TEnum>()
        {
            return ToDictionary<TEnum>(false);
        }

        /// <summary>
        /// Returns a IDictionary object but uses EnumDisplayNameAttribute if available and required
        /// </summary>
        public static IDictionary<int, string> ToDictionary<TEnum>(bool useDisplayNameAttribute)
        {
            var type = typeof(TEnum);

            if (!type.IsEnum)
                throw new ArgumentException("The type '" + type.Name + "' supplied is not an Enum");

            var objects = Enum.GetValues(type).Cast<object>();

            return useDisplayNameAttribute
                ? objects.ToDictionary(e => (int)e, e => ((Enum)e).GetDisplayName())
                : objects.ToDictionary(e => (int)e, e => Enum.GetName(type, e));
        }

        public static TEnum Parse<TEnum>(string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        } 
    }
}