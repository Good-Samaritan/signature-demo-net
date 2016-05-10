namespace Xades.Abstractions
{
    public interface IXadesService
    {
        /// <summary>
        /// Валидация XAdES подписи
        /// Бросает исключение, если подпись не верна.
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="elementId"></param>
        void ValidateSignature(string xmlData, string elementId);

        /// <summary>
        /// Подпись XML-документа с помощью XAdES подписи
        /// </summary>
        /// <param name="xmlData">XML-документ, который необходимо подписать</param>
        /// <param name="elementId">Значение атрибута Id узла XML-документа, который необходимо подписать</param>
        /// <param name="certificateThumbprint">Отпечаток сертификата, который необходимо использовать для подписи</param>
        /// <param name="certificatePassword">Пароль от контейнера закрытого ключа используемого сертификата</param>
        /// <returns>Подписанный XML-документ</returns>
        string Sign(string xmlData, string elementId, string certificateThumbprint, string certificatePassword);
    }
}