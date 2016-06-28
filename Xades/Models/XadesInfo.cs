using System;
using System.Security.Cryptography.X509Certificates;

namespace Xades.Models
{
    public class XadesInfo
    {
        public byte[] RawCertData { get; private set; }
        public DateTime SigningDateTimeUtc { get; private set; }
        public int TimeZoneOffsetMinutes { get; private set; }

        public XadesInfo(X509Certificate certificate)
        {
            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);

            RawCertData = certificate.GetRawCertData();
            SigningDateTimeUtc = DateTime.UtcNow;
            TimeZoneOffsetMinutes = Convert.ToInt32(offset.TotalMinutes);
        }
    }
}

