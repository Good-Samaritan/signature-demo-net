using System.Security.Cryptography.X509Certificates;
using Microsoft.Xades;

namespace Xades.Abstractions
{
    /// <summary>
    /// ��������� ����� �����������, ������� �������� XML ��������.
    /// </summary>
    public interface ICertificateMatcher
    {
        /// <summary>
        /// ��������� ����������, ������� �������� ��������.
        /// </summary>
        /// <param name="signedXml">����������� xml ��������</param>
        /// <returns></returns>
        X509Certificate2 GetSignatureCertificate(XadesSignedXml signedXml);
    }
}