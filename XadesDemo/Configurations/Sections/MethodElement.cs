using System.Configuration;

namespace XadesDemo.Configurations.Sections
{
    public class MethodElement : ConfigurationElement
    {
        [ConfigurationProperty("MethodName", IsKey = true, IsRequired = true)]
        public string MethodName
        {
            get { return ((string)(base["MethodName"]));}
            set { base["MethodName"] = value; }
        }

        [ConfigurationProperty("Action", IsKey = false, IsRequired = true)]
        public string Action
        {
            get { return ((string)(base["Action"])); }
            set { base["Action"] = value; }
        }

        [ConfigurationProperty("RequiredBody", DefaultValue = true)]
        public bool RequiredBody
        {
            get { return ((bool)(base["RequiredBody"])); }
            set { base["RequiredBody"] = value; }
        }

        [ConfigurationProperty("Template", IsKey = false, IsRequired = true)]
        public string Template
        {
            get { return ((string)(base["Template"])); }
            set { base["Template"] = value; }
        }
    }
}