using System;
using System.Configuration;
using System.Linq;

namespace XadesDemo.Configurations.Sections
{
    [ConfigurationCollection(typeof(MethodElement), AddItemName = "Method")]
    public class MethodCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MethodElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MethodElement)(element)).MethodName;
        }

        public MethodElement this[string name]
        {
            get {
                var key = BaseGetAllKeys().FirstOrDefault(x => string.Equals((string)x, name, StringComparison.InvariantCultureIgnoreCase));
                return (MethodElement)BaseGet(key ?? name);
            }
        }
    }
}