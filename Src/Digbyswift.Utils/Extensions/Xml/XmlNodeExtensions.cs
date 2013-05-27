using System.Xml;
using System.Xml.Linq;

namespace Digbyswift.Utils.Extensions.Xml
{
	public static class XmlNodeExtensions
	{

		public static XDocument ToXDocument(this XmlNode value)
		{
            return ToXDocument(value, LoadOptions.None);
		}

		public static XDocument ToXDocument(this XmlNode value, LoadOptions loadOptions)
		{
			if (value == null)
				return null;

			XDocument doc = XDocument.Load(new XmlNodeReader(value), loadOptions);
			return doc;
		}

	}
}