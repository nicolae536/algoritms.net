using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms.net
{
    public class SnackVendor
    {
        int numberOfCustomersIndex;

        int[] change = { 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] moneyValues = { 1, 2, 5, 10, 20, 50, 100, 200 };
        int matrixHeight;
        int matrixWidth;

        IDisktraModel<Node> heuristicImpl;
        Node startNode;
        Node destNode;

        List<List<Node>> graphMatrix = new List<List<Node>>();

        public SnackVendor(string input)
        {
            string[] specs = input.Split(' ');
            InitSnackMaching(specs);
            InitRobotSpecs(specs);
        }

        void InitSnackMaching(string[] input)
        {
            matrixHeight = char.ToUpper(input[0][0]) - 64;
            matrixWidth = Int32.Parse(input[0].Remove(0, 1));

            int k = 1;
            int k1 = matrixHeight * matrixWidth;

            for (int i = 0; i < matrixHeight; i++)
            {
                graphMatrix.Add(new List<Node>());
                for (int j = 0; j < matrixWidth; j++)
                {
                    graphMatrix[i].Add(new Node
                    {
                        i = i,
                        j = j
                    });
                    k++;
                }
            }

            numberOfCustomersIndex = k + k1;
        }

        void InitRobotSpecs(string[] specs)
        {
            int robotHeight = char.ToUpper(specs[1][0]) - 65;
            int robotWidth = Int32.Parse(specs[1].Remove(0, 1)) - 1;

            int destHeight = char.ToUpper(specs[2][0]) - 65;
            int destWidth = Int32.Parse(specs[2].Remove(0, 1)) - 1;

            int maxDist = Math.Abs(robotHeight - destHeight) + Math.Abs(robotWidth - destWidth);

            startNode = graphMatrix[robotHeight][robotWidth];
            destNode = graphMatrix[destHeight][destWidth];

            int brokenDir = Int32.Parse(specs[3]);

            heuristicImpl = new RobotArmHeuristic(graphMatrix, matrixHeight, matrixWidth, brokenDir);


        }

        public void AStarFix()
        {
            AStar<Node> aSearchAlgo = new AStar<Node>(heuristicImpl);

            IEnumerable<Node> result = aSearchAlgo.FindPath(startNode, destNode);
            Log(result);
        }

        public void DijstraFix()
        {
            Dijkstra<Node> dijkstra = new Dijkstra<Node>(heuristicImpl);
            IEnumerable<Node> result = dijkstra.FindPath(startNode, destNode);
            Log(result);
        }

        void Log(IEnumerable<Node> result)
        {
            if (result != null)
            {
                Console.WriteLine("Path hans n=" + result.Count() + " nodes");
            }
            else
            {
                Console.WriteLine("No path found");
            }

        }
    }

    class Node : IComparable
    {
        public int i;
        public int j;
        public int dist = 0;

        public int CompareTo(object obj)
        {
            if (obj.GetType() == typeof(Node))
            {
                Node n = (Node)obj;

                if (n.i == i && n.j == j)
                {
                    return 0;
                }

                return -1;
            }

            return 1;
        }
    }

    class RobotArmHeuristic : IDisktraModel<Node>
    {
        List<List<Node>> matrix;
        List<Node> positions;
        int height;
        int width;

        public RobotArmHeuristic(List<List<Node>> matrix, int height, int width, int brokenMove)
        {
            this.matrix = matrix;
            positions = new List<Node>();
            this.height = height;
            this.width = width;
            InitMovements(brokenMove);
        }

        public double VertexLength(Node node, Node neightboar)
        {
            return 1;
        }

        public double HeuristicDistanceBetween(Node start, Node goal)
        {
            return Math.Sqrt(Math.Pow(start.i - goal.i, 2) + Math.Pow(start.j - goal.j, 2));
        }

        public IEnumerable<Node> GetNeighboars(Node node)
        {
            List<Node> neighboars = new List<Node>();

            foreach (var item in positions)
            {
                int nextI = item.i + node.i;
                int nextJ = item.j + node.j;

                if (nextI < 0 ||
                        nextI >= height ||
                        nextJ < 0 ||
                        nextJ >= width)
                {
                    continue;
                }

                neighboars.Add(matrix[nextI][nextJ]);
            }

            return neighboars;
        }

        private void InitMovements(int noMove)
        {
            if (noMove != 0)
            {
                positions.Add(new Node { i = 0, j = 1 });
            }
            if (noMove != 1)
            {
                positions.Add(new Node { i = -1, j = 1 });
            }
            if (noMove != 2)
            {
                positions.Add(new Node { i = -1, j = 0 });
            }
            if (noMove != 3)
            {
                positions.Add(new Node { i = -1, j = -1 });
            }
            if (noMove != 4)
            {
                positions.Add(new Node { i = 0, j = -1 });
            }
            if (noMove != 5)
            {
                positions.Add(new Node { i = 1, j = -1 });
            }
            if (noMove != 6)
            {
                positions.Add(new Node { i = 1, j = 0 });
            }
            if (noMove != 7)
            {
                positions.Add(new Node { i = 1, j = 1 });
            }
        }
    }

    class SnackVendorFactory
    {
        public static void RunProblem(string algorithm)
        {
            string[] inputs =
            {
                "D6 D5 B2 4",
                "W18 E15 E2 4",
                "S15 R6 H4 2",
                "Z26 T2 Z25 0",
                "Z6 Z2 B5 2",
                "Z26 E5 X23 7",
                "E25 D2 B23 0",
                "Z2 A1 Z2 6",
                "Z2 A1 Z1 6"
            };

            foreach (string item in inputs)
            {
                SnackVendor a = new SnackVendor(item);

                if (algorithm == "astar")
                {
                    a.AStarFix();
                }

                if (algorithm == "dijstra")
                {
                    a.DijstraFix();
                }
            }

            Console.WriteLine();
        }
    }
}
