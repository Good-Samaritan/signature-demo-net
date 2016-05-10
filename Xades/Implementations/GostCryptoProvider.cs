using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using CryptoPro.Sharpei;
using CryptoPro.Sharpei.Xml;
using Microsoft.Xades;
using Org.BouncyCastle.X509;
using Xades.Abstractions;
using Xades.Helpers;
using Xades.Models;

namespace Xades.Implementations
{
    public class GostCryptoProvider : ICryptoProvider
    {
        private Dictionary<string, string> HashAlgorithmMap { get; set; } = new Dictionary<string, string>
        {
            ["http://www.w3.org/2001/04/xmldsig-more#gostr3411"] = "GOST3411"
        };

        public HashAlgorithm GetHashAlgorithm(string algorithm)
        {
            string algorithmName;
            var algorithmUrl = algorithm;
            if (!HashAlgorithmMap.TryGetValue(algorithmUrl, out algorithmName))
            {
                return null;
            }
            var pkHash = HashAlgorithm.Create(algorithmName);
            return pkHash;
        }

        private int _referenceIndex;

        public string SignatureMethod => CPSignedXml.XmlDsigGost3410UrlObsolete;
        public string DigestMethod => CPSignedXml.XmlDsigGost3411UrlObsolete;

        public AsymmetricAlgorithm GetAsymmetricAlgorithm(X509Certificate2 certificate, string privateKeyPassword)
        {
            var provider = (Gost3410CryptoServiceProvider)certificate.PrivateKey;

            if (!string.IsNullOrEmpty(privateKeyPassword))
            {
                var secureString = new SecureString();
                foreach (var chr in privateKeyPassword)
                    secureString.AppendChar(chr);

                provider.SetContainerPassword(secureString);
            }

            return provider;
        }

        public Reference GetReference(string signedElementId, string signatureId)
        {
            var reference = new Reference
            {
                Uri = $"#{signedElementId}",
                DigestMethod = DigestMethod,
                Id = $"{signatureId}-ref{_referenceIndex}"
            };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());

            _referenceIndex++;

            return reference;
        }

        public AsymmetricSignatureFormatter GetSignatureFormatter(X509Certificate2 certificate)
        {
            if (certificate.PrivateKey is Gost3410CryptoServiceProvider)
            {
#pragma warning disable 612
                var signatureDescription = (SignatureDescription)CryptoConfig.CreateFromName(CPSignedXml.XmlDsigGost3410UrlObsolete);
#pragma warning restore 612
                return signatureDescription.CreateFormatter(certificate.PrivateKey);
            }

            throw new NotSupportedException("Неизвестный тип закрытого ключа");
        }

        public XadesObject GetXadesObject(XadesInfo xadesInfo, string signatureId)
        {
            var xadesObject = new XadesObject
            {
                QualifyingProperties = new QualifyingProperties
                {
                    Target = $"#{signatureId}",
                    SignedProperties = new SignedProperties { Id = $"{signatureId}-signedprops" }
                }
            };

            var hashAlgorithm = GetHashAlgorithm();
            var hashValue = hashAlgorithm.ComputeHash(xadesInfo.RawCertData);

            var x509CertificateParser = new X509CertificateParser();
            var bouncyCert = x509CertificateParser.ReadCertificate(xadesInfo.RawCertData);

            var cert = new Cert
            {
                IssuerSerial = new IssuerSerial
                {
                    X509IssuerName = bouncyCert.IssuerDN.ToX509IssuerName(),
                    X509SerialNumber = bouncyCert.SerialNumber.ToString()
                },
                CertDigest =
                {
                    DigestValue = hashValue,
                    DigestMethod = new DigestMethod { Algorithm = DigestMethod }
                }
            };

            var signedSignatureProperties = xadesObject.QualifyingProperties.SignedProperties.SignedSignatureProperties;
            signedSignatureProperties.SigningCertificate.CertCollection.Add(cert);
            signedSignatureProperties.SigningTime = xadesInfo.SigningDateTimeUtc.ToDateTimeOffset(xadesInfo.TimeZoneOffsetMinutes);

            return xadesObject;
        }

        private static HashAlgorithm GetHashAlgorithm() => HashAlgorithm.Create("GOST3411");
    }
}
