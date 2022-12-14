using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace WorkSchedule.Date
{
    [Serializable]
    public class Block
    {
        
        private List<Day> days;
        private int dayworker;
        public FillMethod FilledMethod { private set; get; }

        public int Week { get
            {
                return days[0].Week;
            } }

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
                        else if (i != dayworker && day.WorkTypes[i] == Day.WorkType.Night) result += 1.0f;
                        else if (day.Last) result += 0.5f;
                        

                    }
                }
                return result;

            } }
        
        public int Length { get { return days.Count; } }

        public Block Copy()
        {
            return new Block(days.Select(day => day.Clone()).ToList());
            
        }

        public Block(List<Day> days)
        {
            this.days = days;
            dayworker = WorkerDataManager.GetDayWorker(Week);

        }
        public Block[] Split(int idx)
        {
            
            var leftDays = new List<Day>();
            var rightDays = new List<Day>();
            for(int i = 0; i < days.Count; i++)
            {
                var day = days[i];
                if(i < idx)
                {
                    leftDays.Add(day);
                }else if(i > idx)
                {
                    rightDays.Add(day);
                }
            }
            var result = new List<Block>();
            if (rightDays.Count > 0)
            {
                result.Add(new Block(rightDays));
            }
            if (leftDays.Count > 0)
            {
                result.Add(new Block(leftDays));
            }
            

            return result.ToArray();
        }
        public bool PermuteWork(int idx, int a, int b)
        {
            
            if ((days[idx].Last || idx + 1 >= Length) && ((days[idx].WorkTypes[b] == Day.WorkType.Night && days[idx].WorkTypes[a] == Day.WorkType.Day) || (days[idx].WorkTypes[a] == Day.WorkType.Night && days[idx].WorkTypes[b] == Day.WorkType.Day)))
            {
                
                days[idx].PermuteWork(a, b);
                return true;
            }
            
            return false;

        }
        public Block SetFirstDay(Day.WorkType[] workTypes)
        {
            if(workTypes.Length != 3)
            {
                throw new ArgumentException("the length of workTypes is must be 3");
            }
            if(days[0].WorkTypes.Length != 0)
            {
                throw new Exception("when Setting first day, the first day muse be empty");
            }
            for(int i= 0; i < 3; i++)
            {
                days[0].Append(workTypes[i]);
            }
            return this;
        }

        public enum FillMethod { First = 0, Second = 1 };
        public Block Fill(FillMethod method)
        {
            FilledMethod = method;
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
                        day.PermuteWork(Array.IndexOf(day.WorkTypes, Day.WorkType.Day), Array.IndexOf(day.WorkTypes, Day.WorkType.Night));
                    }
                    
                }
                latestDay = day;

            }

            return this;

        }

        

    }

    [Serializable]
    public class Blocks
    {
        private List<Block> blocks;

        

        public void SplitBlock(int blockIdx, int dayIdx)
        {
            var targetBlock = blocks[blockIdx];
            blocks.RemoveAt(blockIdx);
            var splitedBlocks = targetBlock.Split(dayIdx);
            foreach(var splitedBlock in splitedBlocks)
            {
                if(splitedBlock.Length > 0)
                    blocks.Insert(blockIdx, splitedBlock);
            }
            
        }

        public static bool operator ==(Blocks a, Blocks b)
        {
            if (a.Length != b.Length) return false;
            for(int i=0; i<a.Length; i++)
            {
                for(int j=0; j<a.blocks[i].Days.Length; j++)
                {
                    if (!Day.Eqaul(a.blocks[i].Days[j], b.blocks[i].Days[j])) return false;
                }
                
            }
            return true;
        }

        public static bool operator !=(Blocks a, Blocks b)
        {
            return !(a == b);
        }

        public Day[] Days { get
            {
                var result = new List<Day>();
                foreach (var block in blocks)
                {
                    foreach(var day in block.Days)
                    {
                        result.Add(day);
                    }
                }
                return result.ToArray();


            } }
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

        public Block GetBlock(int idx)
        {
            return blocks[idx].Copy();
        }
            


        public Blocks SetFirstDay(int idx, Day.WorkType[] workTypes)
        {
            blocks[idx].SetFirstDay(workTypes);
            return this;
        }
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

        public List<Blocks> PermuteByBestWay()
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
            if(bestBlocks.Count <= 0)
            {
                bestBlocks.Add(this);
            }
            return bestBlocks;

        }

    }

    
}
