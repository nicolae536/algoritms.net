using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using algorithms.net.matrix;
using System.Drawing.Imaging;

namespace algorithms.net.dynamic_programming
{
    public class AdictiveGameFlow
    {
        class PositionRowColumn
        {
            public int row;
            public int matrixRow;
            public int column;
            public int matrixColumn;

            public static PositionRowColumn FromString(string positionNr, int nrOfColumns)
            {
                return FromNumber(Int32.Parse(positionNr), nrOfColumns);
            }

            public static PositionRowColumn FromNumber(int position, int nrOfColumns)
            {
                int row = position / nrOfColumns;
                double coulmnsDiff = (double)position / nrOfColumns;

                if ((double)position / nrOfColumns > row)
                {
                    row++;
                }
                int column = position % nrOfColumns;
                if (column == 0)
                {
                    column = nrOfColumns;
                }

                return new PositionRowColumn
                {
                    row = row,
                    matrixRow = row - 1,
                    column = column,
                    matrixColumn = column - 1
                };
            }
        }

        class FlowPoint : IComparable
        {
            public int row;
            public int column;

            public int matrixRow;
            public int matrixColumn;

            public int color;
            public int manhattanDistance;

            public FlowPoint destination;

            public void ComputeManhatan(FlowPoint point)
            {
                manhattanDistance = Math.Abs(row - point.row) + Math.Abs(column - point.column);
            }

            public bool ColorEqual(FlowPoint point)
            {
                return point.color == color;
            }

            public int CompareTo(object obj)
            {
                if (obj.GetType() != this.GetType())
                {
                    return -1;
                }

                FlowPoint next = (FlowPoint)obj;

                if (row > next.row)
                {
                    return -1;
                }

                if (row < next.row)
                {
                    return 1;
                }

                if (column < next.column)
                {
                    return 1;
                }

                if (column > next.column)
                {
                    return -1;
                }

                return 0;
            }

            public override string ToString()
            {
                return color.ToString();
            }
        }

        List<List<FlowPoint>> flowMatrix;
        Bitmap flowMatrixClone;
        Dictionary<FlowPoint, FlowPoint> pointPairs;

        int nrOfRows;
        int nrOfColumns;
        int numberOfPositions;

        public AdictiveGameFlow(string input, string it)
        {
            Console.Write(it + " = ");
            string[] data = input.Split(' ');
            nrOfRows = Int32.Parse(data[0]);
            nrOfColumns = Int32.Parse(data[1]);
            numberOfPositions = Int32.Parse(data[2]);

            ResetMatrix();
            LoadPoints(data);
            CheckPaths(data, it);

            //Console.WriteLine(string.Join(' ', pointPairs.Keys.OrderBy(it => it.color).Select(it => it.manhattanDistance)));
        }

        private void ResetMatrix()
        {
            flowMatrixClone = new Bitmap(nrOfColumns, nrOfRows);
            flowMatrix = new List<List<FlowPoint>>();
            for (int i = 0; i < nrOfRows; i++)
            {
                flowMatrix.Add(new List<FlowPoint>());
                for (int j = 0; j < nrOfColumns; j++)
                {
                    flowMatrix[i].Add(new FlowPoint()
                    {
                        color = 0,
                        column = j + 1,
                        row = i + 1,
                        matrixColumn = j,
                        matrixRow = i
                    });
                    flowMatrixClone.SetPixel(j, i, Color.White);

                }
            }
            pointPairs = new Dictionary<FlowPoint, FlowPoint>();
        }

        public void LoadPoints(string[] data)
        {

            for (int i = 3; i < (numberOfPositions * 2) + 3; i += 2)
            {
                PositionRowColumn rowAndColumn = PositionRowColumn.FromString(data[i], nrOfColumns);

                FlowPoint point = new FlowPoint
                {
                    row = rowAndColumn.row,
                    column = rowAndColumn.column,
                    matrixRow = rowAndColumn.row - 1,
                    matrixColumn = rowAndColumn.column - 1,
                    color = Int32.Parse(data[i + 1])
                };
                FlowPoint parent = pointPairs.Keys.FirstOrDefault(it => it.color == point.color);

                if (parent != null)
                {
                    pointPairs[parent] = point;
                    parent.ComputeManhatan(point);
                    point.manhattanDistance = parent.manhattanDistance;

                    parent.destination = point;
                    point.destination = parent;
                }
                else
                {
                    pointPairs.Add(point, null);
                }

                flowMatrix[point.matrixRow][point.matrixColumn] = point;
                flowMatrixClone.SetPixel(point.matrixColumn, point.matrixRow, Color.Black);
            }
        }

