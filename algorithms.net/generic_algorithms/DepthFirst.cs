using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms.net
{
    public class DepthFirst<TNode> where TNode : IComparable
    {
        IAStarHeuristic<TNode> heuristicImpl;
        List<TNode> visitedVertexes;
        Dictionary<TNode, TNode> parentToNode;

        public DepthFirst(IAStarHeuristic<TNode> heuristicImpl)
        {
            this.heuristicImpl = heuristicImpl;
        }

        public List<TNode> FindFirstPath(TNode start, TNode goal)
        {
            if (start.CompareTo(goal) == 0)
            {
                return new List<TNode>();
            }

            visitedVertexes.Add(start);
            foreach (TNode node in heuristicImpl.GetNeighboars(start))
            {
                bool isVisited = visitedVertexes.FirstOrDefault(it => it.CompareTo(node) == 0) != null;

                if (!isVisited)
                {
                    List<TNode> result = FindFirstPath(start, goal);

                    if (result != null)
                    {
                        result.Insert(0, start);
                        return result;
                    }
                }
            }

            return null;
        }
    }
}
