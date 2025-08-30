using System.Globalization;
namespace EBook.Core.Helpers
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static Double ToMilliseconds(this DateTime date)
        {
            return (date - UnixEpoch).TotalMilliseconds;
        }
        public static double ToMilliseconds(this DateTimeOffset date)
        {
            return (date - UnixEpoch).TotalMilliseconds;
        }
        public static DateTime FromUnixTime(this long unixTime)
        {
            return UnixEpoch.AddSeconds(unixTime);
        }
        public static long ToUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date - UnixEpoch).TotalSeconds);
        }
        public static long ToUnixTime(this DateTimeOffset date)
        {
            return Convert.ToInt64((date - UnixEpoch).TotalSeconds);
        }

        public static String ToVerbose(this DateTime date, IFormatProvider info = null)
        {
            info = info ?? CultureInfo.InvariantCulture;
            return date.ToString("MMM d, yyyy", info);
        }
        public static String ToIsoString(this DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ", CultureInfo.InvariantCulture);
        }
        public static DateTime GetClosestDayOfWeek(this DateTime @this, DayOfWeek dayOfWeek, int hour, int minute, int second)
        {
            var daysToAdd = (int)dayOfWeek - (int)@this.DayOfWeek;
            if (daysToAdd < 0)
                daysToAdd += 7;
            var dt = new DateTime(@this.Year, @this.Month, @this.Day, hour, minute, second, @this.Kind);
            if ((dt - @this).TotalSeconds > (7 * 24 * 60 * 60))
                dt = dt.AddDays(-7);
            if ((dt - @this).TotalSeconds < 0)
                dt = dt.AddDays(7);
            return dt;
        }
        public static DateTime GetDay(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, @this.Day, 0, 0, 0, @this.Kind);
        }
        public static bool IsLeapYear(this DateTime date)
        {
            return date.Year.IsLeapYear();
        }
        private static bool IsLeapYear(this int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }
    }
}