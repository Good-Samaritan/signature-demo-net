using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.Xades;
using Xades.Abstractions;
using Xades.Exceptions;
using Xades.Helpers;
using Xades.Models;

namespace Xades
{
    public class XadesBesSignedXml : XadesSignedXml
    {
        public ICertificateMatcher CertificateMatcher { get; set; }
        public ICryptoProvider CryptoProvider { get; set; }

        /// <param name="document">Документ, который необходимо подписать с помощью XAdES-BES</param>
        public XadesBesSignedXml(XmlDocument document) : base(document) { }

        /// <param name="document">Документ, подписанный с помощью XAdES-BES</param>
        /// <param name="elementId">Id подписанного элемента</param>
        public XadesBesSignedXml(XmlDocument document, string elementId) : base(document, elementId)
        {
            var elementToVerify = FindElement(elementId, document);
            var signatureNodeList = FindSignatureNodes(elementToVerify);
            if (!signatureNodeList.Any())
            {
                throw new InvalidOperationException($"Элемент с Id {elementId} не содержит подписи");
            }
            if (signatureNodeList.Length > 1)
            {
                throw new InvalidOperationException($"Элемент с {elementId} подписан более одного раза");
            }
            var signatureNode = (XmlElement) signatureNodeList[0];
            var standart = GetSignatureStandart(signatureNode);
            if (standart != KnownSignatureStandard.Xades)
            {
                throw new InvalidOperationException($"Элемент с Id {elementId} подписан не по стандарту XADES-BES");
            }
            LoadXml(signatureNode);
        }

        /// <summary>
        /// Валидация XAdES-BES подписи
        /// </summary>
        public void Validate()
        {
            X509Certificate2 matchedCert;
            try
            {
                matchedCert = CertificateMatcher.GetSignatureCertificate(this);
            }
            catch (InvalidOperationException ex)
            {
                throw new XadesBesValidationException(ex.Message, ex);
            }
            if (matchedCert == null)
            {
                throw new XadesBesValidationException("XML подпись неверна");
            }
            ValidateCertificate(matchedCert);
            ValidateAdditionalProperties();
        }

        /// <summary>
        /// Подписывает XML-документ c помощью XAdES-BES
        /// </summary>
        /// <param name="certificate">Сертификат, с помощью которого производится подпись</param>
        /// <param name="privateKeyPassword">Пароль от контейнера закрытого ключа используемого сертификата</param>
        public void ComputeSignature(X509Certificate2 certificate, string privateKeyPassword)
        {
            var signatureId = $"xmldsig-{Guid.NewGuid().ToString().ToLower()}";

            SigningKey = CryptoProvider.GetAsymmetricAlgorithm(certificate, privateKeyPassword);

            Signature.Id = signatureId;
            SignatureValueId = $"{signatureId}-sigvalue";

            var reference = CryptoProvider.GetReference(SignedElementId, signatureId);
            AddReference(reference);

            SignedInfo.CanonicalizationMethod = XmlDsigCanonicalizationUrl;
            SignedInfo.SignatureMethod = CryptoProvider.SignatureMethod;

            var xadesInfo = new XadesInfo(certificate);
            KeyInfo = KeyInfoHelper.Create(certificate);

            var xadesObject = CryptoProvider.GetXadesObject(xadesInfo, signatureId);
            AddXadesObject(xadesObject);

            ComputeSignature();

            HashAlgorithm hashAlgorithm;
            GetSignedInfoHash(out hashAlgorithm);

            var formatter = CryptoProvider.GetSignatureFormatter(certificate);
            var signedHash = formatter.CreateSignature(hashAlgorithm.Hash);
            Signature.SignatureValue = signedHash;
        }

        public XmlElement FindElement(string elementId, XmlDocument xmlDocument)
        {
            var elementToVerify = GetIdElement(xmlDocument, elementId);
            if (elementToVerify == null)
            {
                throw new InvalidOperationException($"Элемент с Id {elementId} не найден");
            }
            return elementToVerify;
        }

        private KnownSignatureStandard GetSignatureStandart(XmlElement signatureElement)
        {
            return GetXadesObjectElement(signatureElement) == null ? KnownSignatureStandard.XmlDsig : KnownSignatureStandard.Xades;
        }

        private static XmlNode[] FindSignatureNodes(XmlElement elementToVerify)
        {
            var items = elementToVerify.ChildNodes
                .OfType<XmlNode>()
                .Where(x => string.Equals(x.LocalName, SignatureTagName, StringComparison.InvariantCulture)
                        && string.Equals(x.NamespaceURI, XmlDsigNamespaceUrl, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();
            return items;
        }

        private void ValidateAdditionalProperties()
        {
            var signedSignatureProperties = SignedSignatureProperties;
            var signedDataObjectProperties = SignedDataObjectProperties;

            //XAdES 1.4.2 clause G.2.2.6 Checking SignaturePolicyIdentifier
            ThrowIfTrue(signedSignatureProperties?.SignaturePolicyIdentifier != null, "SignaturePolicyIdentifier");

            //XAdES 1.4.2 clause G.2.2.8 Checking DataObjectFormat
            ThrowIfTrue(signedDataObjectProperties?.DataObjectFormatCollection.IsNotEmpty(), "DataObjectFormat");

            //XAdES 1.4.2 clause G.2.2.9 Checking CommitmentTypeIndication
            ThrowIfTrue(signedDataObjectProperties?.CommitmentTypeIndicationCollection.IsNotEmpty(), "CommitmentTypeIndication");

            //XAdES 1.4.2 clause G.2.2.11 Checking SignerRole
            var signerRole = signedSignatureProperties?.SignerRole;
            ThrowIfTrue(signerRole?.ClaimedRoles != null || signerRole?.CertifiedRoles != null, "SignerRole");

            //XAdES 1.4.2 clause G.2.2.16.1.1 Checking AllDataObjectsTimeStamp
            ThrowIfTrue(signedDataObjectProperties?.AllDataObjectsTimeStampCollection.IsNotEmpty(), "AllDataObjectsTimeStamp");

            //XAdES 1.4.2 clause G.2.2.16.1.2 Checking IndividualDataObjectsTimeStamp
            ThrowIfTrue(signedDataObjectProperties?.IndividualDataObjectsTimeStampCollection.IsNotEmpty(), "IndividualDataObjectsTimeStamp");
        }

        private static void ValidateCertificate(X509Certificate2 certificate)
        {
            if (DateTime.Now > certificate.NotAfter || DateTime.Now < certificate.NotBefore)
            {
                throw new XadesBesValidationException("Срок действия сертификата истек или еще не наступил");
            }
            var isCertValid = certificate.Verify();
            if (!isCertValid)
            {
                throw new XadesBesValidationException("Неверный сертификат");
            }
        }

        private static void ThrowIfTrue(bool? condition, string tagName)
        {
            if (condition == true)
            {
                throw new XadesBesValidationException($"Свойство xades:{tagName} не поддерживается");
            }
        }

        private const string SignatureTagName = "Signature";
    }
}