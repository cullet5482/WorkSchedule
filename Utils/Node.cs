using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Date;

namespace WorkSchedule
{
    public class Node
    {
        Blocks blocks;
        public Blocks Blocks { get { return blocks; } }
        List<Node> children = new List<Node>();

        int Loss
        {
            get
            {
                return blocks.Loss;
            }
        }

        public Node AddChild(Node child)
        {
            children.Add(child);
            return this;
        }

        public Node(Blocks blocks)
        {
            this.blocks = blocks;
        }

        public Node Copy()
        {
            return new Node(blocks.Clone());
        }

        static private Node FillNode(Node node, int idx, Block.FillMethod method = Block.FillMethod.First)
        {
            node.blocks.Fill(idx, method);
            return node;
        }

        static public List<Node> WaterTree(Node node)
        {
            var result = new List<Node>();
            var minLoss = int.MaxValue;
            WaterTree(node, result, ref minLoss);
            return result;
        }
        static private void WaterTree(Node node, List<Node> bestNode, ref int minLoss, int depth = 0)
        {
            if(depth >= node.blocks.Length)
            {
                var loss = node.Loss;
                if(loss < minLoss)
                {
                    minLoss = loss;
                    bestNode.Clear();
                    bestNode.Add(node);
                    

                }else if(loss == minLoss)
                {
                    bestNode.Add(node);
                }
                return;
            }

            if(!node.blocks.FirstEmptyAt(depth))
            {
                var filledNode = node.Copy();
                node.AddChild(FillNode(filledNode, depth));
                WaterTree(filledNode, bestNode, ref minLoss, depth + 1);
            }
            else
            {
                var leftNode = node.Copy();
                var rightNode = node.Copy();
                FillNode(leftNode, depth, Block.FillMethod.First);
                FillNode(rightNode, depth, Block.FillMethod.Second);

                node.AddChild(leftNode);
                node.AddChild(rightNode);
                WaterTree(leftNode, bestNode, ref minLoss, depth+1);
                WaterTree(rightNode, bestNode, ref minLoss, depth + 1);
            }

        }
    }
}
