using System;
using System.Collections.Generic;


namespace StrikeTrip.UtilityMethods
{
    public class Utility
    {
        public static List<TimeSpan> LoadTimes()
        {
            List<TimeSpan> times = new List<TimeSpan>();
            for (int i = 0; i <= 21; i += 3)
            {
                times.Add(new TimeSpan(i, 0, 0));
            }
            return times;
        }
        public static DateTime CurrentDateTime()
        {
            return DateTime.Now;
        }
        public static DateTime GetDepartureDate()
        {
            return DateTime.Now.AddDays(2);
        }
        public static DateTime GetReturnDate()
        {
            return DateTime.Now.AddDays(7);
        }
        public static DateTime GetFutureDate()
        {
            return DateTime.Now.AddDays(100);
        }
        public static DateTime GetFutureCompareDate()
        {
            return DateTime.Now.AddDays(20);
        }
        public static DateTime GetPastDate()
        {
            return DateTime.Now.AddDays(-20);
        }
    }
}