using System.Configuration;

namespace XadesDemo.Configurations.Sections
{
    public class SoapConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("SoapTemplate", IsRequired = true)]
        public string SoapTemplatePath
        {
            get { return (string)base["SoapTemplate"]; }
            set { base["SoapTemplate"] = value; }
        }

        [ConfigurationProperty("ISRequestHeaderTemplate", IsRequired = true)]
        public string ISRequestHeaderTemplatePath
        {
            get { return (string)base["ISRequestHeaderTemplate"]; }
            set { base["ISRequestHeaderTemplate"] = value; }
        }

        [ConfigurationProperty("RequestHeaderTemplate", IsRequired = true)]
        public string RequestHeaderTemplatePath
        {
            get { return (string)base["RequestHeaderTemplate"]; }
            set { base["RequestHeaderTemplate"] = value; }
        }
    }
}