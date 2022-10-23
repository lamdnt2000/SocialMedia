using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Utils
{
    public class DateUtil
    {

        public static DateTime TimeStampToDateTime(long timeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timeStamp).ToLocalTime();
            return dateTime;
        }

        public static long DateTimeToTimeStamp(DateTime dateTime)
        {
            return ((DateTimeOffset) dateTime).ToUnixTimeSeconds();
        }

        public static double DiffDate(DateTime d1, DateTime d2)
        {
            return ((TimeSpan)(d2 - d1)).TotalDays;
        }

        public static int CompareDateWithoutTime(DateTime d1, DateTime d2)
        {
            return d2.ToShortDateString().CompareTo(d1.ToShortDateString());
        }
    }
}
