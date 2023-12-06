namespace Days;

using Utilities;

public static class DayOne
{
    public static long HandleDayOne()
    {
        var output = 0;
        var input = InputReader.Get(".\\input\\day_one_input.txt");
        foreach (var line in input)
        {
            var firstNumber = GetFirstNumber(line);
            var secondNumber = GetLastNumber(line);

            // Comment for step 1
            var numbers = new List<int>() { firstNumber, secondNumber };

            // Uncomment for step 1
            // var numbers = new List<int>() { firstNumber, secondNumber };
            // foreach (char character in workingLine)
            // {
            //     if (Char.IsNumber(character))
            //     {
            //         numbers.Add((int)Char.GetNumericValue(character));
            //     }
            // }

            if (int.TryParse($"{numbers.First()}{numbers.Last()}", out var fullNumber))
            {
                Console.WriteLine($"{line} {fullNumber}");
                output += fullNumber;
            }
            else
            {
                Console.WriteLine($"Broken crap {line}");
            }
        }

        return output;
    }

    private static int GetFirstNumber(string inputLine)
    {
        inputLine = inputLine.ToLowerInvariant();
        for (int i = 0; i < inputLine.Length; i++)
        {
            if (Char.IsNumber(inputLine[i]))
            {
                return (int)Char.GetNumericValue(inputLine[i]);
            }

            var substring = inputLine.Substring(0, i + 1);
            int? stringNumber = intInSubstring(substring);

            if (stringNumber is not null)
            {
                return (int)stringNumber;
            }
        }
        return -1;
    }

    private static int GetLastNumber(string inputLine)
    {
        inputLine = inputLine.ToLowerInvariant();
        for (int i = inputLine.Length - 1; i >= 0; i--)
        {
            if (Char.IsNumber(inputLine[i]))
            {
                return (int)Char.GetNumericValue(inputLine[i]);
            }

            var substring = inputLine.Substring(i, inputLine.Length - i);
            int? stringNumber = intInSubstring(substring);

            if (stringNumber is not null)
            {
                return (int)stringNumber;
            }
        }
        return -1;
    }

    private static int? intInSubstring(string substring)
    {
        if (substring.Contains("one"))
        {
            return 1;
        }
        if (substring.Contains("two"))
        {
            return 2;
        }
        if (substring.Contains("three"))
        {
            return 3;
        }
        if (substring.Contains("four"))
        {
            return 4;
        }
        if (substring.Contains("five"))
        {
            return 5;
        }
        if (substring.Contains("six"))
        {
            return 6;
        }
        if (substring.Contains("seven"))
        {
            return 7;
        }
        if (substring.Contains("eight"))
        {
            return 8;
        }
        if (substring.Contains("nine"))
        {
            return 9;
        }

        return null;
    }
}