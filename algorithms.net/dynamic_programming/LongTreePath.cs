using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms.net.dynamic_programming
{
    class LongTreePath
    {
        class Edge
        {
            public int node1;
            public int node2;
        }

        class TreeNode
        {
            public int value;
            public TreeNode parent;
            public int height;
            public int label;
            public List<TreeNode> childrens = new List<TreeNode>();
        }

        List<int> a;
        List<TreeNode> adiacency;
        int currentMax;

        LongTreePath(string inputs)
        {
            a = new List<int>();
            adiacency = new List<TreeNode>();

            string[] data = inputs.Split(' ');
            string[] aRr = data[0].Split(',');

            for (int i = 0; i < aRr.Length; i++)
            {
                a.Add(Int32.Parse(aRr[i]));
            }
            aRr = data[1].Split(',');
            int currentHeight = 0;
            int aIndex = 0;

            for (int i = 0; i < aRr.Length; i += 2)
            {
                int node1 = Int32.Parse(aRr[i]), node2 = Int32.Parse(aRr[i + 1]);

                TreeNode node = adiacency.FirstOrDefault(it => it.value == node1);
                if (node == null)
                {
                    node = new TreeNode
                    {
                        parent = null,
                        value = node1,
                        label = a[aIndex],
                        height = currentHeight
                    };
                    aIndex++;
                    adiacency.Add(node);
                }

                TreeNode child = adiacency.FirstOrDefault(it => it.value == node2);

                if (child == null)
                {
                    child = new TreeNode
                    {
                        parent = node,
                        value = node2,
                        label = a[aIndex],
                        height = node.height + 1
                    };
                    aIndex++;
                    node.childrens.Add(child);
                    adiacency.Add(child);
                }
            }

            Console.WriteLine("Max path on tree " + FindMaxPath());
        }

        int FindMaxPath()
        {
            int maxPath = int.MinValue;

            foreach (TreeNode item in adiacency)
            {
                foreach (TreeNode item1 in adiacency)
                {
                    if (item1 == item || item.label != item1.label)
                    {
                        continue;
                    }

                    int path = FindPath(new Dictionary<TreeNode, bool>(), item, item1, 0);

                    if (path != -1 && path - 1 > maxPath)
                    {
                        maxPath = path - 1;
                    }
                }
            }

            return maxPath;
        }

        int FindPath(Dictionary<TreeNode, bool> removedNodes, TreeNode startNode, TreeNode endNode, int pathLenght)
        {
            if (startNode == endNode)
            {
                return pathLenght;
            }


            // Try to find path going on node childrens
            foreach (TreeNode child in startNode.childrens)
            {
                // if we navigate up and then explore the childrens we should not explore the subtree
                // which we are comming from cause this it means that we don't have the node which we are
                // looking for as a child of the previous node
                if (child.label == startNode.label &&
                    !removedNodes.ContainsKey(child))
                {
                    int newPath = FindPath(removedNodes, child, endNode, pathLenght + 1);

                    if (newPath != -1)
                    {
                        return newPath;
                    }
                }
            }

            // if the node parent wasn't visited this tourn it means that we cand try to go one level up and
            // and on the childrens like in this example
            //----------1, 1
            //
            //-----1, 2        1, 3
            //
            //  2, 4      2, 5
            if (!removedNodes.ContainsKey(startNode.parent) &&
                startNode.parent != null &&
                startNode.parent.label == endNode.label)
            {
                if (!removedNodes.ContainsKey(startNode))
                {
                    removedNodes.Add(startNode, true);
                }
                removedNodes.Add(startNode.parent, true);
                int newPath = FindPath(removedNodes, startNode.parent, endNode, pathLenght + 1);

                if (newPath != -1)
                {
                    return newPath;
                }
            }

            return -1;
        }

        public static void RunProblem()
        {
            string[] inputs =
           {
                "1,1,1,2,2,1 1,2,1,3,2,4,2,5,3,6",
                "1,1,1,2,2 1,2,1,3,2,4,2,5",
                "1,1,1,2,2,1,2,2,2 1,2,1,3,2,4,2,5,3,6,5,7,7,8,8,9",
            };

            foreach (string item in inputs)
            {
                new LongTreePath(item);
            }
        }
    }
}
