using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Xades.Helpers
{
    public static class KeyInfoHelper
    {
        public static KeyInfo Create(X509Certificate2 certificate)
        {
            var xmlDocument = new XmlDocument();

            var x509DataElement = xmlDocument.CreateElement("ds", "X509Data", KeyInfoNamespace);
            var x509CertificateElement = xmlDocument.CreateElement("ds", "X509Certificate", KeyInfoNamespace);
            x509CertificateElement.InnerText = Convert.ToBase64String(certificate.GetRawCertData());

            x509DataElement.AppendChild(x509CertificateElement);

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoNode(x509DataElement));

            return keyInfo;
        }

        private const string KeyInfoNamespace = "http://www.w3.org/2000/09/xmldsig#";
    }
}