using System.Configuration;

namespace XadesDemo.Configurations.Sections
{
    public class SigningConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("CertificateThumbprint", IsRequired = true)]
        public string CertificateThumbprint
        {
            get { return (string)base["CertificateThumbprint"]; }
            set { base["CertificateThumbprint"] = value; }
        }

        [ConfigurationProperty("CertificatePassword", IsRequired = false)]
        public string CertificatePassword
        {
            get { return (string)base["CertificatePassword"]; }
            set { base["CertificatePassword"] = value; }
        }
    }
}