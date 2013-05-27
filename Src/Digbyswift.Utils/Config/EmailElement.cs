using System.Configuration;

namespace Digbyswift.Utils.Config
{
	/// <summary>
	/// This class defines the properties of each of the
	/// child elements of the emailConfig element
	/// </summary>
	public class EmailElement : ConfigurationElement
	{
		[ConfigurationProperty("address", IsRequired = true)]
		public string Address
		{
			get
			{
				return this["address"] as string;
			}
		}

		[ConfigurationProperty("displayName", IsRequired = false)]
		public string DisplayName
		{
			get { return this["displayName"] as string; }
		}

	}
}