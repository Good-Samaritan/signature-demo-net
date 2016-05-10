using Org.BouncyCastle.Asn1.X509;

namespace Xades.Helpers
{
    public static class X509NameHelper
    {
        public static string ToX509IssuerName(this X509Name x509Name)
        {
            return x509Name.ToString().Replace("E=", "1.2.840.113549.1.9.1=");
        }
    }
}
