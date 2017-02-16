using System;
using System.Configuration;
using System.Linq;

namespace XadesDemo.Configurations.Sections
{
    [ConfigurationCollection(typeof(ServiceElement), AddItemName = "Service")]
    public class ServiceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)(element)).ServiceName;
        }

        public ServiceElement this[string name]
        {
            get
            {
                var key = BaseGetAllKeys().FirstOrDefault(x => string.Equals((string)x, name, StringComparison.InvariantCultureIgnoreCase));
                return (ServiceElement)BaseGet(key ?? name);
            }
        }
    }
}