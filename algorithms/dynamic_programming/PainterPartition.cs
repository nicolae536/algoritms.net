using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms.dynamic_programming
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
        List<int> boardsToPaint;

        public PainterPartition(string input)
        {
            string[] data = input.Split(',');
            painters = Int32.Parse(data[0]);
            boardsToPaint = new List<int>();

            for (int i = 1; i < data.Length; i++)
            {
                boardsToPaint.Add(Int32.Parse(data[i]));
            }

            Console.WriteLine("Min time to paint partitions " + StartPainting());
        }

        public int StartPainting()
        {
            int partitionSplit = painters > 1
                ? boardsToPaint.Count / (painters - 1)
                : 1;
            MinPriorityHeap<Painter> activePaintIndexes = new MinPriorityHeap<Painter>();

            int currentPartitionIndex = 0;
            for (int i = 1; i < painters + 1; i++)
            {
                if (currentPartitionIndex >= boardsToPaint.Count)
                {
                    continue;
                }

                activePaintIndexes.Add(new Painter()
                {
                    id = i,
                    currentIndex = currentPartitionIndex,
                    startIndex = currentPartitionIndex,
                    numberOfBoards = boardsToPaint[currentPartitionIndex]
                }, boardsToPaint[currentPartitionIndex]);
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
                    boardsToPaint[item.currentIndex] = item.numberOfBoards;
                }
                minPaintingSeconds += minValue.numberOfBoards;

                foreach (Painter item in todoPainting)
                {
                    boardsToPaint[item.currentIndex] = 0;
                }

                foreach (Painter item in todoPainting)
                {
                    // go foreward if you can and paint more
                    if (IsInList(boardsToPaint, item.currentIndex + 1) &&
                        boardsToPaint[item.currentIndex + 1] != 0)
                    {
                        item.currentIndex++;
                        item.numberOfBoards = boardsToPaint[item.currentIndex];
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
                        (IsInList(boardsToPaint, item.currentIndex - 1) &&
                    boardsToPaint[item.currentIndex - 1] != 0))
                    {
                        item.currentIndex--;
                        item.numberOfBoards = boardsToPaint[item.currentIndex];
                        activePaintIndexes.Add(item, item.numberOfBoards);
                    }
                }

                todoPainting = new List<Painter>();

            }

            return minPaintingSeconds;
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
            };

            foreach (string item in inputs)
            {
                new PainterPartition(item);
            }
        }
    }
}
