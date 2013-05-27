using System.Collections.Generic;

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

	}
}