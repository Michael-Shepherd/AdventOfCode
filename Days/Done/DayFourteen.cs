using System.Collections.Immutable;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.VisualBasic;
using Utilities;

namespace Days;

public static class DayFourteen
{
    private static readonly char BLOCK = '#';
    private static readonly char ROUND = 'O';
    private static readonly char SPACE = '.';

    public static long Handle()
    {
        return HandleStepTwo();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\fourteen.txt");
        var inputMatrix = new List<List<char>>();
        var columnBlockRowIndices = new Dictionary<int, List<int>>();
        var roundCoordinates = new List<(int, int)>();
        for (int j = 0; j < input.Length; j++)
        {
            var line = input[j];
            inputMatrix.Add(line.ToList());
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == BLOCK)
                {
                    // column is I, row is J
                    if (columnBlockRowIndices.TryGetValue(i, out var list))
                    {
                        columnBlockRowIndices[i].Add(j);
                    }
                    else
                    {
                        columnBlockRowIndices.Add(i, new List<int>() { j });
                    }
                }
                else if (line[i] == ROUND)
                {
                    roundCoordinates.Add((j, i));
                }
            }
        }

        foreach (var line in inputMatrix)
        {
            Console.WriteLine(string.Join(" ", line));
        }

        inputMatrix = inputMatrix.ShiftNorth();
        Console.WriteLine("\nShifted:");
        foreach (var line in inputMatrix)
        {
            Console.WriteLine(string.Join(" ", line));
        }

        var loadDict = new Dictionary<int, long>();
        for (int i = 0; i < inputMatrix.Count(); i++)
        {
            var weight = GetWeightOnRow(inputMatrix, i);
            Console.WriteLine($"row: {i} weight: {weight}");
            loadDict.Add(i, weight);
        }


        return loadDict.Values.Sum();
    }

    public static long GetWeightOnRow(List<List<char>> inputMatrix, int rowIndex)
    {
        long load = 0;

        for (int i = 0; i < inputMatrix[rowIndex].Count; i++)
        {
            if (inputMatrix[rowIndex][i] == ROUND)
            {
                load += inputMatrix.Count() - rowIndex;
            }
        }

        return load;
    }

    public static List<List<char>> ShiftNorth(this List<List<char>> inputMatrix)
    {
        var newMatrix = new List<List<char>>();
        var (columnBlockIndices, roundCoords) = GetBlocksAndArounds(inputMatrix);

        inputMatrix.ForEach((item) =>
        {
            var wololo = item.ToList();
            newMatrix.Add(wololo);
        });

        foreach (var round in roundCoords)
        {
            try
            {
                if (newMatrix[round.Item1 - 1][round.Item2] != SPACE)
                {
                    continue;
                }

                columnBlockIndices.TryGetValue(round.Item2, out var colBlocks);

                var blockRow = -1;
                if (colBlocks is not null && colBlocks.Where(i => i < round.Item1).Any())
                {
                    // ASSUMPTION: Colblock is sorted ascending
                    blockRow = colBlocks.Where(i => i < round.Item1).Last();
                }
                var nextRowIndex = round.Item1 - 1;

                while (nextRowIndex > blockRow)
                {
                    // Console.WriteLine("Next Row: " + nextRowIndex + "Block Rown: " + blockRow);
                    // Console.WriteLine("Source: " + newMatrix[nextRowIndex + 1][round.Item2] + " " + (nextRowIndex + 1, round.Item2));
                    // Console.WriteLine("Destination: " + newMatrix[nextRowIndex][round.Item2] + " " + (nextRowIndex, round.Item2));
                    if (newMatrix[nextRowIndex][round.Item2] != SPACE)
                    {
                        break;
                    }
                    else
                    {
                        newMatrix[nextRowIndex][round.Item2] = ROUND;
                        newMatrix[nextRowIndex + 1][round.Item2] = SPACE;
                    }
                    nextRowIndex--;
                }
            }
            catch (Exception)
            {
                continue;
            }
        }

        return newMatrix;
    }

    public static List<List<char>> ShiftSouth(this List<List<char>> inputMatrix)
    {
        var newMatrix = inputMatrix.TurnRight();
        newMatrix = newMatrix.TurnRight();

        var (newColumnBlockIndices, newRoundCoords) = GetBlocksAndArounds(newMatrix);

        newMatrix = newMatrix.ShiftNorth();

        newMatrix = newMatrix.TurnLeft();
        newMatrix = newMatrix.TurnLeft();

        return newMatrix;
    }


    public static List<List<char>> ShiftEast(this List<List<char>> inputMatrix)
    {
        var newMatrix = inputMatrix.TurnLeft();
        var (newColumnBlockIndices, newRoundCoords) = GetBlocksAndArounds(newMatrix);

        newMatrix = newMatrix.ShiftNorth();

        newMatrix = newMatrix.TurnRight();

        return newMatrix;
    }

    public static List<List<char>> ShiftWest(this List<List<char>> inputMatrix)
    {
        var newMatrix = inputMatrix.TurnRight();
        var (newColumnBlockIndices, newRoundCoords) = GetBlocksAndArounds(newMatrix);

        newMatrix = newMatrix.ShiftNorth();

        newMatrix = newMatrix.TurnLeft();

        return newMatrix;
    }

    public static (Dictionary<int, List<int>>, List<(int, int)>) GetBlocksAndArounds(List<List<char>> input)
    {
        var columnBlockRowIndices = new Dictionary<int, List<int>>();
        var roundCoordinates = new List<(int, int)>();
        for (int j = 0; j < input.Count(); j++)
        {
            var line = input[j];
            for (int i = 0; i < line.Count(); i++)
            {
                if (line[i] == BLOCK)
                {
                    // column is I, row is J
                    if (columnBlockRowIndices.TryGetValue(i, out var list))
                    {
                        columnBlockRowIndices[i].Add(j);
                    }
                    else
                    {
                        columnBlockRowIndices.Add(i, new List<int>() { j });
                    }
                }
                else if (line[i] == ROUND)
                {
                    roundCoordinates.Add((j, i));
                }
            }
        }

        return (columnBlockRowIndices, roundCoordinates);
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\fourteen.txt");
        var inputMatrix = new List<List<char>>();
        var columnBlockRowIndices = new Dictionary<int, List<int>>();
        var roundCoordinates = new List<(int, int)>();
        for (int j = 0; j < input.Length; j++)
        {
            var line = input[j];
            inputMatrix.Add(line.ToList());
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == BLOCK)
                {
                    // column is I, row is J
                    if (columnBlockRowIndices.TryGetValue(i, out var list))
                    {
                        columnBlockRowIndices[i].Add(j);
                    }
                    else
                    {
                        columnBlockRowIndices.Add(i, new List<int>() { j });
                    }
                }
                else if (line[i] == ROUND)
                {
                    roundCoordinates.Add((j, i));
                }
            }
        }



        foreach (var line in inputMatrix)
        {
            Console.WriteLine(string.Join(" ", line));
        }

        // inputMatrix = inputMatrix.ShiftSouth(columnBlockRowIndices, roundCoordinates);

        var shiftNorthhistoryDictionary = new Dictionary<string, List<List<char>>>();
        var shiftWesthistoryDictionary = new Dictionary<string, List<List<char>>>();
        var shiftSouthhistoryDictionary = new Dictionary<string, List<List<char>>>();
        var shiftEasthistoryDictionary = new Dictionary<string, List<List<char>>>();

        var fullRotationHistory = new Dictionary<string, List<List<char>>>();

        Stopwatch roatationStopwatch = Stopwatch.StartNew();
        int rotations = 1000000000;
        var listOfResults = new List<long>();
        for (int i = 0; i < rotations; i++)
        {
            var startingMatrix = inputMatrix;
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (fullRotationHistory.TryGetValue(inputMatrix.Flatten(), out var fullRotation))
            {
                inputMatrix = fullRotation;
                if (i % 1000000 == 0)
                {
                    var newLoadDict = new Dictionary<int, long>();
                    for (int j = 0; j < fullRotation.Count(); j++)
                    {
                        var weight = GetWeightOnRow(fullRotation, j);
                        // Console.WriteLine($"row: {j} weight: {weight}");
                        newLoadDict.Add(j, weight);
                    }
                    Console.WriteLine(i + "," + newLoadDict.Values.Sum());
                }
            }
            else
            {
                var startNorth = inputMatrix;
                if (shiftNorthhistoryDictionary.TryGetValue(inputMatrix.Flatten(), out var north))
                {
                    Console.WriteLine("North Cache Hit");
                    inputMatrix = north;
                }
                else
                {
                    inputMatrix = inputMatrix.ShiftNorth();
                    shiftNorthhistoryDictionary.Add(startNorth.Flatten(), inputMatrix);
                }
                // Console.WriteLine("Shifted North:");
                // foreach (var line in inputMatrix)
                // {
                //     Console.WriteLine(string.Join(" ", line));
                // }

                var startWest = inputMatrix;
                if (shiftWesthistoryDictionary.TryGetValue(inputMatrix.Flatten(), out var west))
                {
                    Console.WriteLine("West Cache hit");
                    inputMatrix = west;
                }
                else
                {
                    inputMatrix = inputMatrix.ShiftWest();
                    shiftNorthhistoryDictionary.Add(startWest.Flatten(), inputMatrix);
                }
                // Console.WriteLine("Shifted West:");
                // foreach (var line in inputMatrix)
                // {
                //     Console.WriteLine(string.Join(" ", line));
                // }
                var startSouth = inputMatrix;
                if (shiftSouthhistoryDictionary.TryGetValue(inputMatrix.Flatten(), out var south))
                {
                    Console.WriteLine("South Cache Hit");
                    inputMatrix = south;
                }
                else
                {
                    inputMatrix = inputMatrix.ShiftSouth();
                    shiftSouthhistoryDictionary.Add(startSouth.Flatten(), inputMatrix);
                }
                // Console.WriteLine("Shifted South:");
                // foreach (var line in inputMatrix)
                // {
                //     Console.WriteLine(string.Join(" ", line));
                // }
                var startEast = inputMatrix;
                if (shiftEasthistoryDictionary.TryGetValue(inputMatrix.Flatten(), out var east))
                {
                    Console.WriteLine("East Cache Hit");
                    inputMatrix = east;
                }
                else
                {
                    inputMatrix = inputMatrix.ShiftEast();
                    shiftEasthistoryDictionary.Add(startEast.Flatten(), inputMatrix);
                }
                // Console.WriteLine("Shifted East:");
                // foreach (var line in inputMatrix)
                // {
                //     Console.WriteLine(string.Join(" ", line));
                // }
                fullRotationHistory.Add(startingMatrix.Flatten(), inputMatrix);
            }


            stopwatch.Stop();
            // Console.WriteLine("Time To Rotate : {0}", stopwatch.Elapsed);
            // Console.WriteLine("Estimated Time : {0}", stopwatch.Elapsed * rotations);
            if (i % 10000000 == 0)
            {
                Console.WriteLine(i);
                Console.WriteLine("Time To Rotate : {0}", stopwatch.Elapsed);
                Console.WriteLine("Full Rotation TimeElapsed : {0}", roatationStopwatch.Elapsed);
                Console.WriteLine("Estimated Time : {0}", stopwatch.Elapsed * rotations);
                Console.WriteLine("Estimated Time Hours : {0}", (stopwatch.Elapsed * rotations).Hours);
                Console.WriteLine("Estimated Time Minutes : {0}", (stopwatch.Elapsed * rotations).Minutes);

            }
        }
        roatationStopwatch.Stop();
        Console.WriteLine("Full Rotation Time : {0}", roatationStopwatch.Elapsed);


        // Console.WriteLine("Shifted");
        // foreach (var line in inputMatrix)
        // {
        //     Console.WriteLine(string.Join(" ", line));
        // }

        Console.WriteLine();
        Console.WriteLine("Results:");
        listOfResults.Sort();
        Console.WriteLine(string.Join("\n", listOfResults.Distinct()));
        Console.WriteLine();

        var loadDict = new Dictionary<int, long>();
        for (int i = 0; i < inputMatrix.Count(); i++)
        {
            var weight = GetWeightOnRow(inputMatrix, i);
            // Console.WriteLine($"row: {i} weight: {weight}");
            loadDict.Add(i, weight);
        }


        return loadDict.Values.Sum();
    }
}