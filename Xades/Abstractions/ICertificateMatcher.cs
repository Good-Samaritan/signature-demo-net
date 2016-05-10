using System.Security.Cryptography.X509Certificates;
using Microsoft.Xades;

namespace Xades.Abstractions
{
    /// <summary>
    /// Реализует поиск сертификата, которым подписан XML документ.
    /// </summary>
    public interface ICertificateMatcher
    {
        /// <summary>
        /// Извлекает сертификат, которым подписан документ.
        /// </summary>
        /// <param name="signedXml">Подписанный xml документ</param>
        /// <returns></returns>
        X509Certificate2 GetSignatureCertificate(XadesSignedXml signedXml);
    }
}