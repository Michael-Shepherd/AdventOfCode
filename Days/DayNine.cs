using Utilities;

namespace Days;

public static class DayNine
{
    public static long Handle()
    {
        return HandleStepTwo();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\nine.txt");


        long sumOfAdditions = 0;
        foreach (var line in input)
        {
            var currentNumbers = line.Split(" ").Select(long.Parse).ToList();
            var originalLastNumber = currentNumbers.Last();
            var numberRows = new List<List<long>>() { };
            while (true)
            {
                var newCurrentNumbers = new List<long>();
                for (int i = 1; i < currentNumbers.Count(); i++)
                {
                    newCurrentNumbers.Add(currentNumbers[i] - currentNumbers[i - 1]);
                }
                var distinctCurrent = newCurrentNumbers.Distinct();
                if (distinctCurrent.Count() == 1 && distinctCurrent.First() == 0)
                {
                    break;
                }
                numberRows.Add(newCurrentNumbers);
                currentNumbers = newCurrentNumbers;
            }
            var lastNumbers = numberRows.Select(n => n.Last()).Append(originalLastNumber);
            Console.WriteLine(line + ": " + string.Join(", ", lastNumbers));
            sumOfAdditions += lastNumbers.Sum();
        }

        return sumOfAdditions;
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\nine.txt");

        long sumOfAdditions = 0;
        foreach (var line in input)
        {
            var currentNumbers = line.Split(" ").Select(long.Parse).ToList();
            var originalLastNumber = currentNumbers.Last();
            var originalFirst = currentNumbers.First();
            var numberRows = new List<List<long>>() { };
            while (true)
            {
                var newCurrentNumbers = new List<long>();
                for (int i = 1; i < currentNumbers.Count(); i++)
                {
                    newCurrentNumbers.Add(currentNumbers[i] - currentNumbers[i - 1]);
                }
                var distinctCurrent = newCurrentNumbers.Distinct();
                if (distinctCurrent.Count() == 1 && distinctCurrent.First() == 0)
                {
                    numberRows.Add(newCurrentNumbers);
                    break;
                }
                numberRows.Add(newCurrentNumbers);
                currentNumbers = newCurrentNumbers;
            }
            Console.WriteLine(line);
            foreach (var nums in numberRows)
            {
                Console.WriteLine(string.Join(" ", nums));
            }
            var firstNumbers = numberRows.Select(n => n.First()).Reverse().ToArray();

            Console.WriteLine("First Numbers: " + string.Join(" ", firstNumbers));

            var differences = new List<long>() { 0 };
            for (int i = 1; i < firstNumbers.Length; i++)
            {
                Console.WriteLine(firstNumbers[i] + " - " + differences[i - 1] + " = " + (firstNumbers[i] - differences[i - 1]));
                differences.Add(firstNumbers[i] - differences[i - 1]);
            }

            Console.WriteLine("Differences: " + string.Join(" ", differences));
            Console.WriteLine("Result? " + (originalFirst - differences.Last()));
            sumOfAdditions += originalFirst - differences.Last();
        }

        return sumOfAdditions;
    }
}