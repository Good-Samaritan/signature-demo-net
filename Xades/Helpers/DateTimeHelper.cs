using System;

namespace Xades.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime, int timeZoneOffsetMinutes = 0)
        {
            var shiftedDateTime = dateTime.AddMinutes(timeZoneOffsetMinutes);
            var unspecifiedDateTime = DateTime.SpecifyKind(shiftedDateTime, DateTimeKind.Unspecified);
            return new DateTimeOffset(unspecifiedDateTime, new TimeSpan(0, timeZoneOffsetMinutes, 0));
        }
    }
}
