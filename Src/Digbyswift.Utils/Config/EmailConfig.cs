using System.Configuration;

namespace Digbyswift.Utils.Config
{
    /// <summary>
    /// Defines XPath statements that map to specific umbraco nodes
    /// </summary>
    public sealed class EmailConfig : ConfigurationSection
    {

        #region Singleton definition

        private static readonly EmailConfig _emailGroups;
		internal EmailConfig() { }
        static EmailConfig()
        {
            _emailGroups = ConfigurationManager.GetSection(SectionName) as EmailConfig; 
        }

        public static EmailConfig Instance
        {
            get { return _emailGroups; }
        }

        #endregion

        private const string SectionName = "emails";

        [ConfigurationCollection(typeof(EmailGroupCollection))]
        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        public EmailGroupCollection Groups
        {
            get
            {
                return (EmailGroupCollection)base[""];
            }
        }

	}

}