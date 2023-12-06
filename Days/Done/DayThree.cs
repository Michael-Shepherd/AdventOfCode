namespace Days;

using Utilities;

public static class DayThree
{
    public static long Handle()
    {
        return HandlePartTwo();
    }

    private static long HandlePartTwo()
    {
        string[] input = InputReader.Get(".\\input\\day_three_input.txt");
        // part, row, col
        var partsSpotList = new List<(string, List<(int, int)>)>();
        var listOfStarLocations = new List<(int, int)>();

        for (int i = 0; i < input.Length; i++)
        {
            var currentNumber = "";
            var listOfLocations = new List<(int, int)>();
            for (var j = 0; j < input[i].Length; j++)
            {
                var item = input[i][j];

                if (item == '*')
                {
                    listOfStarLocations.Add((i, j));
                }

                if (char.IsNumber(item))
                {
                    currentNumber = currentNumber + item;
                    listOfLocations.Add((i, j));

                    if (j == input[i].Length - 1)
                    {
                        partsSpotList.Add((currentNumber, listOfLocations));
                    }
                }
                else if (currentNumber != "")
                {
                    // track all locations of number
                    partsSpotList.Add((currentNumber, listOfLocations));

                    listOfLocations = new List<(int, int)>();
                    currentNumber = "";
                }
            }
        }

        char[][] inputMatrix = input.Select(item => item.ToArray()).ToArray();

        var sumOfGearRatios = 0;
        foreach (var gear in listOfStarLocations)
        {
            Console.WriteLine($"{gear} - {inputMatrix[gear.Item1][gear.Item2]} ");
            var touchingParts = getListOfTouchingParts(gear.Item1, gear.Item2, partsSpotList);
            if (touchingParts.Count() == 2)
            {
                int.TryParse(touchingParts[0], out var one);
                int.TryParse(touchingParts[1], out var two);
                Console.WriteLine($"{one} * {two}");

                sumOfGearRatios += one * two;
            }
        }

        return sumOfGearRatios;
    }

    private static List<string> getListOfTouchingParts(int row, int col, List<(string, List<(int, int)>)> parts)
    {
        var touchingParts = new List<string>();

        var up = (row - 1, col);
        var down = (row + 1, col);
        var left = (row, col - 1);
        var right = (row, col + 1);
        var upRight = (row - 1, col + 1);
        var upLeft = (row - 1, col - 1);
        var downRight = (row + 1, col + 1);
        var downLeft = (row + 1, col - 1);

        var touchingCoOrdinates = new List<(int, int)>() { up, down, left, right, upRight, upLeft, downRight, downLeft };

        foreach (var part in parts)
        {


            if (part.Item2.Intersect(touchingCoOrdinates).Any())
            {
                foreach (var coOrd in part.Item2)
                {
                    Console.Write(coOrd);
                }
                Console.WriteLine("");
                touchingParts.Add(part.Item1);
            }
        }

        foreach (var coOrd in touchingParts)
        {
            Console.Write(coOrd);
        }
        Console.WriteLine("");
        Console.WriteLine("***");

        return touchingParts.ToList();
    }

    private static long HandlePartOne()
    {
        string[] input = InputReader.Get(".\\input\\day_three_input.txt");
        // part, row, col
        var partsSpotList = new List<(string, List<(int, int)>)>();
        var partsList = new List<int>();

        for (int i = 0; i < input.Length; i++)
        {
            var currentNumber = "";
            var listOfLocations = new List<(int, int)>();
            for (var j = 0; j < input[i].Length; j++)
            {
                var item = input[i][j];
                if (char.IsNumber(item))
                {
                    currentNumber = currentNumber + item;
                    listOfLocations.Add((i, j));
                    if (j == input[i].Length - 1)
                    {
                        partsSpotList.Add((currentNumber, listOfLocations));
                    }
                }
                else if (currentNumber != "")
                {
                    // track all locations of number
                    partsSpotList.Add((currentNumber, listOfLocations));

                    listOfLocations = new List<(int, int)>();
                    currentNumber = "";
                }
            }
        }

        char[][] inputMatrix = input.Select(item => item.ToArray()).ToArray();

        foreach (var item in partsSpotList)
        {
            foreach (var coOrd in item.Item2)
            {
                if (IsLocationTouchingMachine(coOrd.Item1, coOrd.Item2, inputMatrix))
                {
                    int.TryParse(item.Item1, out int partNUmber);
                    partsList.Add(partNUmber);
                    // Console.WriteLine(item.Item1);
                    break;
                }
            }


        }

        // foreach (var item in partsList)
        // {
        //     Console.WriteLine($"Required Part: {item}");
        // }
        return partsList.Sum();
    }

    private static bool IsLocationTouchingMachine(int row, int col, char[][] inputMatrix)
    {
        var minRow = 0;
        var maxRow = inputMatrix.Length - 1;
        var minCol = 0;
        var maxCol = inputMatrix[0].Length - 1;

        // for (int i = 0; i < inputMatrix.Length; i++)
        // {
        //     for (int j = 0; j < inputMatrix[i].Length; j++)
        //     {
        //         Console.Write(inputMatrix[i][j]);
        //     }
        //     Console.WriteLine();
        // }

        var up = (row - 1, col);
        var down = (row + 1, col);
        var left = (row, col - 1);
        var right = (row, col + 1);
        var upRight = (row - 1, col + 1);
        var upLeft = (row - 1, col - 1);
        var downRight = (row + 1, col + 1);
        var downLeft = (row + 1, col - 1);

        if (row == maxRow && col == minRow)
        {
            Console.WriteLine($"right: {right} {inputMatrix[right.Item1][right.Item2]}");
        }

        var touchingCoOrdinates = new List<(int, int)>() { up, down, left, right, upRight, upLeft, downRight, downLeft };
        foreach (var coOrd in touchingCoOrdinates)
        {
            if (coOrd.Item1 > maxRow
            || coOrd.Item1 < minRow
            || coOrd.Item2 < minCol
            || coOrd.Item2 > maxCol)
            {
                continue;
            }

            var touchingChar = inputMatrix[coOrd.Item1][coOrd.Item2];
            if (touchingChar == '.' || char.IsDigit(touchingChar))
            {
                continue;
            }

            return true;
        }

        return false;
    }

}