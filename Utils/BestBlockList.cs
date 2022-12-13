using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Date;

namespace WorkSchedule
{
    [Serializable]
    public class BestBlockList : List<Blocks>
    {
        int maxlen;


        public float BestScore
        {
            get
            {
                int count = Count;
                if (count <= 0)
                    return float.MaxValue;
                else
                    return this.Max(b => b.Score);

            }
        }
        public float LowScore
        {
            get
            {
                int count = Count;
                if (count < 0)
                {
                    return float.MinValue;
                }
                else
                    return this.Min(b => b.Score);
            }
        }
        
        public int Loss
        {
            get
            {
                int count = Count;
                if (count <= 0)
                    return int.MaxValue;
                else
                    return this[0].Loss;

            }
        }
        
        public BestBlockList(int maxlen)
        {
            this.maxlen = maxlen;
        }
        
        public new BestBlockList Add(Blocks blocks)
        {
            
            if(blocks.Loss < Loss)
            {
                Clear();
                base.Add(blocks);
                return this;
            }
            else if (blocks.Loss > Loss)
            {
                return this;
            }

            if (this.Any(b => b == blocks)) return this;
            if (Count >= maxlen)
            {
                if (blocks.Score <= LowScore)
                    return this;
                else
                {
                    Remove(Find(b => b.Score == LowScore));
                }

            }
            
            base.Add(blocks);
            return this;
            
            
        }
    }
}
