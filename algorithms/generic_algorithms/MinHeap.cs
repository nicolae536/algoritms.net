using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms
{
    public class HeapNode<TNode>
    {
        // 0 => equality
        // -1 this is > then compared
        // 1 this is < than compared
        public IComparable key;
        public TNode node;
    }

    public class PriorityHeap<TNode>
    {
        List<HeapNode<TNode>> heapArray;
        HashSet<TNode> nodesDefinitions = new HashSet<TNode>();

        public PriorityHeap()
        {
            heapArray = new List<HeapNode<TNode>>();
        }

        public int Count
        {
            get
            {
                return heapArray.Count;
            }
        }

        public bool Contains(TNode item)
        {
            return nodesDefinitions.Contains(item);
        }

        public TNode Peek()
        {
            if (Count == 0)
            {
                throw new IndexOutOfRangeException();
            }

            return heapArray[0].node;
        }

        public TNode Pool()
        {
            if (Count == 0)
            {
                throw new IndexOutOfRangeException();
            }

            HeapNode<TNode> hNode = heapArray[0];
            heapArray.Remove(hNode);
            HeapifyDown();
            nodesDefinitions.Remove(hNode.node);
            return hNode.node;
        }

        public void Add(TNode item, IComparable priority)
        {
            if (!nodesDefinitions.Contains(item))
            {
                nodesDefinitions.Add(item);
            }

            HeapNode<TNode> node = new HeapNode<TNode>
            {
                key = priority,
                node = item
            };
            heapArray.Add(node);
            HeapifyUp();
        }

        private void HeapifyDown()
        {
            int index = 0;

            while (HasLeftChild(index))
            {
                int smallChildIndex = GetLeftChildIndex(index);
                HeapNode<TNode> leftChild = LeftChild(index);


                if (HasRightChild(index) && RightChild(index).key.CompareTo(leftChild.key) == 1)
                {
                    smallChildIndex = GetRightChildIndex(index);
                }

                if (heapArray[index].key.CompareTo(heapArray[smallChildIndex].key) == 1)
                {
                    return;
                }

                Swap(index, smallChildIndex);
                index = smallChildIndex;
            }
        }

        private void HeapifyUp()
        {
            int index = heapArray.Count - 1;

            while (HasParent(index) &&
                Parent(index).key.CompareTo(heapArray[index].key) == -1)
            {
                int parentIndex = GetParentIndex(index);
                Swap(parentIndex, index);
                index = parentIndex;
            }
        }

        private HeapNode<TNode> Parent(int index)
        {
            return heapArray[GetParentIndex(index)];
        }

        private HeapNode<TNode> LeftChild(int index)
        {
            return heapArray[GetLeftChildIndex(index)];
        }

        private HeapNode<TNode> RightChild(int index)
        {
            return heapArray[GetRightChildIndex(index)];
        }

        private bool HasParent(int index)
        {
            int indx = GetParentIndex(index);
            return IsInHeap(indx);
        }

        private bool HasLeftChild(int index)
        {
            int indx = GetLeftChildIndex(index);
            return IsInHeap(indx);
        }

        private bool HasRightChild(int index)
        {
            int indx = GetRightChildIndex(index);
            return IsInHeap(indx);
        }

        private bool IsInHeap(int index)
        {
            return index > 0 && index < heapArray.Count;
        }

        private int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        private int GetLeftChildIndex(int index)
        {
            return index * 2 + 1;
        }

        private int GetRightChildIndex(int index)
        {
            return index * 2 + 2;
        }

        private void Swap(int index1, int index2)
        {
            HeapNode<TNode> node = heapArray[index1];

            heapArray[index1] = heapArray[index2];
            heapArray[index2] = node;
        }
    }
}
