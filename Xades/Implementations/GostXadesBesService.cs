using System;
using Xades.Abstractions;
using Xades.Helpers;

namespace Xades.Implementations
{
    public class GostXadesBesService : IXadesService
    {
        public void ValidateSignature(string xmlData, string elementId)
        {
            if (string.IsNullOrEmpty(xmlData))
            {
                throw new ArgumentNullException("xmlData");
            }
            if (string.IsNullOrWhiteSpace(elementId))
            {
                throw new ArgumentNullException("elementId");
            }

            var document = XmlDocumentHelper.Create(xmlData);
            var signedXml = new XadesBesSignedXml(document, elementId)
            {
                CertificateMatcher = new CertificateMatcher(new GostCryptoProvider())
            };
            signedXml.Validate();
        }

        public string Sign(string xmlData, string elementId, string certificateThumbprint, string certificatePassword)
        {
            if (string.IsNullOrEmpty(xmlData))
            {
                throw new ArgumentNullException("xmlData");
            }
            if (string.IsNullOrEmpty(elementId))
            {
                throw new ArgumentNullException("elementId");
            }
            if (string.IsNullOrEmpty(certificateThumbprint))
            {
                throw new ArgumentNullException("certificateThumbprint");
            }

            var originalDoc = XmlDocumentHelper.Create(xmlData);
            var certificate = CertificateHelper.GetCertificateByThumbprint(certificateThumbprint);

            var xadesSignedXml = new XadesBesSignedXml(originalDoc)
            {
                SignedElementId = elementId,
                CryptoProvider = new GostCryptoProvider()
            };

            var element = xadesSignedXml.FindElement(elementId, originalDoc);
            if (element == null)
            {
                throw new InvalidOperationException(string.Format("Не удалось найти узел c Id {0}", elementId));
            }

            xadesSignedXml.ComputeSignature(certificate, certificatePassword);
            xadesSignedXml.InjectSignatureTo(originalDoc);

            return originalDoc.OuterXml;
        }
    }
}