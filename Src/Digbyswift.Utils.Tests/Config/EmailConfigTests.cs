using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Digbyswift.Utils.Config;
using NUnit.Framework;

namespace Digbyswift.Utils.Tests.Config
{

	[TestFixture]
	public class EmailConfigTests
	{
		private XDocument _xDoc;
		private XElement _configurationElement;

		private static readonly string TempDir = Path.GetTempPath();
		private const string TempFileName = "email.config";
		private static readonly string TempPath = Path.Combine(TempDir, TempFileName);
		private static readonly string TempAppPath = Path.Combine(TempDir, Path.GetFileNameWithoutExtension(TempFileName));


		[SetUp]
		public void Setup()
		{
			_xDoc = new XDocument();
			_configurationElement = new XElement("configuration",
										new XElement("configSections",
											new XElement("section",
												new XAttribute("name", "emails"),
												new XAttribute("type", "Digbyswift.Utils.Config.EmailConfig, Digbyswift.Utils")
											)));
		}

		[TearDown]
		public void TearDown()
		{
			File.Delete(TempPath);
			File.Delete(TempAppPath);
		}

		[Test]
		public void ConfigurationManager_ConfigSectionPresent_CanCastToEmailConfig()
		{
			// Arrange
			_configurationElement.Add(
					new XElement("emails")
				);

			var config = SaveConfigFile();

			// Act
			var emailConfig = config.GetSection("emails") as EmailConfig;

			// Assert
			Assert.That(emailConfig, Is.Not.Null);
		}

		[Test]
		public void EmailGroups_NoGroupElements_ReturnsZero()
		{
			// Arrange
			_configurationElement.Add(
					new XElement("emails")
				);

			var config = SaveConfigFile();
			var emailConfig = config.GetSection("emails") as EmailConfig;

			// Act
			var groupCount = emailConfig.Groups.Count;

			// Assert
			Assert.That(groupCount, Is.EqualTo(0));
		}

		[Test]
		public void EmailGroups_TwoEmailGroupElements_ReturnsTwo()
		{
			// Arrange
			_configurationElement.Add(
					new XElement("emails",
							new XElement("emailGroup", new XAttribute("name", "default")),
							new XElement("emailGroup", new XAttribute("name", "test"))
						));

			var config = SaveConfigFile();
			var emailConfig = config.GetSection("emails") as EmailConfig;

			// Act
			var groupCount = emailConfig.Groups.Count;

			// Assert
			Assert.That(groupCount, Is.EqualTo(2));
		}

		[Test]
		public void EmailGroups_GroupElementsPresent_CanAccessViaName()
		{
			// Arrange
			_configurationElement.Add(
					new XElement("emails",
							new XElement("emailGroup", new XAttribute("name", "default")),
							new XElement("emailGroup", new XAttribute("name", "test"))
						));

			var config = SaveConfigFile();
			var emailConfig = config.GetSection("emails") as EmailConfig;

			// Act
			var group = emailConfig.Groups["default"];

			// Assert
			Assert.That(group.Name, Is.EqualTo("default"));
		}

		[Test]
		public void EmailElement_TwoPresent_ReturnsTwo()
		{
			// Arrange
			_configurationElement.Add(
					new XElement("emails",
						new XElement("emailGroup", new XAttribute("name", "default"),
							new XElement("email",
                                new XAttribute("address", "kieron@digbyswift.com"),
								new XAttribute("displayName", "Kieron")
							),
							new XElement("email",
                                new XAttribute("address", "kieron.mcintyre@digbyswift.com"),
								new XAttribute("displayName", "Andy")
							)
						),
						new XElement("emailGroup", new XAttribute("name", "test"))
					));

			var config = SaveConfigFile();
			var emailConfig = config.GetSection("emails") as EmailConfig;

			// Act
			var emailCount = emailConfig.Groups["default"].Count;

			// Assert
			Assert.That(emailCount, Is.EqualTo(2));
		}

		[Test]
		public void EmailElement_AnyPresent_CanAccessObjectViaIndex()
		{
			// Arrange
			_configurationElement.Add(
					new XElement("emails",
						new XElement("emailGroup", new XAttribute("name", "default"),
							new XElement("email",
                                new XAttribute("address", "kieron@digbyswift.com"),
								new XAttribute("displayName", "Kieron")
							),
							new XElement("email",
                                new XAttribute("address", "kieron.mcintyre@digbyswift.com")
							)
						),
						new XElement("emailGroup", new XAttribute("name", "test"))
					));

			var config = SaveConfigFile();
			var emailConfig = config.GetSection("emails") as EmailConfig;

			// Act
			var emailElement = emailConfig.Groups["default"][0];

			// Assert
			Assert.That(emailElement, Is.TypeOf<EmailElement>());
		}

		[Test]
		public void EmailElement_TwoPresent_CanAccessPropertiesViaIndex()
		{
			// Arrange
			_configurationElement.Add(
					new XElement("emails",
						new XElement("emailGroup", new XAttribute("name", "default"),
							new XElement("email",
								new XAttribute("address", "kieron@digbyswift.com"),
								new XAttribute("displayName", "Kieron")
							),
							new XElement("email",
                                new XAttribute("address", "kieron.mcintyre@digbyswift.com")
							)
						),
						new XElement("emailGroup", new XAttribute("name", "test"))
					));

			var config = SaveConfigFile();
			var emailConfig = config.GetSection("emails") as EmailConfig;

			// Act
			var emailElement = emailConfig.Groups["default"][0];
			string emailAddress = emailElement.Address;
			string displayName = emailElement.DisplayName;

			// Assert
            Assert.That(emailAddress, Is.EqualTo("kieron@digbyswift.com"));
			Assert.That(displayName, Is.EqualTo("Kieron"));
		}

		[Test]
		public void EmailElement_TwoPresent_CanAccessPropertiesViaCollection()
		{
			// Arrange
			_configurationElement.Add(
					new XElement("emails",
						new XElement("emailGroup", new XAttribute("name", "default"),
							new XElement("email",
                                new XAttribute("address", "kieron@digbyswift.com"),
								new XAttribute("displayName", "Kieron")
							),
							new XElement("email",
                                new XAttribute("address", "kieron.mcintyre@digbyswift.com")
							)
						),
						new XElement("emailGroup", new XAttribute("name", "test"))
					));

			var config = SaveConfigFile();
			var emailConfig = config.GetSection("emails") as EmailConfig;

			// Act
			var emailElement = emailConfig.Groups["default"].Emails.FirstOrDefault();
			string emailAddress = emailElement.Address;
			string displayName = emailElement.DisplayName;

			// Assert
            Assert.That(emailAddress, Is.EqualTo("kieron@digbyswift.com"));
			Assert.That(displayName, Is.EqualTo("Kieron"));
		}



		#region Private

		private Configuration SaveConfigFile()
		{
			_xDoc.Add(_configurationElement);
			_xDoc.Save(TempPath);
			_xDoc.Save(TempAppPath);

			return ConfigurationManager.OpenExeConfiguration(TempAppPath);
		}

		#endregion

	}


}