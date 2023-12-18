using Utilities;
using System.Drawing;

namespace Days;

public static class DayEighteen
{
    private static readonly string EMPTY = "EMPTY";
    public static long Handle()
    {
        return HandleStepTwo();
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\eighteen.txt");
        var instructions = new List<(Direction, int, string)>();
        var Coords = new Dictionary<(int, int), string>();
        var currentSpot = (0, 0, EMPTY);
        var lowestRow = 0;
        var lowestCol = 0;
        var highestCol = 0;
        var highestRow = 0;
        foreach (var line in input)
        {
            var value = line.Split(" ").Last().Replace("(", "").Replace(")", "");
            var distance = Convert.ToInt32(value[0..5], 16);
            var direction = value.Last().MapToDirection();

            Console.WriteLine($"{value} {direction} {distance}");

            instructions.Add((direction, distance, value));
            for (int i = 1; i < distance + 1; i++)
            {
                if (direction == Direction.UP)
                {
                    currentSpot = (currentSpot.Item1 - 1, currentSpot.Item2, value);
                    Coords.Add((currentSpot.Item1, currentSpot.Item2), value);
                }
                if (direction == Direction.DOWN)
                {
                    currentSpot = (currentSpot.Item1 + 1, currentSpot.Item2, value);
                    Coords.Add((currentSpot.Item1, currentSpot.Item2), value);
                }
                if (direction == Direction.LEFT)
                {
                    currentSpot = (currentSpot.Item1, currentSpot.Item2 - 1, value);
                    Coords.Add((currentSpot.Item1, currentSpot.Item2), value);
                }
                if (direction == Direction.RIGHT)
                {
                    currentSpot = (currentSpot.Item1, currentSpot.Item2 + 1, value);
                    Coords.Add((currentSpot.Item1, currentSpot.Item2), value);
                }

                if (currentSpot.Item1 < 0 || currentSpot.Item2 < 0)
                {
                    if (currentSpot.Item1 < lowestRow)
                    {
                        lowestRow = currentSpot.Item1;
                    }

                    if (currentSpot.Item2 < lowestCol)
                    {
                        lowestCol = currentSpot.Item2;
                    }
                }

                highestRow = currentSpot.Item1 > highestRow ? currentSpot.Item1 : highestRow;
                highestCol = currentSpot.Item2 > highestCol ? currentSpot.Item2 : highestCol;
            }
        }

        var actualMatrix = new List<List<string>>();
        for (int i = 0; i <= highestRow - lowestRow; i++)
        {
            var newlist = new List<string>();
            for (int j = 0; j <= highestCol - lowestCol; j++)
            {
                newlist.Add(EMPTY);
            }
            actualMatrix.Add(newlist);
        }

        var listOfCoords = new List<(int, int)>();
        foreach (var coord in Coords)
        {
            actualMatrix[coord.Key.Item1 - lowestRow][coord.Key.Item2 - lowestCol] = coord.Value;
            listOfCoords.Add((coord.Key.Item1 - lowestRow, coord.Key.Item2 - lowestCol));
        }

        var touched = new HashSet<(int, int)>();
        var inside = new List<(int, int)>();
        var outside = new List<(int, int)>();
        long countOfSpace = listOfCoords.Count;
        for (int i = 0; i <= highestRow - lowestRow; i++)
        {
            for (int j = 0; j <= highestCol - lowestCol; j++)
            {
                if (!touched.Contains((i, j)))
                {
                    (var canTouchWall, var groupOfNodes) = hasPathToWall(actualMatrix, (i, j), touched);
                    foreach (var t in groupOfNodes)
                    {
                        touched.Add(t);
                    }

                    if (!canTouchWall)
                    {
                        countOfSpace += groupOfNodes.Count;
                        inside.AddRange(groupOfNodes);
                    }
                    else
                    {
                        outside.AddRange(groupOfNodes);
                    }
                }
            }
        }

        visualiseCoords(actualMatrix, outside, inside);
        // Console.WriteLine(string.Join("\n", Coords.Keys));
        return countOfSpace;
    }
    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\eighteen.txt");
        var instructions = new List<(Direction, int, string)>();
        var Coords = new Dictionary<(int, int), string>();
        var currentSpot = (0, 0, EMPTY);
        var lowestRow = 0;
        var lowestCol = 0;
        var highestCol = 0;
        var highestRow = 0;
        foreach (var line in input)
        {
            var direction = line.Split(" ")[0][0].MapToDirection();
            var distance = int.Parse(line.Split(" ")[1]);
            var value = line.Split(" ").Last().Replace("(", "").Replace(")", "");
            instructions.Add((direction, distance, value));
            for (int i = 1; i < distance + 1; i++)
            {
                if (direction == Direction.UP)
                {
                    currentSpot = (currentSpot.Item1 - 1, currentSpot.Item2, value);
                    Coords.Add((currentSpot.Item1, currentSpot.Item2), value);
                }
                if (direction == Direction.DOWN)
                {
                    currentSpot = (currentSpot.Item1 + 1, currentSpot.Item2, value);
                    Coords.Add((currentSpot.Item1, currentSpot.Item2), value);
                }
                if (direction == Direction.LEFT)
                {
                    currentSpot = (currentSpot.Item1, currentSpot.Item2 - 1, value);
                    Coords.Add((currentSpot.Item1, currentSpot.Item2), value);
                }
                if (direction == Direction.RIGHT)
                {
                    currentSpot = (currentSpot.Item1, currentSpot.Item2 + 1, value);
                    Coords.Add((currentSpot.Item1, currentSpot.Item2), value);
                }

                if (currentSpot.Item1 < 0 || currentSpot.Item2 < 0)
                {
                    if (currentSpot.Item1 < lowestRow)
                    {
                        lowestRow = currentSpot.Item1;
                    }

                    if (currentSpot.Item2 < lowestCol)
                    {
                        lowestCol = currentSpot.Item2;
                    }
                }

                highestRow = currentSpot.Item1 > highestRow ? currentSpot.Item1 : highestRow;
                highestCol = currentSpot.Item2 > highestCol ? currentSpot.Item2 : highestCol;
            }
        }

