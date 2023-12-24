using System.Diagnostics;
using Utilities;

namespace Days;

public static class DayTwentyOne
{
    public static long Handle()
    {
        return HandleStepTwo();
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\twentyOne.txt").Select(line => line.ToArray()).ToArray();
        var coOrdinates = new Dictionary<(int, int), char>();
        var blocks = new HashSet<(int, int)>();
        var startCoord = (-1, -1);
        var startingRows = 0;
        var startingCols = 0;
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == 'S')
                {
                    startCoord = (i, j);
                }
                else if (input[i][j] == '#')
                {
                    blocks.Add((i, j));
                }
                coOrdinates.Add((i, j), input[i][j]);
                startingCols = j;
            }
            startingRows = i;
        }

        return WalkAroundTwo(blocks, startCoord, 500, startingRows, startingCols);
    }


    private static long WalkAroundTwo(HashSet<(int, int)> blocks, (int, int) start, int steps, int rows, int cols)
    {
        var rowsTop = 0;
        var rowsBottom = rows;
        var colsLeft = 0;
        var colsRight = cols;
        var startingSpots = new HashSet<(int, int)>() { start };
        for (int step = 1; step <= steps; step++)
        {
            var nexStartingSpots = new HashSet<(int, int)>();
            foreach (var coord in startingSpots)
            {
                var up = (coord.Item1 - 1, coord.Item2);
                var down = (coord.Item1 + 1, coord.Item2);
                var left = (coord.Item1, coord.Item2 - 1);
                var right = (coord.Item1, coord.Item2 + 1);
                var coordsToCheck = new List<(int, int)>() { up, down, left, right };

                // Parallel.For(0, coordsToCheck.Count, i =>
                // {
                //     var spot = coordsToCheck[i];
                //     (var isBlocked, _, _) = IsBlockBlocked(blocks, spot, rowsTop, rowsBottom, colsLeft, colsRight);
                //     // Console.WriteLine(blocks.Count);
                //     if (!isBlocked)
                //     {
                //         lock (nexStartingSpots)
                //         {
                //             nexStartingSpots.Add(spot);
                //         }
                //     }
                // });
                foreach (var spot in coordsToCheck)
                {
                    (var isBlocked, _, _) = IsBlockBlocked(blocks, spot, rowsTop, rowsBottom, colsLeft, colsRight);
                    // Console.WriteLine(blocks.Count);
                    if (!isBlocked)
                    {
                        nexStartingSpots.Add(spot);
                    }
                }
            }
            if (step % 1000000 == 0)
            {
                Console.WriteLine(step);
            }
            startingSpots = nexStartingSpots;
        }
        return startingSpots.Count();
    }

    // TODO: Use maths
    public static (bool, HashSet<(int, int)>, (int rowsTop, int rowsBottom, int colsLeft, int colsRight)) IsBlockBlocked(HashSet<(int, int)> blocks, (int, int) coord, int rowsTop, int rowsBottom, int colsLeft, int colsRight)
    {
        int i = 1;
        var adjustedCoord = coord;
        var rowShift = (1 + rowsBottom) - rowsTop;
        var colShift = (colsRight + 1) - colsLeft;
        while (true)
        {
            if (adjustedCoord.Item1 >= rowsTop &&
            adjustedCoord.Item1 <= rowsBottom &&
            adjustedCoord.Item2 >= colsLeft &&
            adjustedCoord.Item2 <= colsRight)
            {
                // Console.WriteLine("returned");
                // check blocks
                if (blocks.Contains(adjustedCoord))
                {

                    return (true, blocks, (rowsTop, rowsBottom, colsLeft, colsRight));
                }

                return (false, blocks, (rowsTop, rowsBottom, colsLeft, colsRight));
            }

            // UP RIGHT
            if (adjustedCoord.Item1 < rowsTop * i && adjustedCoord.Item2 > colsRight * i)
            {
                adjustedCoord = (adjustedCoord.Item1 + (i * rowShift), adjustedCoord.Item2 - (i * colShift));
            }
            // DOWN RIGHT
            else if (adjustedCoord.Item1 > rowsBottom * i && adjustedCoord.Item2 > colsRight * i)
            {
                adjustedCoord = (adjustedCoord.Item1 - (i * rowShift), adjustedCoord.Item2 - (i * colShift));
            }
            // DOWN LEFT
            else if (adjustedCoord.Item1 > rowsBottom * i && adjustedCoord.Item2 < colsLeft * i)
            {
                adjustedCoord = (adjustedCoord.Item1 - (i * rowShift), adjustedCoord.Item2 + (i * colShift));

            }
            // UP LEFT
            else if (adjustedCoord.Item1 < rowsTop * i && adjustedCoord.Item2 < colsLeft * i)
            {
                adjustedCoord = (adjustedCoord.Item1 + (i * rowShift), adjustedCoord.Item2 + (i * colShift));

            }

            // UP
            else if (adjustedCoord.Item1 < rowsTop * i)
            {
                adjustedCoord = (adjustedCoord.Item1 + (i * rowShift), adjustedCoord.Item2);

            }
            // DOWN
            else if (adjustedCoord.Item1 > rowsBottom * i)
            {
                adjustedCoord = (adjustedCoord.Item1 - (i * rowShift), adjustedCoord.Item2);

            }
            // LEFT
            else if (adjustedCoord.Item2 < colsLeft * i)
            {
                adjustedCoord = (adjustedCoord.Item1, adjustedCoord.Item2 + (i * colShift));

            }
            // RIGHT
            else if (adjustedCoord.Item2 > colsRight * i)
            {
                adjustedCoord = (adjustedCoord.Item1, adjustedCoord.Item2 - (i * colShift));
            }
            else
            {
                if (blocks.Contains(adjustedCoord))
                {
                    return (true, blocks, (rowsTop, rowsBottom, colsLeft, colsRight));
                }

                return (false, blocks, (rowsTop, rowsBottom, colsLeft, colsRight));
            }
            // i++;
        }
    }

    public static bool IsBlockBlocked(Dictionary<(int, int), char> garden, (int, int) coord)
    {
        if (garden.TryGetValue((coord.Item1, coord.Item2), out var spotValue))
        {
            return spotValue == '#';
        }
        else
        {
            return false;
        }
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\twentyOne.txt").Select(line => line.ToArray()).ToArray();
        var coOrdinates = new Dictionary<(int, int), char>();
        var startCoord = (-1, -1);
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == 'S')
                {
                    startCoord = (i, j);
                }
                coOrdinates.Add((i, j), input[i][j]);
            }
        }

        return WalkAround(coOrdinates, startCoord, 64);
    }

    private static long WalkAround(Dictionary<(int, int), char> garden, (int, int) start, int steps)
    {
        var startingSpots = new HashSet<(int, int)>() { start };
        for (int step = 1; step <= steps; step++)
        {
            var nexStartingSpots = new HashSet<(int, int)>();
            foreach (var coord in startingSpots)
            {
                var up = (coord.Item1 - 1, coord.Item2);
                var down = (coord.Item1 + 1, coord.Item2);
                var left = (coord.Item1, coord.Item2 - 1);
                var right = (coord.Item1, coord.Item2 + 1);
                var coordsToCheck = new List<(int, int)>() { up, down, left, right };

                foreach (var spot in coordsToCheck)
                {
                    if (!IsBlockBlocked(garden, spot))
                    {
                        nexStartingSpots.Add(spot);
                    }
                }
            }
            startingSpots = nexStartingSpots;
        }
        return startingSpots.Count();
    }
}