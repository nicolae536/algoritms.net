using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms.net.dynamic_programming
{
    class Painter : IComparable
    {
        public int id = 0;
        public int currentIndex = 0;
        public int startIndex = 0;
        public int numberOfBoards = 0;
        public bool wasReseted = false;

        public int CompareTo(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return -1;
            }

            if (id == ((Painter)obj).id)
            {
                return 0;
            }

            if (id > ((Painter)obj).id)
            {
                return -1;
            }

            return 1;
        }
    }

    public class PainterPartition
    {
        int painters = 0;
        List<int> boardsToPaint1;
        List<int> boardsToPaint;

        public PainterPartition(string input)
        {
            string[] data = input.Split(',');
            painters = Int32.Parse(data[0]);
            boardsToPaint1 = new List<int>();
            boardsToPaint = new List<int>();

            for (int i = 1; i < data.Length; i++)
            {
                boardsToPaint.Add(Int32.Parse(data[i]));
                boardsToPaint1.Add(Int32.Parse(data[i]));
            }

            Console.WriteLine("Naice => Min time to paint partitions " + StartPainting());
            Console.WriteLine("Min time to paint partitions " + Partition());
        }

        public int StartPainting()
        {
            int partitionSplit = painters > 1
                ? boardsToPaint1.Count / (painters - 1)
                : 1;
            MinPriorityHeap<Painter> activePaintIndexes = new MinPriorityHeap<Painter>();

            int currentPartitionIndex = 0;
            for (int i = 1; i < painters + 1; i++)
            {
                if (currentPartitionIndex >= boardsToPaint1.Count)
                {
                    continue;
                }

                activePaintIndexes.Add(new Painter()
                {
                    id = i,
                    currentIndex = currentPartitionIndex,
                    startIndex = currentPartitionIndex,
                    numberOfBoards = boardsToPaint1[currentPartitionIndex]
                }, boardsToPaint1[currentPartitionIndex]);
                currentPartitionIndex += partitionSplit - 1;
            }

            List<Painter> todoPainting = new List<Painter>();
            int minPaintingSeconds = 0;

            while (activePaintIndexes.Count > 0)
            {
                Painter minValue = activePaintIndexes.Pool();
                todoPainting.Add(minValue);
                while (activePaintIndexes.Count > 0 &&
                    activePaintIndexes.Peek().numberOfBoards == minValue.numberOfBoards)
                {
                    todoPainting.Add(activePaintIndexes.Pool());
                }

                foreach (Painter item in activePaintIndexes)
                {
                    item.numberOfBoards = item.numberOfBoards - minValue.numberOfBoards;
                    activePaintIndexes.UpdatePriority(item, item.numberOfBoards);
                    boardsToPaint1[item.currentIndex] = item.numberOfBoards;
                }
                minPaintingSeconds += minValue.numberOfBoards;

                foreach (Painter item in todoPainting)
                {
                    boardsToPaint1[item.currentIndex] = 0;
                }

                foreach (Painter item in todoPainting)
                {
                    // go foreward if you can and paint more
                    if (IsInList(boardsToPaint1, item.currentIndex + 1) &&
                        boardsToPaint1[item.currentIndex + 1] != 0)
                    {
                        item.currentIndex++;
                        item.numberOfBoards = boardsToPaint1[item.currentIndex];
                        activePaintIndexes.Add(item, item.numberOfBoards);
                        continue;
                    }

                    // reset to the starting point and try to paint back
                    if (!item.wasReseted)
                    {
                        item.currentIndex = item.startIndex;
                        item.wasReseted = true;
                    }

                    // if the painter was reseted we try to paint backwards
                    if (item.wasReseted &&
                        (IsInList(boardsToPaint1, item.currentIndex - 1) &&
                    boardsToPaint1[item.currentIndex - 1] != 0))
                    {
                        item.currentIndex--;
                        item.numberOfBoards = boardsToPaint1[item.currentIndex];
                        activePaintIndexes.Add(item, item.numberOfBoards);
                    }
                }

                todoPainting = new List<Painter>();

            }

            return minPaintingSeconds;
        }

        private int GetMax()
        {
            int max = int.MinValue;

            for (int i = 0; i < boardsToPaint.Count; i++)
            {
                if (boardsToPaint[i] > max)
                {
                    max = boardsToPaint[i];
                }
            }

            return max;
        }

        private int GetSum()
        {
            int sum = 0;

            for (int i = 0; i < boardsToPaint.Count; i++)
            {
                sum += boardsToPaint[i];
            }

            return sum;
        }

        private int GetPaintersToPaintPartition(int maxAllowedPartition)
        {
            int total = 0, numPainters = 1;

            for (int i = 0; i < boardsToPaint.Count; i++)
            {
                total += boardsToPaint[i];

                if (total > maxAllowedPartition)
                {
                    total = boardsToPaint[i];
                    numPainters++;
                }
            }

            return numPainters;
        }

        private int Partition()
        {
            // get the maximum from the array (the biggest partition)
            int lo = GetMax();
            // get the sum of the array => this represents the maximum value which can be outputed            
            int hi = GetSum();

            while (lo < hi)
            {
                // compute a partition length between the maximum value and the current biggest partition
                int newPartitionLength = lo + (hi - lo) / 2;
                // get how many painters we need to use if we have this partition                
                int requiredPainters = GetPaintersToPaintPartition(newPartitionLength);

                if (requiredPainters <= painters)
                {
                    // if we are less or equel then the number of painters our partition is to small 
                    // => decrese the hi so the partition will increase
                    hi = newPartitionLength;
                }
                else
                {
                    // if we need more painters for this king of partition
                    // we need to increase the partition so a painter will paint more
                    lo = newPartitionLength + 1;
                }
            }

            return lo;
        }

        private bool IsInList(List<int> list, int index)
        {
            return index < list.Count && index >= 0;
        }

        public static void RunProblem()
        {
            string[] inputs =
            {
                "2,10,10,10,10",
                "2,10,20,30,40",
                "3,1,2,3,4,5,6,7,8,9"
            };

            foreach (string item in inputs)
            {
                new PainterPartition(item);
            }
        }
    }
}
