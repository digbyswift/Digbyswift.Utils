using System.Web;

namespace Digbyswift.Utils.Extensions.Net
{
	public static class HttpRequestExtensions
	{

        public struct ServerVariables
        {
            public static string HttpXForwardedFor = "HTTP_X_FORWARDED_FOR";
            public static string RemoteAddr = "REMOTE_ADDR";
        }


        public static string GetCurrentIp(this HttpRequest value)
        {
            return new HttpRequestWrapper(value).GetCurrentIp();
        }

        public static string GetCurrentIp(this HttpRequestBase value)
        {
            string workingIp = value.ServerVariables[ServerVariables.HttpXForwardedFor];

            //If there is no proxy, get the standard remote address
            if (workingIp.TrimToNull() != null && workingIp.ToUpperInvariant() != "UNKNOWN")
                return workingIp;

            return value.ServerVariables[ServerVariables.RemoteAddr];
        }

        /// <summary>
        /// Specifies whether the current HttpRequest object has a Referrer
        /// </summary>
        public static bool HasReferrer(this HttpRequest value)
        {
            return new HttpRequestWrapper(value).HasReferrer();
        }

        /// <summary>
        /// Specifies whether the current HttpRequest object has a Referrer
        /// </summary>
        public static bool HasReferrer(this HttpRequestBase value)
        {
            return value.UrlReferrer != null;
        }

        /// <summary>
        /// Returns true if the the current HttpRequest object's referrer is
        /// internal. Will return false if no referrer exists.
        /// </summary>
        public static bool HasInternalReferrer(this HttpRequest value)
        {
            return new HttpRequestWrapper(value).HasInternalReferrer();
        }
        
        /// <summary>
        /// Returns true if the the current HttpRequest object's referrer is
        /// internal. Will return false if no referrer exists.
        /// </summary>
        public static bool HasInternalReferrer(this HttpRequestBase value)
        {
            if (!value.HasReferrer())
                return false;

            return value.UrlReferrer.DnsSafeHost == value.Url.DnsSafeHost;
        }


	}
}