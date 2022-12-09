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

        public Day[] Days
        {
            get
            {
                return days.ToArray();
            }
        }
        public bool FirstEmpty
        {
            get
            {
                return days[0].Empty;
            }
        }

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

        public Block Copy()
        {
            return new Block(days.Select(day => day.Clone()).ToList(), dayworker);
            
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
        public Block Fill(FillMethod method)
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

        public int Loss { get
            {
                int result = 0;
                foreach(int count in WorkCount)
                {
                    result += Math.Abs(count);
                }
                return result;
            } }

        
        
        public bool FirstEmptyAt(int idx)
        {
            return blocks[idx].FirstEmpty;
        }

        public Blocks Fill(int idx, Block.FillMethod method)
        {
            blocks[idx].Fill(method);
            return this;
        }

        public Blocks(List<Block> blocks)
        {
            this.blocks = blocks;
        }

        public Blocks Clone()
        {
            return new Blocks(blocks.Select(block => block.Copy()).ToList());
            
        }

        public (List<Blocks>, int, float) PermuteByBestWay()
        {
            ///permute 1번 진행시 Loss가 가장 낮은 Blocks들 중에서, 가장 좋은 Score를 기록한 Blocks들을 반환

            float bestScore = float.MinValue;
            List<Blocks> bestBlocks = new List<Blocks>();
            int bestLoss = int.MaxValue;
            var count = WorkCount;

            for(int i=0; i<Length; i++)
            {
                var block = blocks[i];
                if(block.FirstEmpty)
                {
                    continue;
                }

                var days = block.Days;
                for(int j=0; j<days.Length; j++)
                {
                    var workTypes = days[j].WorkTypes;
                    var negativeWorker = new List<int>();
                    var positiveWorker = new List<int>();
                    for(int k =0; k<workTypes.Length; k++)
                    {
                        if(count[k] < 0)
                        {
                            negativeWorker.Add(k);
                        }else if(count[k] > 0)
                        {
                            positiveWorker.Add(k);
                        }
                    }
                    foreach(int n in negativeWorker)
                    {
                        foreach(int p in positiveWorker)
                        {
                            if((int)workTypes[p] >= 0 && (int)workTypes[n] <= -1)
                            {
                                var tempBlocks = Clone();
                                if(tempBlocks.blocks[i].PermuteWork(j, n, p))
                                {
                                    var loss = tempBlocks.Loss;
                                    if (loss > bestLoss)
                                    {
                                        continue;
                                    }else if(loss < bestLoss)
                                    {
                                        bestLoss = loss;
                                        bestBlocks.Clear();
                                        bestScore = float.MinValue;
                                    }
                                    var score = tempBlocks.Score;
                                    if(score > bestScore)
                                    {
                                        bestScore = score;
                                        bestBlocks.Clear();
                                        bestBlocks.Add(tempBlocks);
                                    }else if(score == bestScore)
                                    {
                                        bestBlocks.Add(tempBlocks);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            (List<Blocks> blocks, int loss, float score) result = (bestBlocks, bestLoss, bestScore);
            return result;

        }

    }

    
}