        public void CheckPaths(string[] data, string path)
        {
            List<int> pathOutputs = new List<int>();

            int startI = (numberOfPositions * 2) + 3;
            int numberOfPaths = Int32.Parse(data[startI]);
            startI++;

            int i = startI;
            int pathsCounter = 0;
            while (pathsCounter < numberOfPaths)
            {
                int color = Int32.Parse(data[i]);
                PositionRowColumn rowAndColumn = PositionRowColumn.FromString(data[i + 1], nrOfColumns);
                int pathLength = Int32.Parse(data[i + 2]);

                FlowPoint currentPoint = flowMatrix[rowAndColumn.matrixRow][rowAndColumn.matrixColumn];
                FlowPoint destination = currentPoint.destination;
                List<FlowPoint> tailOfpoints = new List<FlowPoint>();

                int index = 1;
                int j = i + 3;
                int pathEnd = i + pathLength + 3;
                bool pathIsValid = true;

                while (j < pathEnd && pathIsValid)
                {
                    int nextRow = currentPoint.matrixRow;
                    int nextColumn = currentPoint.matrixColumn;
                    switch (data[j][0].ToString().ToLower())
                    {
                        case "n":
                            nextRow = nextRow - 1;
                            break;
                        case "e":
                            nextColumn = nextColumn + 1;
                            break;
                        case "s":
                            nextRow = nextRow + 1;
                            break;
                        case "w":
                            nextColumn = nextColumn - 1;
                            break;
                    }

                    if (IsMoveValid(nextRow, nextColumn, currentPoint.color, j + 1 == pathEnd, destination))
                    {
                        flowMatrix[nextRow][nextColumn].color = currentPoint.color;
                        tailOfpoints.Add(currentPoint);
                        currentPoint = flowMatrix[nextRow][nextColumn];
                        index++;
                        j++;
                    }
                    else
                    {
                        pathIsValid = false;
                    }
                }

                pathOutputs.Add(pathIsValid ? 1 : -1);
                pathOutputs.Add(pathIsValid ? index - 1 : index);
                if (pathIsValid)
                {
                    foreach (FlowPoint item in tailOfpoints)
                    {
                        flowMatrixClone.SetPixel(item.matrixColumn, item.matrixRow, Color.Black);
                    }
                }

                i = pathEnd;
                pathsCounter++;
            }

            int width = 2048;
            int height = 2048;
            float scale = Math.Min(width / nrOfColumns, height / nrOfRows);
            var bmp = new Bitmap(width, height);
            var graph = Graphics.FromImage(bmp);
            var brush = new SolidBrush(Color.White);

            var scaleWidth = (int)(nrOfColumns * scale);
            var scaleHeight = (int)(nrOfRows * scale);

            graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
            graph.DrawImage(flowMatrixClone, ((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight);

            bmp.Save(Program.PROJECT_ROOT + path + ".png", ImageFormat.Png);
            Console.WriteLine(string.Join(" ", pathOutputs.Select(it => it.ToString()).ToList()));
            Console.WriteLine();



            //MatrixModel<FlowPoint>.LogOnScreen(mat);
        }

        private bool IsMoveValid(int nextMatrixRow, int nextMatrixColumn, int color, bool isLastPoint, FlowPoint destination)
        {
            // outside of bounds
            if (destination == null ||
                nextMatrixRow < 0 || nextMatrixRow >= nrOfRows ||
                nextMatrixColumn < 0 || nextMatrixColumn >= nrOfColumns)
            {
                return false;
            }

            FlowPoint point = flowMatrix[nextMatrixRow][nextMatrixColumn];
            // not last move and crossing itself or another color
            if (!isLastPoint && point.color != 0 && (destination != point || point.color != color))
            {
                return false;
            }

            // is last move and does not end on the proper color if the destionation for that point is null or the color is different
            if (isLastPoint && (point.color != color || point.destination == null || point != destination))
            {
                return false;
            }

            // is valid
            return true;
        }

        public static void RunProblem()
        {
            //string[] inputs1 =
            //{
            //    "/adictiveGame/level1-0.in",
            //    "/adictiveGame/level1-1.in",
            //    "/adictiveGame/level1-2.in",
            //    "/adictiveGame/level1-3.in",
            //};

            //foreach (string it in inputs1)
            //{
            //    new AdictiveGameFlow(File.ReadAllText(Program.PROJECT_ROOT + "/dynamic_programming" + it));
            //}


            //string[] inputs2 =
            //{
            //    "/adictiveGame/level2-0.in",
            //    "/adictiveGame/level2-1.in",
            //    "/adictiveGame/level2-2.in",
            //    "/adictiveGame/level2-3.in",
            //};

            //string[] inputs3 =
            //{
            //    "/adictiveGame/level3-0.in",
            //    "/adictiveGame/level3-01.in",
            //    "/adictiveGame/level3-02.in",
            //    "/adictiveGame/level3-03.in",
            //    "/adictiveGame/level3-04.in",
            //    "/adictiveGame/level3-1.in",
            //    "/adictiveGame/level3-2.in",
            //    "/adictiveGame/level3-3.in",
            //    "/adictiveGame/level3-4.in",
            //    "/adictiveGame/level3-5.in",
            //    "/adictiveGame/level3-6.in",
            //    "/adictiveGame/level3-7.in",
            //};

            string[] inputs4 =
            {
                //"/adictiveGame/level4-0.in",
                //"/adictiveGame/level4-1.in",
                //"/adictiveGame/level4-2.in",
                //"/adictiveGame/level4-3.in",
                //"/adictiveGame/level4-4.in",
                "/adictiveGame/level4-5.in",
            };

            foreach (string it in inputs4)
            {
                new AdictiveGameFlow(File.ReadAllText(Program.PROJECT_ROOT + "/dynamic_programming" + it), "/dynamic_programming" + it);
            }
            Console.ReadKey();

        }
    }
}
