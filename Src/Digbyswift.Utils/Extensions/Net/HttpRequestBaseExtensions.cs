using System.Web;

namespace Digbyswift.Utils.Extensions.Net
{
	public static class HttpRequestBaseExtensions
	{

        public static string GetCurrentIp(this HttpRequestBase value)
        {
            string workingIp = value.ServerVariables[HttpRequestExtensions.ServerVariables.HttpXForwardedFor];

            //If there is no proxy, get the standard remote address
            if (workingIp.TrimToNull() != null && workingIp.ToUpperInvariant() != "UNKNOWN")
                return workingIp;

            return value.ServerVariables[HttpRequestExtensions.ServerVariables.RemoteAddr];
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
        public static bool HasInternalReferrer(this HttpRequestBase value)
		{
			if (!value.HasReferrer())
				return false;

			return value.UrlReferrer.DnsSafeHost == value.Url.DnsSafeHost;
		}

	}
}