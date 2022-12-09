using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Date
{
    public class DayData
    {
        public enum WorkType {Day = 0, Dayoff = -1, Night = 1}
    }


    public class Day
    {
        private bool indep;
        private DateTime date;
        private bool last;

        public Day(Day day)
        {
            indep = day.indep;
            date = day.date;
            last = day.last;
        }

        public Day(DateTime date, bool indep, bool last)
        {
            this.date = date;
            this.indep = indep;
            this.last = last;

        }
    }
}
