using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utils
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

        public static DateTime StringToDateTime(string time)
        {
            return DateTime.Parse(time);
        }

        public static long DateTimeToTimeStamp(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        public static double DiffDate(DateTime d1, DateTime d2)
        {
            return ((TimeSpan)(d2 - d1)).TotalDays;
        }
        public static int DiffWeek(DateTime d1, DateTime d2)
        {
            return Convert.ToInt32((((TimeSpan)(d2 - d1)).TotalDays)/7);
        }
        public static int DiffMonth(DateTime d1, DateTime d2)
        {
            return ((d2.Year - d1.Year) * 12) + d2.Month - d1.Month;
        }

        public static int CompareDateWithoutTime(DateTime d1, DateTime d2)
        {
            return d2.ToShortDateString().CompareTo(d1.ToShortDateString());
        }
        public static (DateTime, DateTime) GenerateDateInRange(int range)
        {
            DateTime now = DateTime.Now;
            DateTime before = DateTime.Now.AddMonths(-range);
            return (before, now);
        }

        public static DateTime GenerateDayInRange(int range)
        {
            
            return DateTime.Now.AddDays(range);
           
        }
    }
}
