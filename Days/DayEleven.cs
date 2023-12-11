using Microsoft.VisualBasic;
using Utilities;

namespace Days;

public static class DayEleven
{
    public static long Handle()
    {
        return HandleStepOne();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\eleven.txt");
        var rowsToExpandIndices = new List<int>();
        var columnsToExpandIndices = new List<int>();
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].Distinct().Count() == 1)
            {
                rowsToExpandIndices.Add(i);
            }
        }
        for (int j = 0; j < input[0].Length; j++)
        {
            var column = "";
            for (int i = 0; i < input.Length; i++)
            {
                column += input[i][j];
            }
            if (column.Distinct().Count() == 1)
            {
                columnsToExpandIndices.Add(j);
            }
        }

        var expansionModifier = 10;
        var fullInput = new List<List<char>>();
        var rowWidth = input[0].Length;
        for (int i = 0; i < input.Length; i++)
        {
            if (rowsToExpandIndices.Contains(i))
            {
                for (int j = 0; j < expansionModifier - 1; j++)
                {
                    fullInput.Add(new string('0', rowWidth).ToList());
                }
            }

            var line = input[i];
            var lineToAdd = line.Replace('.', '0');
            lineToAdd = lineToAdd.Replace('#', '1');

            fullInput.Add(lineToAdd.ToList());
        }

        var colModifier = 0;
        foreach (var col in columnsToExpandIndices)
        {
            for (int i = 0; i < fullInput.Count(); i++)
            {
                for (int j = 0; j < expansionModifier - 1; j++)
                {
                    fullInput[i].Insert(col + colModifier + j, '0');
                }
            }
            colModifier++;
        }

        var galaxyIndex = 1;
        var fullInputAsInt = new List<List<int>>() { };
        // galaxy num, row, col
        var galaxyIndices = new Dictionary<int, (int, int)>();
        for (int i = 0; i < fullInput.Count(); i++)
        {
            var newRow = new List<int>(new int[fullInput[i].Count()]);
            fullInputAsInt.Add(newRow);
            for (int j = 0; j < fullInput[i].Count(); j++)
            {
                if (fullInput[i][j] == '1')
                {
                    galaxyIndices.Add(galaxyIndex, (i, j));
                    fullInputAsInt[i][j] = galaxyIndex++;
                }
            }
        }

        foreach (var line in fullInputAsInt)
        {
            Console.WriteLine(string.Join(" ", line));
        }

        // index 1, index 2, distance
        var distancePairs = new Dictionary<(int, int), long>();
        foreach (var indexOne in galaxyIndices.Keys)
        {
            foreach (var indexTwo in galaxyIndices.Keys)
            {
                if (GetDistanceFromDictionary(distancePairs, indexOne, indexTwo) is null && indexOne != indexTwo)
                {
                    distancePairs.Add((indexOne, indexTwo), -1);
                }
            }
        }
        // Console.WriteLine(distancePairs.Count());

        foreach (var galaxy in galaxyIndices)
        {
            distancePairs = GetUpdatedDistancePairsForFromNode(fullInputAsInt, distancePairs, galaxy.Value);
            // Console.WriteLine(galaxy.Value);
        }

        Console.WriteLine(distancePairs.Count());
        long count = 0;
        foreach (var pair in distancePairs)
        {
            Console.WriteLine(count += pair.Value);
            Console.WriteLine($"Pair {pair.Key} Distance {pair.Value}");

        }


        return distancePairs.Values.Sum();
    }

    private static Dictionary<(int, int), long> GetUpdatedDistancePairsForFromNode(List<List<int>> input, Dictionary<(int, int), long> existingPairs, (int, int) coOrds)
    {
        var listOfCoordsToCheck = new List<(int, int)>() { coOrds };
        var checkedCoords = new HashSet<(int, int)> { };
        var actualCoordIndex = input[coOrds.Item1][coOrds.Item2];
        var distance = 1;
        while (listOfCoordsToCheck.Count() > 0)
        {
            var newListToCheck = new List<(int, int)>();
            foreach (var coord in listOfCoordsToCheck)
            {
                var coordIndex = input[coord.Item1][coord.Item2];

                if (!checkedCoords.Contains(coord))
                {
                    checkedCoords.Add(coord);
                }
                else
                {
                    continue;
                }

                var up = (coord.Item1 - 1, coord.Item2);
                var down = (coord.Item1 + 1, coord.Item2);
                var left = (coord.Item1, coord.Item2 - 1);
                var right = (coord.Item1, coord.Item2 + 1);
                var coordsToCheck = new List<(int, int)>() { up, down, left, right };
                // Console.WriteLine("*********************");
                foreach (var check in coordsToCheck)
                {
                    // Console.WriteLine(coord);
                    // Console.WriteLine(check);
                    // Console.WriteLine("***");
                    try
                    {
                        var currentSpot = input[check.Item1][check.Item2];
                        if (checkedCoords.Contains(check))
                        {
                            continue;
                        }

                        if (currentSpot != 0 && actualCoordIndex != 0 && GetDistanceFromDictionary(existingPairs, actualCoordIndex, currentSpot) == -1)
                        {
                            // Console.WriteLine((actualCoordIndex, currentSpot) + " " + distance);
                            existingPairs[(actualCoordIndex, currentSpot)] = distance;
                        }
                        // else
                        // {
                        //     Console.WriteLine((coordIndex, currentSpot) + " " + distance);

                        // }

                        newListToCheck.Add(check);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        continue;
                    }
                }
            }
            listOfCoordsToCheck = newListToCheck;
            distance++;
        }

        return existingPairs;
    }

    private static long? GetDistanceFromDictionary(Dictionary<(int, int), long> dict, int valueOne, int valueTwo)
    {
        long value = 0;

        if (!dict.TryGetValue((valueOne, valueTwo), out value))
        {
            dict.TryGetValue((valueTwo, valueOne), out value);
        }

        return value == 0 ? null : value;
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\day_seven_input.txt");
        return -1;
    }
}