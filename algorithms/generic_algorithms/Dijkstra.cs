using System;
using System.Collections.Generic;


namespace algorithms
{

    public class DijkstraFindAllPathsResponse<TNode>
    {
        public Dictionary<TNode, double> distances;
        public Dictionary<TNode, TNode> nodeToParent;
    }

    public interface IDisktraModel<TNode> : IAStarHeuristic<TNode>
    {
        double VertexLength(TNode node, TNode neightboar);
    }

    public class Dijkstra<TNode> where TNode : IComparable
    {
        private Dictionary<TNode, double> distanceFromStartToNode;
        private MaxPriorityHeap<TNode> unexploredSet;
        // For each node, which node it can most efficiently be reached from.
        // If a node can be reached from many nodes, cameFrom will eventually contain the
        // most efficient previous step.
        Dictionary<TNode, TNode> nodeToParent;
        private IDisktraModel<TNode> heuristicImpl;

        public Dijkstra(IDisktraModel<TNode> heuristicImpl)
        {
            this.heuristicImpl = heuristicImpl;
        }

        public List<TNode> FindPath(TNode start, TNode goal)
        {
            DijkstraFindAllPathsResponse<TNode> resp = FindAllPaths(start);

            List<TNode> fastestPath = ReconstructPath(goal);
            if (fastestPath.Count == 1)
            {
                return null;
            }

            return fastestPath;
        }

        public DijkstraFindAllPathsResponse<TNode> FindAllPaths(TNode start)
        {
            InitSets(start);
            while (unexploredSet.Count > 0)
            {
                TNode current = unexploredSet.Pool();
                double distanceFromNode = distanceFromStartToNode.GetValueOrDefault(current);

                foreach (TNode neightboar in heuristicImpl.GetNeighboars(current))
                {
                    double distanceFromStart = distanceFromNode + heuristicImpl.VertexLength(current, neightboar);
                    double distanceToN = distanceFromStartToNode.ContainsKey(neightboar) ? distanceFromStartToNode.GetValueOrDefault(neightboar) : double.MaxValue;
                    if (distanceFromStart < distanceToN)
                    {
                        ReplaceInDictionary(distanceFromStartToNode, neightboar, distanceFromStart);
                        ReplaceInDictionary(nodeToParent, neightboar, current);
                        if (!unexploredSet.Contains(neightboar))
                        {
                            unexploredSet.Add(neightboar, distanceFromStart);
                        }
                    }
                }
            }

            return new DijkstraFindAllPathsResponse<TNode>
            {
                distances = distanceFromStartToNode,
                nodeToParent = nodeToParent
            };
        }

        private List<TNode> ReconstructPath(TNode current)
        {
            List<TNode> path = new List<TNode> { current };

            while (nodeToParent.ContainsKey(current))
            {
                current = nodeToParent.GetValueOrDefault(current);
                path.Add(current);
            }

            return path;
        }

        private void InitSets(TNode start)
        {

            distanceFromStartToNode = new Dictionary<TNode, double>
            {
                {start, 0 }
            };
            unexploredSet = new MaxPriorityHeap<TNode>();
            unexploredSet.Add(start, 0);
            nodeToParent = new Dictionary<TNode, TNode>();
        }

        private void ReplaceInDictionary<T, K>(Dictionary<T, K> dict, T key, K value)
        {
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }

            dict.Add(key, value);
        }
    }
}
