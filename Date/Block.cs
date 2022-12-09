using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace WorkSchedule.Date
{
    
    public class Block
    {
        
        private List<Day> days;
        private int week;
        private int dayworker;

        public int[] WorkCount { get
            {
                var result = new int[] { 0, 0, 0 };
                foreach (Day day in days)
                {
                    var workTypes = day.WorkTypes;
                    for (int i = 0; i < workTypes.Length; i++)
                    {
                        result[i] += (int)workTypes[i];
                    }
                }
                return result;
            } }

        public float Score { get
            {
                float result = 0.0f;
                foreach(Day day in days)
                {
                    for(int i = 0; i < day.WorkTypes.Length;i++)
                    {
                        if (i == dayworker && day.WorkTypes[dayworker] == Day.WorkType.Day) result += 1.0f;
                        if (i != dayworker && day.WorkTypes[i] == Day.WorkType.Night) result += 1.0f;

                    }
                }
                return result;

            } }
        
        public int Length { get { return days.Count; } }

        public Block(Block block)
        {
            days = block.days.Select(day => new Day(day)).ToList();
            week = block.week;
            dayworker = block.dayworker;
        }

        public Block(List<Day> days, int dayworker)
        {
            this.days = days;
            this.dayworker = dayworker;
        }

        public bool PermuteWork(int idx, int a, int b)
        {
            
            if ((days[idx].Last || idx + 1 >= Length) || (days[idx].WorkTypes[b] == Day.WorkType.Night && days[idx + 1].WorkTypes[a] == Day.WorkType.Dayoff) || (days[idx].WorkTypes[a] == Day.WorkType.Night && days[idx + 1].WorkTypes[b] == Day.WorkType.Dayoff))
            {
                days[idx].PermuteWork(a, b);
                return true;
            }

            return false;

        }
        public enum FillMethod { First = 0, Second = 1 };
        public Block FillBlock(FillMethod method)
        {
            Day latestDay = null;
            foreach(Day day in days)
            {
                if (!day.Empty)
                {
                    latestDay = day;
                    continue;
                }

                if(latestDay == null)
                {
                    if (method == FillMethod.First)
                        day.Append(Day.WorkType.Night).Append(Day.WorkType.Dayoff);
                    else
                        day.Append(Day.WorkType.Dayoff).Append(Day.WorkType.Night);

                    day.Insert(dayworker, Day.WorkType.Day);
                }
                else
                {
                    var latestWorkTypes = latestDay.WorkTypes;
                    /*
                     다음과 같이 가정해도 일반성을 잃지 않음.
                    latestWorkTypes = [-1, 0, 1] 이라고하자.
                    1) dayworker가 0인 경우
                    다음 날은 [0, 1, -1]
                    2) dayworker가 1인 경우
                    다음 날은 [1, 0, -1]
                    3) dayworker가 2인 경우
                    다음날은 [0, 1, -1]
                    
                    즉 다음과 같은 과정으로 진행하면됨
                    1.dayworker에 상관없이 근무를 뒤집는다 즉, [1, 0, -1]이 된다.
                    2.이전날 dayworker가 주간근무인 경우 바꾸지 않는다 그렇지 않으면, 뒤집었을때의 0과 1을 permute한다.
                     
                     */


                    foreach (var workTypes in latestWorkTypes)
                    {
                        switch(workTypes)
                        {
                            case Day.WorkType.Dayoff:
                                day.Append(Day.WorkType.Night); break;
                            case Day.WorkType.Night:
                                day.Append(Day.WorkType.Dayoff); break;
                            case Day.WorkType.Day:
                                day.Append(Day.WorkType.Day); break;
                        }
                    }

                    if(latestWorkTypes[dayworker] != Day.WorkType.Day)
                    {
                        day.PermuteWork(Array.IndexOf(latestWorkTypes, Day.WorkType.Day), Array.IndexOf(latestWorkTypes, Day.WorkType.Night));
                    }
                    
                }
                latestDay = day;

            }

            return this;

        }

        

    }

    public class Blocks
    {
        private List<Block> blocks;
        public int Length { get { return blocks.Count; } }
        public int[] WorkCount
        {
            get
            {
                var result = new int[] { 0, 0, 0 };
                foreach(var block in blocks)
                {
                    var workCount = block.WorkCount;
                    for(int i = 0; i < workCount.Length; i++)
                    {
                        result[i] += workCount[i];
                    }
                }
                return result;
            }
        }

        public float Score { get
            {
                float result = 0.0f;
                foreach(var block in blocks)
                {
                    result += block.Score;
                }
                return result;
            } }


        public Blocks(Blocks blocks)
        {
            this.blocks = blocks.blocks.Select(block => new Block(block)).ToList();
        }

    }

    
}
