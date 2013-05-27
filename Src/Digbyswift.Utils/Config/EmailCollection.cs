using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Digbyswift.Utils.Config
{
    public sealed class EmailCollection : ConfigurationElementCollection
    {

		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base["name"];
			}
		}

	    public IEnumerable<EmailElement> Emails
	    {
			get { return this.Cast<EmailElement>(); }
	    }

	    #region Overridden methods to define collection

        protected override ConfigurationElement CreateNewElement()
        {
            return new EmailElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
			return ((EmailElement)element).Address;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get { return "email"; }
        }

        #endregion

		public EmailElement this[int i]
		{
			get
			{
				return (EmailElement)this.BaseGet(i);
			}
		}

	}
}
