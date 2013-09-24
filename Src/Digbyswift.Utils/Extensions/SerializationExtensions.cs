using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Digbyswift.Utils.Extensions
{
	public static class SerializationHelper
	{
	
		public static string SerializeAsXml(this object value)
		{
			if(value == null)
				return null;

			var serializer = new XmlSerializer(value.GetType());
			string output;
			
			using(var writer = new StringWriter())
			{
				var xSettings = new XmlWriterSettings {Indent = false};

				using(var xWriter = XmlWriter.Create(writer, xSettings))
				{
					serializer.Serialize(xWriter, value);
					xWriter.Flush();

					output = writer.ToString();
				}
			}

			return output;
		}

    public static T DeserializeFromXml<T>(this string serializedData)
		{
			if (serializedData == null || serializedData.TrimToNull() != null)
				return default(T);

			T output;
			var serializer = new XmlSerializer(typeof(T));

			using (var reader = new StringReader(serializedData))
			{
				output = (T)serializer.Deserialize(reader);
			}
			return output;
		}

	}
}
