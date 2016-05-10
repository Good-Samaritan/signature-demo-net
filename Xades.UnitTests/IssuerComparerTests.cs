using NUnit.Framework;
using Xades.Implementations;

namespace Xades.UnitTests
{
    [TestFixture]
    public class IssuerComparerTests
    {
        private const string Windows_81_V1 = "CN=\"Тестовый УЦ ООО \"\"КРИПТО-ПРО\"\"\", O=\"ООО \"\"КРИПТО-ПРО\"\"\", C=RU, E=info@cryptopro.ru, " +
                                            "L=Москва, S=77 г. Москва, STREET=\"ул.Сущёвский вал, д. 18\", ИНН=007717107991, ОГРН=1037700085444";
        private const string FromSignature = "1.2.643.100.1=1037700085444,1.2.643.3.131.1.1=007717107991,STREET=ул. Сущёвский вал\\, " +
                                        "д. 18,ST=77 г. Москва,L=Москва,1.2.840.113549.1.9.1=info@cryptopro.ru,C=RU,O=ООО \\\"КРИПТО-ПРО\\\"," +
                                        "CN=Тестовый УЦ ООО \\\"КРИПТО-ПРО\\\"";
        private const string Windows_81_V2 = "CN=\"Тестовый УЦ ООО \"\"КРИПТО-ПРО\"\"\", O=\"ООО \"\"КРИПТО-ПРО\"\"\", C=RU, E=info@cryptopro.ru, " +
                                            "L=Москва, S=77 г. Москва, STREET=\"ул. Сущёвский вал, д. 18\", INN=007717107991, OGRN=1037700085444";

        [TestCase(Windows_81_V1, FromSignature, ExpectedResult = true, TestName = "При декодировании могут быть убраны пробелы после точки")]
        [TestCase(Windows_81_V1, FromSignature, ExpectedResult = true, TestName = "При декодировании могут использоваться имена полей ИНН/ОГРН/ОГРНИП")]
        [TestCase(Windows_81_V2, FromSignature, ExpectedResult = true, TestName = "При декодировании могут использоваться имена полей INN/OGRN/OGRNIP")]
        public bool AreSameIssuer_Tests(string first, string second)
        {
            return new IssuerComparer().AreSameIssuer(first, second);
        }
    }
}