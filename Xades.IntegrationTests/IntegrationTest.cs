using System;
using NUnit.Framework;
using Xades.Exceptions;
using Xades.Implementations;

namespace Xades.IntegrationTests
{
    [TestFixture]
    public class IntegrationTest
    {
        #region Validate Signature Tests

        private const string NoSigningCertificateError = "Не найдено ни одного сертификата, обозначенного в xades:SigningCertificate";
        private const string InvalidXmlSignatureError = "XML подпись неверна";
        private const string DefaultSignedDataElementId = "signed-data-container";

        [TestCase(@"Valid\sign.xml", DefaultSignedDataElementId)]
        [TestCase(@"Valid\sing-certificate-is-not-installed.xml", "asd")]
        [TestCase(@"Integration\exportNsiListResponse.xml", DefaultSignedDataElementId)]
        public void Validate_ValidSignedXml_NotThrows(string exampleXmlFileName, string elementId)
        {
            var data = NUnitTestHelper.GetInputFile(exampleXmlFileName);
            var service = new GostXadesBesService();
            service.ValidateSignature(data, elementId);
        }
        
        [Ignore("Сертификат устарел")]
        [TestCase(@"Invalid\invalid-signature.xml", DefaultSignedDataElementId, InvalidXmlSignatureError,
            TestName = "Неверная XML подпись")]
        [TestCase(@"Invalid\invalid-reference-to-signed-element.xml", DefaultSignedDataElementId, InvalidXmlSignatureError,
            TestName = "Неверная ссылка в signed info на подписанный элемент")]
        [TestCase(@"Invalid\invalid-reference-to-signature.xml", DefaultSignedDataElementId, InvalidXmlSignatureError,
            TestName = "Неверная ссылка в signed info на подпись")]
        [TestCase(@"Invalid\certificate-not-match-by-digest-value.xml", DefaultSignedDataElementId, NoSigningCertificateError,
            TestName = "Неверный сертификат: digest value")]
        [TestCase(@"Invalid\certificate-not-match-by-issuer-name.xml", DefaultSignedDataElementId, NoSigningCertificateError,
            TestName = "Неверный сертификат: issuer name")]
        [TestCase(@"Invalid\certificate-not-match-by-serial-number.xml", DefaultSignedDataElementId, NoSigningCertificateError,
            TestName = "Неверный сертификат: serial number")]
        [TestCase(@"Invalid\hash-algorithm-not-supported.xml", DefaultSignedDataElementId, "Алгоритм http://www.w3.org/2000/09/xmldsig#sha1 не поддерживается",
            TestName = "Алгоритм хеширования не поддерживается")]
        [TestCase(@"Invalid\commitment.xml", DefaultSignedDataElementId, "Свойство xades:CommitmentTypeIndication не поддерживается",
            TestName = "CommitmentTypeIndication")]
        [TestCase(@"Invalid\signing-role.xml", DefaultSignedDataElementId, "Свойство xades:SignerRole не поддерживается",
            TestName = "SignerRole")]
        [TestCase(@"Invalid\data-object-format.xml", DefaultSignedDataElementId, "Свойство xades:DataObjectFormat не поддерживается",
            TestName = "DataObjectFormat")]
        [TestCase(@"Integration\exportNsiListResponse-expired.xml", DefaultSignedDataElementId, "Срок действия сертификата истек или еще не наступил",
            TestName = "Истек срок действия сертификата")]
        public void Validate_InvalidSignedXml_ThrowsXadesBesValidationException(string exampleXmlFileName, string elementId, string expectedExceptionMessage)
        {
            var data = NUnitTestHelper.GetInputFile(exampleXmlFileName);
            var service = new GostXadesBesService();
            var ex = Assert.Throws<XadesBesValidationException>(() => service.ValidateSignature(data, elementId));
            Assert.AreEqual(expectedExceptionMessage, ex.Message);
        }

        [TestCase(@"Invalid\signed-more-than-once.xml", DefaultSignedDataElementId, "Элемент с signed-data-container подписан более одного раза",
            TestName = "Signed more than once")]
        [TestCase(@"Invalid\not-signed.xml", DefaultSignedDataElementId, "Элемент с Id signed-data-container не содержит подписи",
            TestName = "Not signed")]
        [TestCase(@"Invalid\xmldsig.xml", "data", "Элемент с Id data подписан не по стандарту XADES-BES",
            TestName = "Signed by XMLDSIG")]
        [TestCase(@"Invalid\signature-not-found.xml", DefaultSignedDataElementId, "Элемент с Id signed-data-container не содержит подписи",
            TestName = "Xml doesn't contains signature")]
        [TestCase(@"Valid\sign.xml", "signed-data", "Элемент с Id signed-data не найден",
            TestName = "Invalid element to verify id")]
        [TestCase(@"Invalid\signed-insde.xml", "asd", "Элемент с Id asd не содержит подписи",
            TestName = "Проверка родительского элемента, дочерний элемент подписан")]
        public void Validate_InvalidSignedXml_ThrowsInvalidOperationException(string exampleXmlFileName, string elementId, string expectedExceptionMessage)
        {
            var data = NUnitTestHelper.GetInputFile(exampleXmlFileName);
            var service = new GostXadesBesService();
            var ex = Assert.Throws<InvalidOperationException>(() => service.ValidateSignature(data, elementId));
            Assert.AreEqual(expectedExceptionMessage, ex.Message);
        }

        #endregion

        #region Sing Document Tests

        private const string CertificateThumbprint = "‎139136c6f90a972bb3eabe6368ca80043291da18";
        private const string CertificatePassword = "1";

        [TestCase(@"NotSigned\not-signed-root.xml", "some-id", TestName = "sign-root")]
        [TestCase(@"NotSigned\not-signed-child.xml", "some-id", TestName = "sign-root-child")]
        public void Sign_ValidXml_NotThrows(string exampleXmlFileName, string elementId)
        {
            var data = NUnitTestHelper.GetInputFile(exampleXmlFileName);
            var service = new GostXadesBesService();

            var signedData = service.Sign(data, elementId, CertificateThumbprint, CertificatePassword);

            service.ValidateSignature(signedData, elementId);
        }

        #endregion
    }
}