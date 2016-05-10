namespace Xades.Abstractions
{
    /// <summary>
    /// ������������� ������� ��� �������� ������������ ��������� �����������.
    /// </summary>
    public interface IIssuerComparer
    {
        /// <summary>
        /// ���������, ������������� �� ��� ������ ������ �������� ����������� (X509IssuerName).
        /// </summary>
        /// <param name="first">������ ��������</param>
        /// <param name="second">������ ��������</param>
        /// <returns>true, ���� �������� �������������, false �����</returns>
        bool AreSameIssuer(string first, string second);
    }
}