using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule
{
    public static class DateTimeExtensions
    {
        public static int GlobalWeek(this DateTime date)
        {
            int milisecond = (int)date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            int week = milisecond / (24 * 60 * 60 * 1000 * 7);
            return week;
        }
    }
}
