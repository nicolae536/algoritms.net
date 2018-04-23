using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms
{
    public interface IAStarHeuristic<TNode>
    {
        double HeuristicDistanceBetween(TNode start, TNode goal);

        IEnumerable<TNode> GetNeighboars(TNode node);
    }

    public class AStar<TNode> where TNode : IComparable
    {
        // The set of nodes already evaluated
        Dictionary<TNode, bool> nodesVisited;

        // The set of currently discovered nodes that are not evaluated yet.
        // Initially, only the start node is known.
        MaxPriorityHeap<TNode> notEvaluatedNodes;

        // For each node, which node it can most efficiently be reached from.
        // If a node can be reached from many nodes, cameFrom will eventually contain the
        // most efficient previous step.
        Dictionary<TNode, TNode> nodeToParent;

        // For each node, the cost of getting from the start node to that node.
        Dictionary<TNode, double> costFromStartToNode;

        // For each node, the total cost of getting from the start node to the goal
        // by passing by that node. That value is partly known, partly heuristic.
        //Dictionary<TNode, double> costToGoal;

        IAStarHeuristic<TNode> heuristicImpl;

        public AStar(IAStarHeuristic<TNode> heuristicImpl)
        {
            this.heuristicImpl = heuristicImpl;
        }

        public IEnumerable<TNode> FindPath(TNode start, TNode goal)
        {
            InitializeSets(start, goal);
            while (notEvaluatedNodes.Count > 0)
            {
                TNode current = notEvaluatedNodes.Peek();

                if (current.CompareTo(goal) == 0)
                {
                    // TODO reconstruct_path
                    return ReconstructPath(current);
                }

                notEvaluatedNodes.Pool();
                nodesVisited.Add(current, true);
                foreach (TNode neighboar in heuristicImpl.GetNeighboars(current))
                {
                    if (nodesVisited.ContainsKey(neighboar))
                    {
                        continue;
                    }

                    // compute heuristic cost
                    double tentativeDistance = costFromStartToNode.GetValueOrDefault(current) + heuristicImpl.HeuristicDistanceBetween(current, neighboar);
                    // This is not a better path.
                    double costOrDefault = costFromStartToNode.ContainsKey(neighboar) ? costFromStartToNode.GetValueOrDefault(neighboar) : double.MaxValue;

                    if (tentativeDistance >= costOrDefault)
                    {
                        AddWithPriotiry(neighboar, int.MaxValue);
                        continue;
                    }

                    // This path is the best until now. Record it!                
                    ReplaceInDictionary(nodeToParent, neighboar, current);
                    ReplaceInDictionary(costFromStartToNode, neighboar, tentativeDistance);
                    //ReplaceInDictionary(costToGoal, neighboar, heuristicImpl.DistanceBetween(neighboar, goal));
                    AddWithPriotiry(neighboar, heuristicImpl.HeuristicDistanceBetween(neighboar, goal));

                    //notEvaluatedNodes = notEvaluatedNodes.OrderBy(keyIt => costToGoal.GetValueOrDefault(keyIt.Key)).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
                }
            }

            return null;
        }

        private void InitializeSets(TNode start, TNode goal)
        {
            nodesVisited = new Dictionary<TNode, bool>();
            notEvaluatedNodes = new MaxPriorityHeap<TNode>();
            notEvaluatedNodes.Add(start, heuristicImpl.HeuristicDistanceBetween(start, goal));
            Dictionary<TNode, TNode> dictionary = new Dictionary<TNode, TNode>();
            nodeToParent = dictionary;

            costFromStartToNode = new Dictionary<TNode, double>
            {
                { start, 0 }
            };
            //costToGoal = new Dictionary<TNode, double>
            //{
            //    { start, heuristicImpl.DistanceBetween(start, goal) }
            //};
        }

        private void ReplaceInDictionary<T, K>(Dictionary<T, K> dict, T key, K value)
        {
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }

            dict.Add(key, value);
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

        private void AddWithPriotiry(TNode node, IComparable priority)
        {
            if (!notEvaluatedNodes.Contains(node))
            {
                notEvaluatedNodes.Add(node, priority);
            }
        }
    }
}
