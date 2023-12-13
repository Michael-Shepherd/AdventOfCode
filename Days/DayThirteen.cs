using System.ComponentModel.DataAnnotations;
using Utilities;

namespace Days;

public static class DayThirteen
{
    public static long Handle()
    {
        return HandleStepTwo();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\thirteen.txt");

        var sharedPalindromeInflections = new List<(int, int)>();
        var currentSet = new List<List<string>>();
        var currentCount = 0;
        var currentColumnsToLeftOfReflection = 0;
        var currentRowsAboveReflection = 0;
        var isFirstLine = true;

        for (int i = 0; i < input.Length; i++)
        {
            var line = input[i].Trim();
            if (!string.IsNullOrEmpty(line))
            {
                currentSet.Add(line.Select(c => c + "").ToList());

                if (isFirstLine)
                {
                    isFirstLine = false;
                    sharedPalindromeInflections = GetPalindromeStartAndEnds(line);
                }

                if (sharedPalindromeInflections.Count() == 0 && !isFirstLine)
                {

                }
                else
                {
                    sharedPalindromeInflections = GetSharedPalindromes(sharedPalindromeInflections, line);
                }
            }

            if (string.IsNullOrEmpty(line) || i == input.Length - 1)
            {
                // If col slice, set result, else check list slice
                if (sharedPalindromeInflections.Any())
                {
                    var startIndex = sharedPalindromeInflections.First().Item1;
                    var endIndex = sharedPalindromeInflections.First().Item2;
                    currentColumnsToLeftOfReflection = (endIndex - startIndex) / 2 + startIndex;

                    Console.WriteLine("COLS: " + string.Join(" ", sharedPalindromeInflections));
                    Console.WriteLine(currentColumnsToLeftOfReflection);
                }
                else
                {
                    var rowPalindromes = GetPalindromeListsStartAndEnds(currentSet);

                    var startIndex = rowPalindromes.First().Item1;
                    var endIndex = rowPalindromes.First().Item2 - 1;

                    currentRowsAboveReflection = (endIndex - startIndex) / 2 + startIndex + 1;
                    Console.WriteLine("ROWS: " + string.Join(" ", rowPalindromes));
                    Console.WriteLine(currentRowsAboveReflection);
                }

                isFirstLine = true;
                sharedPalindromeInflections = new List<(int, int)>();
                currentCount += currentColumnsToLeftOfReflection + 100 * currentRowsAboveReflection;
                currentSet = new List<List<string>>();
                currentColumnsToLeftOfReflection = 0;
                currentRowsAboveReflection = 0;
            }
        }

        return currentCount;
    }

    public static List<(int, int)> GetSharedPalindromes(List<(int, int)> sharedPalindromeInflections, string line)
    {
        var newInflections = new List<(int, int)>();
        foreach (var inflection in sharedPalindromeInflections)
        {
            var start = inflection.Item1;
            var end = inflection.Item2;
            var subString = line[start..end];
            if (subString.IsPalindrome())
            {
                newInflections.Add(inflection);
            }
        }
        return newInflections;
    }

    public static List<(int, int)> GetPalindromeListsStartAndEnds(List<List<string>> input)
    {
        var palindromeIndices = new List<(int, int)>();

        for (int i = 0; i < input.Count() - 1; i++)
        {
            for (int j = i + 2; j <= input.Count(); j++)
            {
                if (i == 0 || j == input.Count())
                {
                    var lists = input[i..j];

                    if (lists.IsPalindromeEvenOnly())
                    {
                        palindromeIndices.Add((i, j));
                    }
                }
            }
        }

        return palindromeIndices;
    }

