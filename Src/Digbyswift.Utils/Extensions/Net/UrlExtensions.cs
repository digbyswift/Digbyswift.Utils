using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Digbyswift.Utils.Extensions.Net
{
	public static class UrlReoslvingExtensions
	{
        /// <summary>
        /// Converts a virtual path starting with ~/ into an absolute path starting with /.
        /// </summary>
        public static string ResolveToPath(this string path)
		{
			#region Validate

            var context = HttpContext.Current;

            if (context == null)
                throw new InvalidOperationException("There is no HttpContext to resolve the path against");

            if (path == null)
				throw new ArgumentNullException("path");

			if (path.StartsWith("http:") || path.StartsWith("https:"))
				return path;

            string workingPath = path;

			if (!workingPath.StartsWith("~/") && !workingPath.StartsWith("/"))
				throw new ArgumentException(
					"The path parameter must start with a ~/ or / in order to be converted");

			#endregion

            workingPath = workingPath.TrimStart('~'); // Remove initial ~ if present
            workingPath = String.Format("{0}{1}", context.Request.ApplicationPath, workingPath).Replace("//", "/");
            return workingPath;
		}


		public static Uri ResolveServerUrl(this string url)
		{
			return ResolveServerUrl(url, false, false);
		}

        public static Uri ResolveServerUrl(this string path, bool isHttps, bool includePort)
		{
			if (path.StartsWith("http:") || path.StartsWith("https:"))
				return new Uri(path);

            string workingUrl = ResolveToPath(path);

			var context = HttpContext.Current;

			if (context == null)
				return null;

			Uri currentUri = context.Request.Url;

			workingUrl = String.Format("{0}:{1}//{2}{3}",
				isHttps ? "https" : currentUri.Scheme,
				includePort ? currentUri.Authority : "",
				currentUri.Authority,
				workingUrl);

			return new Uri(workingUrl, UriKind.Absolute);
		}

        /// <summary>
        /// Returns a querystring based upon the values in a NameValueCollection. This will not return the ? character.
        /// </summary>
        /// <param name="value">The NameValueCollection</param>
        /// <param name="ignoreKeys">The keys to ignore. These will be treated case-insensitively.</param>
        public static string ToQueryString(this NameValueCollection value, params string[] ignoreKeys)
        {
            return ToQueryString(value.AllKeys.ToDictionary(k => k, k => value[k]), ignoreKeys);
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