using System;
using System.Collections.Generic;

namespace algorithms.net
{
    public class BreathFirst<TNode> where TNode : IComparable
    {
        IAStarHeuristic<TNode> heuristicImpl;
        Queue<TNode> unexploredNodes;
        Dictionary<TNode, TNode> nodeToParent;

        public BreathFirst(IAStarHeuristic<TNode> heuristicImpl)
        {
            this.heuristicImpl = heuristicImpl;
        }

        public List<TNode> FindPath(TNode start, TNode goal)
        {
            InitSets(start);
            while (unexploredNodes.Count > 0)
            {
                TNode current = unexploredNodes.Dequeue();

                if (current.CompareTo(goal) == 0)
                {
                    return ReconstructPath(goal);
                }

                foreach (TNode neighboar in heuristicImpl.GetNeighboars(current))
                {
                    if (nodeToParent.ContainsKey(neighboar))
                    {
                        nodeToParent.Add(neighboar, current);
                        unexploredNodes.Enqueue(neighboar);
                    }
                }
            }

            return null;
        }

        private void InitSets(TNode start)
        {
            unexploredNodes = new Queue<TNode>();
            unexploredNodes.Enqueue(start);
            nodeToParent = new Dictionary<TNode, TNode>();
        }

        private List<TNode> ReconstructPath(TNode current)
        {
            List<TNode> path = new List<TNode> { current };

            while (nodeToParent.ContainsKey(current))
            {
                current = nodeToParent[current];
                path.Add(current);
            }

            return path;
        }
    }
}
