using System.Configuration;

namespace XadesDemo.Configurations.Sections
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("ServiceName", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ServiceName
        {
            get { return ((string)(base["ServiceName"])); }
            set { base["ServiceName"] = value; }
        }

        [ConfigurationProperty("Path", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Path
        {
            get { return ((string)(base["Path"])); }
            set { base["Path"] = value; }
        }

        [ConfigurationProperty("AddSignature", DefaultValue = true, IsKey = false)]
        public bool AddSignature
        {
            get { return ((bool)(base["AddSignature"])); }
            set { base["AddSignature"] = value; }
        }

        [ConfigurationProperty("AddSenderId", IsRequired = true, IsKey = false)]
        public bool AddSenderId
        {
            get { return ((bool)(base["AddSenderId"])); }
            set { base["AddSenderId"] = value; }
        }

        [ConfigurationProperty("Methods")]
        public MethodCollection Methods => (MethodCollection)base["Methods"];
    }
}