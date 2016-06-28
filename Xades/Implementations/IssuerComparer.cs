using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Asn1.X509;
using Xades.Abstractions;
using Xades.Helpers;

namespace Xades.Implementations
{
    public class IssuerComparer : IIssuerComparer
    {
        static IssuerComparer()
        {
            var regexps = new Dictionary<string, string>
            {
                { "^(ОГРН|OGRN|OID.1.2.643.100.1)=", "1.2.643.100.1="},
                { "^(ОГРНИП|OGRNIP|OID.1.2.643.100.5)=", "1.2.643.100.5="},
                { "^(ИНН|INN|OID.1.2.643.3.131.1.1)=", "1.2.643.3.131.1.1="},
                { "^(E|Е|OID.1.2.840.113549.1.9.1)=", "1.2.840.113549.1.9.1="},
                { "^S=", "ST="},
                { "\\\"", string.Empty},
                { ". ", "."},
            };
            RegexpToReplace = regexps.ToDictionary(x => new Regex(x.Key), x => x.Value);
        }

        private static IEnumerable<string> ParseIssuer(string issuer)
        {
            return Tokenize(issuer).Select(ReplaceOids);
        }

        private static string ReplaceOids(string oidString)
        {
            foreach (var item in RegexpToReplace)
            {
                oidString = item.Key.Replace(oidString, item.Value);
            }
            return oidString;
        }

        private static IEnumerable<string> Tokenize(string issuer)
        {
            var tokenizer = new X509NameTokenizer(issuer);
            while (tokenizer.HasMoreTokens())
            {
                yield return tokenizer.NextToken();
            }
        }

        private static readonly Dictionary<Regex, string> RegexpToReplace;

        public bool AreSameIssuer(string first, string second)
        {
            var firstTokens = ParseIssuer(first);
            var secondTokens = ParseIssuer(second);
            return EnumerableHelper.AreSequenceEquals(firstTokens, secondTokens);
        }
    }
}