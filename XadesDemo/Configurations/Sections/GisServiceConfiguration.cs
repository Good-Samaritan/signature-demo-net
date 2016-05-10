using System.Configuration;

namespace XadesDemo.Configurations.Sections
{
    public class GisServiceConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ServiceCollection Services => (ServiceCollection)(base["Services"]);

        [ConfigurationProperty("SenderId", IsRequired = true)]
        public string SenderId
        {
            get { return (string)base["SenderId"]; }
            set { base["SenderId"] = value; }
        }

        [ConfigurationProperty("SchemaVersion", IsRequired = true)]
        public string SchemaVersion
        {
            get { return (string)base["SchemaVersion"]; }
            set { base["SchemaVersion"] = value; }
        }

        [ConfigurationProperty("BaseUrl", IsRequired = true)]
        public string BaseUrl
        {
            get { return (string)base["BaseUrl"]; }
            set { base["BaseUrl"] = value; }
        }

        [ConfigurationProperty("SoapConfiguration", IsRequired = true)]
        public SoapConfiguration SoapConfiguration
        {
            get { return (SoapConfiguration)base["SoapConfiguration"]; }
            set { base["SoapConfiguration"] = value; }
        }
    }
}