    // start, end
    public static List<(int, int)> GetPalindromeStartAndEnds(string line)
    {
        var palindromeIndices = new List<(int, int)>();

        for (int i = 0; i < line.Length - 1; i++)
        {
            for (int j = i + 2; j <= line.Length; j++)
            {
                var subString = line[i..j];
                if (subString.Length % 2 == 0 && subString.IsPalindrome())
                {
                    if (i == 0 || j == line.Length)
                    {
                        palindromeIndices.Add((i, j));
                    }
                }
            }
        }
        return palindromeIndices;
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\thirteen.txt");

        var sharedPalindromeInflections = new List<(int, int)>();
        var currentSet = new List<List<string>>();
        var currentCount = 0;
        var currentColumnsToLeftOfReflection = 0;
        var currentRowsAboveReflection = 0;
        var isFirstLine = true;

        for (int i = 0; i < input.Length; i++)
        {
            var selectedColumns = (0, 0);
            var selectedRows = (0, 0);
            var line = input[i].Trim();
            if (!string.IsNullOrEmpty(line))
            {
                currentSet.Add(line.Select(c => c + "").ToList());

                if (isFirstLine)
                {
                    isFirstLine = false;
                    sharedPalindromeInflections = GetPalindromeStartAndEnds(line);
                }

                if (sharedPalindromeInflections.Count() == 0 && !isFirstLine)
                {

                }
                else
                {
                    sharedPalindromeInflections = GetSharedPalindromes(sharedPalindromeInflections, line);
                }
            }

            if (string.IsNullOrEmpty(line) || i == input.Length - 1)
            {
                // If col slice, set result, else check list slice
                if (sharedPalindromeInflections.Any())
                {
                    var startIndex = sharedPalindromeInflections.First().Item1;
                    var endIndex = sharedPalindromeInflections.First().Item2;
                    currentColumnsToLeftOfReflection = (endIndex - startIndex) / 2 + startIndex;
                    selectedColumns = (startIndex, endIndex);
                    // Console.WriteLine("COLS: " + string.Join(" ", sharedPalindromeInflections));
                    // Console.WriteLine(currentColumnsToLeftOfReflection);
                }
                else
                {
                    var rowPalindromes = GetPalindromeListsStartAndEnds(currentSet);

                    var startIndex = rowPalindromes.First().Item1;
                    var endIndex = rowPalindromes.First().Item2 - 1;

                    currentRowsAboveReflection = (endIndex - startIndex) / 2 + startIndex + 1;
                    selectedRows = (startIndex, endIndex);

                    // Console.WriteLine("ROWS: " + string.Join(" ", rowPalindromes));
                    // Console.WriteLine(currentRowsAboveReflection);
                }

                // TODO: Find Smudge
                var dataForStepTwo = GetStepTwoDataSets(currentSet);

                foreach (var set in dataForStepTwo)
                {
                    isFirstLine = true;
                    var selectedPalindrome = (0, 0);
                    foreach (var row in set)
                    {
                        var newLine = new string(row.Select(a => a[0]).ToArray());
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            sharedPalindromeInflections = new List<(int, int)>();
                            sharedPalindromeInflections = GetPalindromeStartAndEnds(newLine);
                        }
                        if (sharedPalindromeInflections.Count() == 0 && !isFirstLine)
                        {
                            continue;
                        }
                        else
                        {
                            sharedPalindromeInflections = GetSharedPalindromes(sharedPalindromeInflections, newLine);
                        }
                    }

                    // If col slice, set result, else check list slice
                    sharedPalindromeInflections.Remove(selectedColumns);
                    if (sharedPalindromeInflections.Any())
                    {
                        var correctPalindrome = sharedPalindromeInflections.First();
                        var maxSize = correctPalindrome.Item2 - correctPalindrome.Item1;
                        if (sharedPalindromeInflections.Count() > 1)
                        {
                            foreach (var pal in sharedPalindromeInflections)
                            {
                                if (maxSize < pal.Item2 - pal.Item1)
                                {
                                    correctPalindrome = pal;
                                }
                            }
                        }
                        if (maxSize < selectedPalindrome.Item2 - selectedPalindrome.Item1)
                        {
                            continue;
                        }

                        selectedPalindrome = correctPalindrome;

                        var startIndex = sharedPalindromeInflections.First().Item1;
                        var endIndex = sharedPalindromeInflections.First().Item2;
                        currentColumnsToLeftOfReflection = (endIndex - startIndex) / 2 + startIndex;
                        currentRowsAboveReflection = 0;
                        Console.WriteLine("COLS: " + string.Join(" ", sharedPalindromeInflections));
                        Console.WriteLine(currentColumnsToLeftOfReflection);
                        // foreach (var row in set)
                        // {
                        //     Console.WriteLine(string.Join(" ", row));
                        // }
                    }
                    else
                    {
                        var rowPalindromes = GetPalindromeListsStartAndEnds(set);
                        rowPalindromes.Remove(selectedRows);
                        if (rowPalindromes.Any())
                        {
                            var correctPalindrome = rowPalindromes.First();
                            var maxSize = correctPalindrome.Item2 - correctPalindrome.Item1;
                            if (rowPalindromes.Count() > 1)
                            {
                                foreach (var pal in rowPalindromes)
                                {
                                    if (maxSize < pal.Item2 - pal.Item1)
                                    {
                                        correctPalindrome = pal;
                                    }
                                }
                            }
                            if (maxSize < selectedPalindrome.Item2 - selectedPalindrome.Item1)
                            {
                                continue;
                            }

                            selectedPalindrome = correctPalindrome;

                            var startIndex = correctPalindrome.Item1;
                            var endIndex = correctPalindrome.Item2 - 1;

                            currentColumnsToLeftOfReflection = 0;
                            currentRowsAboveReflection = (endIndex - startIndex) / 2 + startIndex + 1;
                            Console.WriteLine("ROWS: " + string.Join(" ", rowPalindromes));
                            Console.WriteLine(currentRowsAboveReflection);

                            // foreach (var row in set)
                            // {
                            //     Console.WriteLine(string.Join(" ", row));
                            // }
                        }
                    }

                    isFirstLine = true;
                    sharedPalindromeInflections = new List<(int, int)>();
                }



                isFirstLine = true;
                sharedPalindromeInflections = new List<(int, int)>();
                currentCount += currentColumnsToLeftOfReflection + 100 * currentRowsAboveReflection;
                currentSet = new List<List<string>>();
                currentColumnsToLeftOfReflection = 0;
                currentRowsAboveReflection = 0;
            }
        }

        return currentCount;
    }

    private static List<List<List<string>>> GetStepTwoDataSets(List<List<string>> original)
    {
        var ListOfListsOfLists = new List<List<List<string>>>();
        for (int i = 0; i < original.Count(); i++)
        {
            for (int j = 0; j < original[0].Count(); j++)
            {
                var newList = new List<List<string>>();

                original.ForEach((item) =>
                {
                    var wololo = item.ToList();
                    newList.Add(wololo);
                });

                newList[i][j] = original[i][j] == "." ? "#" : ".";
                ListOfListsOfLists.Add(newList);
            }
        }

        return ListOfListsOfLists;
    }
}