using System.Configuration;

namespace Digbyswift.Utils.Config
{
    public sealed class EmailGroupCollection : ConfigurationElementCollection
    {

        #region Overridden methods to define collection

        protected override ConfigurationElement CreateNewElement()
        {
			return new EmailCollection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
			return ((EmailCollection)element).Name;
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
            get { return "emailGroup"; }
        }

        #endregion

        /// <summary>
        /// Default property for accessing emailGroups
        /// </summary>
		public new EmailCollection this[string groupName]
        {
            get
            {
				return (EmailCollection)this.BaseGet(groupName);
            }
        }

    }
}
