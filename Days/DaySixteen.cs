using Microsoft.VisualBasic;
using Utilities;

namespace Days;

public static class DaySixteen
{
    public static long Handle()
    {
        return HandleStepOne();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\sixteen.txt");
        var inputMatrix = input.Select(a => a.ToList()).ToList();

        var maxEnergised = 0;
        for (int bottom = 0; bottom < 2; bottom++)
        {
            var rowIndex = bottom == 1 ? input.Length - 1 : 0;

            for (int i = 0; i < input[0].Length; i++)
            {
                var tilesToCheck = new List<(int, int, Direction)>() { };

                // TOP
                if (rowIndex == 0)
                {
                    if (inputMatrix[rowIndex][i] == '|')
                    {
                        tilesToCheck.Add((rowIndex, i, Direction.DOWN));
                    }
                    if (inputMatrix[rowIndex][i] == '-')
                    {
                        tilesToCheck.Add((rowIndex, i, Direction.RIGHT));
                        tilesToCheck.Add((rowIndex, i, Direction.LEFT));
                    }
                    if (inputMatrix[rowIndex][i] == '.')
                    {
                        tilesToCheck.Add((rowIndex, i, Direction.DOWN));
                    }
                    if (inputMatrix[rowIndex][i] == '\\')
                    {
                        tilesToCheck.Add((rowIndex, i, Direction.RIGHT));
                    }
                    if (inputMatrix[rowIndex][i] == '/')
                    {
                        tilesToCheck.Add((rowIndex, i, Direction.LEFT));
                    }
                }
                // BOTTOM
                else
                {
                    if (inputMatrix[rowIndex][i] == '|')
                    {
                        tilesToCheck.Add((rowIndex, i, Direction.UP));
                    }
                    if (inputMatrix[rowIndex][i] == '-')
                    {
                        tilesToCheck.Add((rowIndex, i, Direction.RIGHT));
                        tilesToCheck.Add((rowIndex, i, Direction.LEFT));
                    }
                    if (inputMatrix[rowIndex][i] == '.')
                    {
                        tilesToCheck.Add((rowIndex, i, Direction.UP));
                    }
                    if (inputMatrix[rowIndex][i] == '\\')
                    {
                        tilesToCheck.Add((rowIndex, i, Direction.LEFT));
                    }
                    if (inputMatrix[rowIndex][i] == '/')
                    {
                        tilesToCheck.Add((rowIndex, i, Direction.RIGHT));
                    }
                }

                Console.WriteLine($"{tilesToCheck.Count} {0} {0}");

                var touched = new HashSet<(int, int, Direction)>();
                while (tilesToCheck.Any())
                {
                    var newTilesToCheck = new List<(int, int, Direction)>();
                    foreach (var tile in tilesToCheck)
                    {
                        if (touched.Contains(tile))
                        {
                            continue;
                        }
                        touched.Add(tile);
                        newTilesToCheck.AddRange(GetNextDirections(inputMatrix, tile));
                    }
                    tilesToCheck = newTilesToCheck;
                }

                // visualiseMatrix(inputMatrix, touched);

                var distinctLocations = touched.Select(x => (x.Item1, x.Item2)).ToHashSet();
                maxEnergised = maxEnergised < distinctLocations.Count ? distinctLocations.Count : maxEnergised;
                if (distinctLocations.Count == 6665)
                {
                    visualiseMatrix(inputMatrix, touched);
                }
            }
        }

        var cornerStarts = new List<(int, int, Direction)>()
        {
            (0, 0, Direction.DOWN),
            (0, input[0].Length -1, Direction.LEFT),
            (input.Length - 1, 0, Direction.RIGHT),
            (input.Length - 1, input[0].Length -1, Direction.LEFT),
        };

        foreach (var start in cornerStarts)
        {
            var tilesToCheck = new List<(int, int, Direction)>() { start };
            var touched = new HashSet<(int, int, Direction)>();
            while (tilesToCheck.Any())
            {
                var newTilesToCheck = new List<(int, int, Direction)>();
                foreach (var tile in tilesToCheck)
                {
                    if (touched.Contains(tile))
                    {
                        continue;
                    }
                    touched.Add(tile);
                    newTilesToCheck.AddRange(GetNextDirections(inputMatrix, tile));
                }
                tilesToCheck = newTilesToCheck;
            }

            // visualiseMatrix(inputMatrix, touched);

            var distinctLocations = touched.Select(x => (x.Item1, x.Item2)).ToHashSet();
            maxEnergised = maxEnergised < distinctLocations.Count ? distinctLocations.Count : maxEnergised;
            // visualiseMatrix(inputMatrix, touched);
        }

        return maxEnergised;
    }


