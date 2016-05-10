namespace Xades.Abstractions
{
    /// <summary>
    /// Предоставляет функции для проверки соответствия издателей сертификата.
    /// </summary>
    public interface IIssuerComparer
    {
        /// <summary>
        /// Проверяет, соответствуют ли две строки одному издателю сертификата (X509IssuerName).
        /// </summary>
        /// <param name="first">Первый издатель</param>
        /// <param name="second">Второй издатель</param>
        /// <returns>true, если издатели соответствуют, false иначе</returns>
        bool AreSameIssuer(string first, string second);
    }
}