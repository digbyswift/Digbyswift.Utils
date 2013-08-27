using System.Collections.Specialized;
using System.Linq;

namespace Digbyswift.Utils.Extensions
{
    public static class NameValueCollectionExtensions
	{
        /// <summary>
        /// Returns a querystring based upon the values in a NameValueCollection. This will not return the ? character.
        /// </summary>
        /// <param name="value">The NameValueCollection</param>
        /// <param name="ignoreKeys">The keys to ignore. These will be treated case-insensitively.</param>
        public static string ToQueryString(this NameValueCollection value, params string[] ignoreKeys)
        {
            return value.AllKeys.ToDictionary(k => k, k => value[k]).ToQueryString(ignoreKeys);
        }

    }
}