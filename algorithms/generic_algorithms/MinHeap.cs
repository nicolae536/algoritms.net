using System;
using System.Collections;
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

    public class MaxPriorityHeap<TNode> : IEnumerable<TNode>, IList<TNode> where TNode : IComparable
    {
        protected List<HeapNode<TNode>> heapArray;
        protected HashSet<TNode> nodesDefinitions = new HashSet<TNode>();

        public MaxPriorityHeap()
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

        public bool IsReadOnly => true;

        public TNode this[int index]
        {
            get => IsInHeap(index) ? heapArray[index].node : default(TNode);
            set
            {
                if (!IsInHeap(index))
                {
                    throw new IndexOutOfRangeException();
                }

                heapArray[index].node = value;
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

        public void UpdatePriority(TNode item, IComparable priority)
        {
            int index = IndexOf(item);


            if (IsInHeap(index))
            {
                heapArray.RemoveAt(index);
                Add(item, priority);
            }

        }

        protected virtual void HeapifyDown(int startIndex = 0)
        {
            int index = startIndex;

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

        protected virtual void HeapifyUp(int startIndex = int.MinValue)
        {
            int index = startIndex == int.MinValue ? heapArray.Count - 1 : startIndex;

            while (HasParent(index) &&
                Parent(index).key.CompareTo(heapArray[index].key) == -1)
            {
                int parentIndex = GetParentIndex(index);
                Swap(parentIndex, index);
                index = parentIndex;
            }
        }

        protected HeapNode<TNode> Parent(int index)
        {
            return heapArray[GetParentIndex(index)];
        }

        protected HeapNode<TNode> LeftChild(int index)
        {
            return heapArray[GetLeftChildIndex(index)];
        }

        protected HeapNode<TNode> RightChild(int index)
        {
            return heapArray[GetRightChildIndex(index)];
        }

        protected bool HasParent(int index)
        {
            int indx = GetParentIndex(index);
            return IsInHeap(indx);
        }

        protected bool HasLeftChild(int index)
        {
            int indx = GetLeftChildIndex(index);
            return IsInHeap(indx);
        }

        protected bool HasRightChild(int index)
        {
            int indx = GetRightChildIndex(index);
            return IsInHeap(indx);
        }

        protected bool IsInHeap(int index)
        {
            return heapArray.Count > 0 && index >= 0 && index < heapArray.Count;
        }

        protected int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        protected int GetLeftChildIndex(int index)
        {
            return index * 2 + 1;
        }

        protected int GetRightChildIndex(int index)
        {
            return index * 2 + 2;
        }

        protected void Swap(int index1, int index2)
        {
            HeapNode<TNode> node = heapArray[index1];

            heapArray[index1] = heapArray[index2];
            heapArray[index2] = node;
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            return new HeapEnumerator
            {
                heapArray = this,
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new HeapEnumerator
            {
                heapArray = this,
            };
        }

        public int IndexOf(TNode item)
        {
            for (int i = 0; i < heapArray.Count; i++)
            {
                if (heapArray[i].node.CompareTo(item) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, TNode item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            if (IsInHeap(index))
            {
                heapArray.RemoveAt(index);
                if (HasParent(index))
                {
                    HeapifyDown(GetParentIndex(index));
                }
                else if (HasLeftChild(index))
                {
                    HeapifyUp(GetLeftChildIndex(index));
                }
                else if (HasRightChild(index))
                {
                    HeapifyUp(GetRightChildIndex(index));
                }
            }
        }

        public void Add(TNode item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(TNode[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TNode item)
        {
            throw new NotImplementedException();
        }

        class HeapEnumerator : IEnumerator<TNode>
        {
            public MaxPriorityHeap<TNode> heapArray;
            public int currentIndex = -1;

            public object Current => heapArray.IsInHeap(currentIndex) ? heapArray[currentIndex] : default(TNode);

            TNode IEnumerator<TNode>.Current => heapArray.IsInHeap(currentIndex) ? heapArray[currentIndex] : default(TNode);

            public void Dispose()
            {
                heapArray = null;
                currentIndex = -1;
            }

            public bool MoveNext()
            {
                if (!heapArray.IsInHeap(currentIndex + 1))
                {
                    return false;
                }
                currentIndex++;
                return true;
            }

            public void Reset()
            {
                currentIndex = -1;
            }
        }
    }

    public class MinPriorityHeap<TNode> : MaxPriorityHeap<TNode> where TNode : IComparable
    {
        protected override void HeapifyDown(int startIndex = 0)
        {
            int index = startIndex;

            while (HasLeftChild(index))
            {
                int smallChildIndex = GetLeftChildIndex(index);
                HeapNode<TNode> leftChild = LeftChild(index);


                if (HasRightChild(index) && RightChild(index).key.CompareTo(leftChild.key) == -1)
                {
                    smallChildIndex = GetRightChildIndex(index);
                }

                if (heapArray[index].key.CompareTo(heapArray[smallChildIndex].key) == -1)
                {
                    return;
                }

                Swap(index, smallChildIndex);
                index = smallChildIndex;
            }
        }

        protected override void HeapifyUp(int startIndex = int.MinValue)
        {
            int index = startIndex == int.MinValue ? heapArray.Count - 1 : startIndex;

            while (HasParent(index) &&
                Parent(index).key.CompareTo(heapArray[index].key) == 1)
            {
                int parentIndex = GetParentIndex(index);
                Swap(parentIndex, index);
                index = parentIndex;
            }
        }
    }
}