    private static List<(int, int, Direction)> GetNextDirections(List<List<char>> inputMatrix, (int, int, Direction) currentLocation)
    {
        var returnList = new List<(int, int, Direction)>();

        var direction = currentLocation.Item3;
        try
        {
            var currentTileValue = inputMatrix[currentLocation.Item1][currentLocation.Item1];
            // Console.WriteLine($"{currentLocation} {currentTileValue} {direction}");
        }
        catch (ArgumentOutOfRangeException)
        {
            return returnList;
        }

        var goDown = (direction == Direction.RIGHT) ? '\\' : (direction == Direction.LEFT) ? '/' : (direction == Direction.DOWN) ? '|' : 'X';
        var goUp = (direction == Direction.RIGHT) ? '/' : (direction == Direction.LEFT) ? '\\' : (direction == Direction.UP) ? '|' : 'X';
        var goLeft = (direction == Direction.UP) ? '\\' : (direction == Direction.LEFT) ? '-' : (direction == Direction.DOWN) ? '/' : 'X';
        var goRight = (direction == Direction.UP) ? '/' : (direction == Direction.RIGHT) ? '-' : (direction == Direction.DOWN) ? '\\' : 'X';
        var splitUpDown = (direction == Direction.RIGHT) ? '|' : (direction == Direction.LEFT) ? '|' : 'X';
        var splitLeftRight = (direction == Direction.UP) ? '-' : (direction == Direction.DOWN) ? '-' : 'X';
        var goThrough = new List<char>() { '.' };
        if (direction == Direction.LEFT || direction == Direction.RIGHT)
        {
            goThrough.Add('-');
        }
        else
        {
            goThrough.Add('|');
        }

        var nextTile = currentLocation;

        switch (direction)
        {
            case Direction.RIGHT:
                nextTile = (currentLocation.Item1, currentLocation.Item2 + 1, direction);
                break;
            case Direction.LEFT:
                nextTile = (currentLocation.Item1, currentLocation.Item2 - 1, direction);
                break;
            case Direction.UP:
                nextTile = (currentLocation.Item1 - 1, currentLocation.Item2, direction);
                break;
            case Direction.DOWN:
                nextTile = (currentLocation.Item1 + 1, currentLocation.Item2, direction);
                break;
            default:
                return returnList;
        }

        try
        {
            var nextTileValue = inputMatrix[nextTile.Item1][nextTile.Item2];

            if (goThrough.Contains(nextTileValue))
            {
                returnList.Add(nextTile);
            }
            else
            if (nextTileValue == goDown)
            {
                nextTile = (nextTile.Item1, nextTile.Item2, Direction.DOWN);
                returnList.Add(nextTile);
            }
            else
            if (nextTileValue == goUp)
            {
                nextTile = (nextTile.Item1, nextTile.Item2, Direction.UP);

                returnList.Add(nextTile);
            }
            else
            if (nextTileValue == goLeft)
            {
                nextTile = (nextTile.Item1, nextTile.Item2, Direction.LEFT);

                returnList.Add(nextTile);
            }
            else
            if (nextTileValue == goRight)
            {
                nextTile = (nextTile.Item1, nextTile.Item2, Direction.RIGHT);
                returnList.Add(nextTile);
            }
            else
            if (nextTileValue == splitLeftRight)
            {
                nextTile = (nextTile.Item1, nextTile.Item2, Direction.RIGHT);
                returnList.Add(nextTile);

                nextTile = (nextTile.Item1, nextTile.Item2, Direction.LEFT);
                returnList.Add(nextTile);

            }
            else
            if (nextTileValue == splitUpDown)
            {
                nextTile = (nextTile.Item1, nextTile.Item2, Direction.UP);
                returnList.Add(nextTile);

                nextTile = (nextTile.Item1, nextTile.Item2, Direction.DOWN);
                returnList.Add(nextTile);
            }

        }
        catch (ArgumentOutOfRangeException)
        {
            return returnList;
        }

        return returnList;
    }

    private static void visualiseMatrix(List<List<char>> inputMatrix, HashSet<(int, int, Direction)> touched)
    {
        // Console.Clear();

        for (int i = 0; i < inputMatrix.Count; i++)
        {
            for (int j = 0; j < inputMatrix[0].Count; j++)
            {
                Direction direction1 = Direction.NONE;
                foreach (var direction in new List<Direction>() { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT })
                {
                    if (touched.Contains((i, j, direction)))
                    {
                        if (direction1 != Direction.NONE) Console.BackgroundColor = ConsoleColor.Red;
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                        }
                        direction1 = direction;
                    }
                }
                if (direction1 != Direction.NONE)
                {
                    Console.Write("# ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.Write(". ");
                }
            }
            Console.WriteLine();
        }
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\day_seven_input.txt");
        return -1;
    }
}

public enum Direction
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN
}