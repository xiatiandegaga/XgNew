using System;

namespace Cloud.Extensions
{
    public static class LongExtensions
    {
        public static DateTime ConvertToDateTime(this long timeStamp) => TimeExtension.Jan1st1970.AddMilliseconds(timeStamp).AddHours(8);
    }
}
