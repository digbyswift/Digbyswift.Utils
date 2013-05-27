using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Digbyswift.Utils.Http
{
	public class WebRequestHelper
	{

		/// <summary>
		/// Returns a WebRequest object with ContentType and AcceptType
		/// properties set to application/xml
		/// </summary>
		public WebRequest CreateXmlPostRequest(string url, XElement xmlData)
		{
			string xmlString = xmlData.ToString(SaveOptions.DisableFormatting);
			return CreateXmlPostRequest(url, xmlString);
		}

		/// <summary>
		/// Returns a WebRequest object with ContentType and AcceptType
		/// properties set to application/xml
		/// </summary>
		public WebRequest CreateXmlPostRequest(string url, string xmlData)
		{
			return CreatePostRequest(url, xmlData, "application/xml");
		}

		/// <summary>
		/// Returns a WebRequest object with ContentType and AcceptType
		/// properties set to application/json
		/// </summary>
		public WebRequest CreateJsonPostRequest(string url, string json)
		{
			return CreatePostRequest(url, json, "application/json");
		}

		#region Methods: Private

		private WebRequest CreatePostRequest(string url, string value, string mimeType)
		{
			var request = (HttpWebRequest)WebRequest.Create(url);

			byte[] bytes = Encoding.UTF8.GetBytes(value);
			request.ContentLength = bytes.Length;
			request.ContentType = mimeType;
			request.Accept = mimeType;
			request.Method = "POST";

			using (var stream = request.GetRequestStream())
			{
				stream.Write(bytes, 0, bytes.Length);
			}

			return request;
		}

		#endregion

	}
}