        var actualMatrix = new List<List<string>>();
        for (int i = 0; i <= highestRow - lowestRow; i++)
        {
            var newlist = new List<string>();
            for (int j = 0; j <= highestCol - lowestCol; j++)
            {
                newlist.Add(EMPTY);
            }
            actualMatrix.Add(newlist);
        }

        var listOfCoords = new List<(int, int)>();
        foreach (var coord in Coords)
        {
            actualMatrix[coord.Key.Item1 - lowestRow][coord.Key.Item2 - lowestCol] = coord.Value;
            listOfCoords.Add((coord.Key.Item1 - lowestRow, coord.Key.Item2 - lowestCol));
        }

        var touched = new HashSet<(int, int)>();
        var inside = new List<(int, int)>();
        var outside = new List<(int, int)>();
        long countOfSpace = listOfCoords.Count;
        for (int i = 0; i <= highestRow - lowestRow; i++)
        {
            for (int j = 0; j <= highestCol - lowestCol; j++)
            {
                if (!touched.Contains((i, j)))
                {
                    (var canTouchWall, var groupOfNodes) = hasPathToWall(actualMatrix, (i, j), touched);
                    foreach (var t in groupOfNodes)
                    {
                        touched.Add(t);
                    }

                    if (!canTouchWall)
                    {
                        countOfSpace += groupOfNodes.Count;
                        inside.AddRange(groupOfNodes);
                    }
                    else
                    {
                        outside.AddRange(groupOfNodes);
                    }
                }
            }
        }

        visualiseCoords(actualMatrix, outside, inside);
        // Console.WriteLine(string.Join("\n", Coords.Keys));
        return countOfSpace;

    }

    private static (bool, HashSet<(int, int)>) hasPathToWall(List<List<string>> inputMatrix, (int, int) startingCoords, HashSet<(int, int)> touched)
    {
        var nodesToCheckNeighbours = new HashSet<(int, int)>() { startingCoords };
        var currentNodes = new HashSet<(int, int)>();
        var hasPathToWall = false;

        while (nodesToCheckNeighbours.Any())
        {
            var newNodesToCheckNeighbours = new HashSet<(int, int)>();
            foreach (var node in nodesToCheckNeighbours)
            {
                if (inputMatrix[node.Item1][node.Item2] != EMPTY)
                {
                    touched.Add(node);
                    continue;
                }

                if (touched.Contains(node))
                {
                    continue;
                }

                var up = (node.Item1 - 1, node.Item2);
                var down = (node.Item1 + 1, node.Item2);
                var left = (node.Item1, node.Item2 - 1);
                var right = (node.Item1, node.Item2 + 1);
                var validChecks = new List<(int, int)>() { up, down, left, right };

                foreach (var check in validChecks)
                {
                    if (touched.Contains(check))
                    {
                        continue;
                    }

                    try
                    {
                        var neighbourValue = inputMatrix[check.Item1][check.Item2];
                        if (neighbourValue == EMPTY)
                        {
                            newNodesToCheckNeighbours.Add(check);
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        hasPathToWall = true;
                    }
                }

                currentNodes.Add(node);
                touched.Add(node);
            }
            nodesToCheckNeighbours = newNodesToCheckNeighbours;
        }
        return (hasPathToWall, currentNodes);
    }

    private static void visualiseCoords(List<List<string>> matrix, List<(int, int)> outside, List<(int, int)> inside)
    {
        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                if (outside.Contains((i, j)))
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                else if (inside.Contains((i, j)))
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }

                if (matrix[i][j] == EMPTY)
                {
                    Console.Write(". ");
                }
                else
                {

                    Console.Write("# ");
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine();
        }
    }


}