using System.Configuration;

namespace XadesDemo.Configurations.Sections
{
    public class GisServiceConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ServiceCollection Services
        {
            get { return (ServiceCollection) (base["Services"]); }
        } 

        [ConfigurationProperty("OrgPpaGuid", IsRequired = true)]
        public string OrgPpaGuid
        {
            get { return (string)base["OrgPpaGuid"]; }
            set { base["OrgPpaGuid"] = value; }
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