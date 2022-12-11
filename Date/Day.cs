using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Date
{
    
    
    [Serializable]
    public class Day
    {
        public enum WorkType { Day = 0, Dayoff = -1, Night = 1 }
        private List<WorkType> workTypes;
        private bool indep;
        private DateTime date;
        private bool? last;
        private bool holiday;

        public bool Empty { get { return workTypes.Count <= 0; } }
        public int Week { get { return date.GlobalWeek(); } }
        
        public DateTime Date { get { return date; } }
        public WorkType[] WorkTypes { get { return workTypes.ToArray(); } }
        public bool Holyday { get { return holiday; } }
        public bool Last { 
            get { 
                if(last == null)
                {
                    return false;
                }
                return (bool)last;
            }
            set
            {
                if(last == null)
                {
                    last = value;
                }
                else
                {
                    throw new Exception("The last must be allocated when it is null");
                }
            }
        
        
        }
        public bool Indep { get { return indep; } }


        public Day Append(WorkType workType)
        {
            if(workTypes.Count >= 3)
            {
                throw new Exception("The length of workTypes is must be less than 3");
            }
            workTypes.Add(workType);
            return this;
        }
        public Day Insert(int idx, WorkType workType)
        {
            if (workTypes.Count >= 3)
            {
                throw new Exception("The length of workTypes is must be less than 3");
            }
            workTypes.Insert(idx, workType);
            return this;
        }

        public Day PermuteWork(int a, int b)
        {
            var temp = workTypes[a];
            workTypes[a] = workTypes[b];
            workTypes[b] = temp;
            return this;
        }

        

        public Day(DateTime date, bool indep, List<WorkType> workTypes = null)
        {
            if (workTypes == null)
                this.workTypes = new List<WorkType>();
            else
                this.workTypes = workTypes;

            this.date = date;
            this.indep = indep;

        }
        private Day(DateTime date, bool indep, bool? last, List<WorkType> workTypes = null)
        {
            if (workTypes == null)
                this.workTypes = new List<WorkType>();
            else
                this.workTypes = workTypes;

            this.date = date;
            this.indep = indep;
            this.last = last;

        }

        public Day Clone()
        {
            return new Day(date, indep, last, workTypes);
        }
    }
}
