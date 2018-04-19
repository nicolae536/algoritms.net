using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace algorithms.generic_algorithms
{
    class GraphNode<TNode>
    {
        public TNode parent;
        public List<TNode> adicencyList;
    }

    class AdiacencyGraph<TNode> : ICollection<TNode> where TNode : IComparable
    {
        Dictionary<TNode, GraphNode<TNode>> nodesDictionary;

        AdiacencyGraph()
        {
            nodesDictionary = new Dictionary<TNode, GraphNode<TNode>>();
        }

        public int Count => nodesDictionary.Count;

        public bool IsReadOnly => true;

        public void Add(TNode item)
        {
            if (nodesDictionary.ContainsKey(item))
            {
                throw new DuplicateWaitObjectException();
            }

            nodesDictionary.Add(item, new GraphNode<TNode> { parent = item, adicencyList = new List<TNode>() });
        }

        public void AddVertex(TNode item, TNode neighboar, bool doubleWay = false)
        {
            if (!nodesDictionary.ContainsKey(item))
            {
                nodesDictionary.Add(item, new GraphNode<TNode> { parent = item, adicencyList = new List<TNode>() });
            }

            GraphNode<TNode> nodeItem = nodesDictionary.GetValueOrDefault(item);
            if (ContainsVertex(item, neighboar))
            {
                throw new DuplicateWaitObjectException();
            }

            nodeItem.adicencyList.Add(neighboar);

            if (doubleWay)
            {
                AddVertex(neighboar, item, false);
            }
        }

        public bool ContainsVertex(TNode parent, TNode child)
        {
            return nodesDictionary.ContainsKey(parent) &&
                nodesDictionary.GetValueOrDefault(parent).adicencyList.Contains(child);
        }

        public void Clear()
        {
            nodesDictionary = new Dictionary<TNode, GraphNode<TNode>>();
        }

        public bool Contains(TNode item)
        {
            return nodesDictionary.ContainsKey(item);
        }

        public void CopyTo(TNode[] array, int arrayIndex)
        {
            array = new TNode[array.Count() + nodesDictionary.Count];
            int i = arrayIndex;
            foreach (TNode item in nodesDictionary.Keys)
            {
                array[i] = item;
                i++;
            }
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            return nodesDictionary.Keys.GetEnumerator();
        }

        public bool Remove(TNode item)
        {
            if (!nodesDictionary.ContainsKey(item))
            {
                throw new KeyNotFoundException();
            }

            foreach (TNode adiacentNode in nodesDictionary.GetValueOrDefault(item).adicencyList)
            {
                GraphNode<TNode> neghboar = nodesDictionary.GetValueOrDefault(adiacentNode);

                if (neghboar != null)
                {
                    neghboar.adicencyList = neghboar.adicencyList.Where(it => it.CompareTo(item) != 0).ToList();
                }
            }

            nodesDictionary.Remove(item);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return nodesDictionary.GetEnumerator();
        }
    }
}
