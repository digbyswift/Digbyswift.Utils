using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digbyswift.Utils.Extensions
{
	public static class DictionaryExtensions
	{
		
		/// <summary>
		/// Performs Add() if the key doesn't exist, or updates the associated value if it does.
		/// </summary>
		public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if (dictionary.ContainsKey(key))
			{
				dictionary[key] = value;
			}

            dictionary.Add(key, value);
		}

		/// <summary>
		/// Retrieves a value based on a key. If the key doesn't exist, the default value for the value type will be returned.
		/// </summary>
		public static TValue FindValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			return FindValueOrDefault(dictionary, key, default(TValue));
		}

		/// <summary>
		/// Retrieves a value based on a key. If the key doesn't exist, the defaultValue supplied will be returned.
		/// </summary>
		public static TValue FindValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			if (dictionary.ContainsKey(key))
			{
				return dictionary[key];
			}
			return defaultValue;
		}

        /// <summary>
        /// Returns a querystring based upon the values in a IDictionary. This will not return the ? character.
        /// </summary>
        /// <param name="value">The NameValueCollection</param>
        /// <param name="ignoreKeys">The keys to ignore. These will be treated case-insensitively.</param>
        public static string ToQueryString(this IDictionary<string, string> value, string[] ignoreKeys)
        {
            string[] lowerCaseKeys = (from key in ignoreKeys select key.ToLower()).ToArray();

            return HttpUtility.UrlDecode(
                String.Join("&",
                    Array.ConvertAll(
                        value.Keys.Where(x => !lowerCaseKeys.Contains(x.ToLower())).ToArray(),
                        key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value[key]))
                    )
                )
            );
        }


	}
}