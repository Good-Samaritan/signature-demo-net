using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using Microsoft.Xades;
using Xades.Models;

namespace Xades.Abstractions
{
    public interface ICryptoProvider
    {
        /// <summary>
        /// URI метода подписи
        /// </summary>
        string SignatureMethod { get; }

        /// <summary>
        /// URI метода хеширования
        /// </summary>
        string DigestMethod { get; }

        /// <summary>
        /// Получение реализации асимметричного алгоритма
        /// </summary>
        /// <param name="certificate">Сертификат, исользуемый для подписания</param>
        /// <param name="privateKeyPassword">Пароль от контейнера закрытого ключа</param>
        /// <returns></returns>
        AsymmetricAlgorithm GetAsymmetricAlgorithm(X509Certificate2 certificate, string privateKeyPassword);

        /// <summary>
        /// Получение Reference-элемента для XML-документа
        /// </summary>
        /// <param name="signedElementId">Идентификатор подписываемого узла XML-документа</param>
        /// <param name="signatureId">Идентификатор подписи</param>
        /// <returns></returns>
        Reference GetReference(string signedElementId, string signatureId);

        /// <summary>
        /// Получение форматтера, с помощью которого будет производиться подпись
        /// </summary>
        /// <param name="certificate">Сертификат, с помощью которого будет производиться подпись</param>
        /// <returns></returns>
        AsymmetricSignatureFormatter GetSignatureFormatter(X509Certificate2 certificate);

        /// <summary>
        /// Получение реализации алгоритма хеширования по URI
        /// </summary>
        /// <param name="algorithm">URI метода хеширования</param>
        /// <returns></returns>
        HashAlgorithm GetHashAlgorithm(string algorithm);

        /// <summary>
        /// Получение объекта XadesObject 
        /// </summary>
        /// <param name="xadesInfo">Информация о XAdES-подписи</param>
        /// <param name="signatureId">Идентификатор подписи</param>
        /// <returns></returns>
        XadesObject GetXadesObject(XadesInfo xadesInfo, string signatureId);
    }
}
