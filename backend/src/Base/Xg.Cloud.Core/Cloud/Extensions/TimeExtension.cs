using Cloud.Models;
using System;

namespace Cloud.Extensions
{
    public static class TimeExtension
    {
        public static readonly DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis() => (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;

        public static long ConvertToTimeStamp(this DateTime time) => (long)(time.AddHours(-8) - Jan1st1970).TotalMilliseconds;

        public static int CurrentTimeSecond() => (int)(DateTime.UtcNow - Jan1st1970).TotalSeconds;

        public static TimeSpan CurrentTimeSpan() => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        public static int CurrentTimeSpanTotalSeconds() => Convert.ToInt32(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).TotalSeconds);

        public static int CurrentTimeSpanString() => Convert.ToInt32(DateTime.Now.ToString("HHmmss"));

        public static string CurrentTimeFFString() =>DateTime.Now.ToString("ssfff");

        public static string GetHoursByMinute(int time)
        {
            if (time < 0)
                throw new MyException("无效的time值",0);
            if (time < 60)
                return $"{time}分钟";
            var timeSpan = TimeSpan.FromMinutes(time);
            return $"{(int)timeSpan.TotalHours}小时{timeSpan.Minutes}分钟";
        }

    }
}
