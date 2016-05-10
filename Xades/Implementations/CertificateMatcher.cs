using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using Microsoft.Xades;
using Xades.Abstractions;
using Xades.Exceptions;
using Xades.Helpers;

namespace Xades.Implementations
{
    public class CertificateMatcher : ICertificateMatcher
    {
        public IIssuerComparer IssuerComparer { get; set; } = new IssuerComparer();

        public CertificateMatcher(ICryptoProvider cryptoProvider)
        {
            if (cryptoProvider == null)
            {
                throw new ArgumentNullException(nameof(cryptoProvider));
            }
            _cryptoProvider = cryptoProvider;
        }

        public X509Certificate2 GetSignatureCertificate(XadesSignedXml signedXml)
        {
            var candidates = GetCandidateCertificates(signedXml).ToArray();
            if (!candidates.Any())
            {
                throw new InvalidOperationException("Не найдено ни одного сертификата, обозначенного в xades:SigningCertificate");
            }
            return FindMatchedCertificate(signedXml, candidates);
        }

        private IEnumerable<X509Certificate2> GetCandidateCertificates(XadesSignedXml signedXml)
        {
            var certificates = ExtractCertificates(signedXml).ToArray();
            if (certificates.Length == 0)
            {
                throw new InvalidOperationException("Элемент KeyInfo.X509Data.X509Certificate не заполнен или отсутствует");
            }
            var certInfosCollection = signedXml.SignedSignatureProperties.SigningCertificate.CertCollection;
            if (certInfosCollection == null)
            {
                throw new InvalidOperationException("Элемент xades:SigningCertificate не заполнен или отсутствует");
            }
            var certInfos = certInfosCollection.OfType<Cert>().ToArray();
            foreach (var certificate in certificates)
            {
                var isCertificateMatch = certInfos.Any(certInfo => IsCertificateMatchCertInfo(certificate, certInfo));
                if (isCertificateMatch)
                {
                    yield return certificate;
                }
            }
        }

        private IEnumerable<X509Certificate2> ExtractCertificates(XadesSignedXml signedXml)
        {
            var keyInfo = signedXml.KeyInfo;
            if (keyInfo == null || keyInfo.Count == 0)
            {
                throw new InvalidOperationException("Элемент KeyInfo не заполнен или отсутствует");
            }
            if (keyInfo.Count > 1)
            {
                throw new InvalidOperationException("Найдено более одного элемента KeyInfo");
            }
            var x509Data = keyInfo.OfType<KeyInfoX509Data>().FirstOrDefault();
            if (x509Data == null)
            {
                throw new InvalidOperationException("Элемент X509Data не заполнен или отсутствует");
            }
            return x509Data.Certificates.OfType<X509Certificate2>();
        }

        private readonly ICryptoProvider _cryptoProvider;

        #region Certificate match

        private bool IsCertificateMatchCertInfo(X509Certificate2 certificate, Cert certInfo)
        {
            var isSerialMatch = IsSerialMatch(certificate, certInfo);
            var issuerNameMatch = IsNameMatch(certificate, certInfo);
            var isCertHashMatch = IsCertHashMatch(certificate, certInfo);
            return isSerialMatch && issuerNameMatch && isCertHashMatch;
        }

        private bool IsSerialMatch(X509Certificate2 certificate, Cert certInfo)
        {
            var issuerSerial = certInfo.IssuerSerial.X509SerialNumber;
            string certInfoSerialNumberHex;
            try
            {
                certInfoSerialNumberHex = ConvertHelper.BigIntegerToHex(issuerSerial);
            }
            catch (FormatException)
            {
                certInfoSerialNumberHex = issuerSerial;
            }
            return certificate.SerialNumber == certInfoSerialNumberHex;
        }

        private bool IsCertHashMatch(X509Certificate2 certificate, Cert certInfo)
        {
            var certDigest = certInfo.CertDigest;
            var pkHash = _cryptoProvider.GetHashAlgorithm(certDigest.DigestMethod.Algorithm);
            if (pkHash == null)
            {
                throw new XadesBesValidationException($"Алгоритм {certDigest.DigestMethod.Algorithm} не поддерживается");
            }
            var hashValue = pkHash.ComputeHash(certificate.RawData);
            return ArrayHelper.AreEquals(hashValue, certDigest.DigestValue);
        }

        private bool IsNameMatch(X509Certificate2 certificate, Cert certInfo)
        {
            return IssuerComparer.AreSameIssuer(certificate.Issuer, certInfo.IssuerSerial.X509IssuerName);
        }

        private X509Certificate2 FindMatchedCertificate(SignedXml signedXml, IEnumerable<X509Certificate2> candidates)
        {
            return candidates.FirstOrDefault(candidate => signedXml.CheckSignature(candidate, true));
        }

        #endregion
    }
}