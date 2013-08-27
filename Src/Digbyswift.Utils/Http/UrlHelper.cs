using System;
using System.Web;

namespace Digbyswift.Utils.Http
{
	public static class UrlHelper
	{
        /// <summary>
        /// Converts a virtual path starting with ~/ into an absolute path starting with /.
        /// </summary>
        public static string ConvertToApplicationPath(this string path)
		{
			#region Validate

            if (path == null)
                throw new ArgumentNullException("path");

            if (path.StartsWith("http:") || path.StartsWith("https:"))
                return path;

            var context = HttpContext.Current;

            if (context == null)
                throw new InvalidOperationException("There is no HttpContext to resolve the path against");

            string workingPath = path;

			if (!workingPath.StartsWith("~/"))
				throw new ArgumentException(
					"The path parameter must start with a ~/ in order to be converted");

			#endregion

            workingPath = workingPath.TrimStart('~'); // Remove initial ~ if present
            workingPath = String.Format("{0}{1}", context.Request.ApplicationPath, workingPath).Replace("//", "/");
            return workingPath;
		}

        /// <summary>
        /// Converts an absolute or virtual path to the URL of the current context's domain
        /// </summary>
        /// <returns></returns>
        public static Uri ConvertToUrl(this string path)
		{
            return ConvertToUrl(path, false, false);
		}

        /// <summary>
        /// Converts an absolute or virtual path to the URL of the current context's domain
        /// </summary>
        /// <returns></returns>
        public static Uri ConvertToUrl(this string path, bool isHttps, bool includePort)
		{
			if (path.StartsWith("http:") || path.StartsWith("https:"))
				return new Uri(path);

            string workingUrl = ConvertToApplicationPath(path);

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

    }
}