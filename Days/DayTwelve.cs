using System.Text.RegularExpressions;
using Utilities;

namespace Days;

public static class DayTwelve
{
    private static readonly char WORKING = '.';
    private static readonly char DAMAGED = '#';
    private static readonly char UNKNOWN = '?';
    private static readonly Regex UNKNOWN_REGEX = new Regex(Regex.Escape(UNKNOWN + ""));

    public static long Handle()
    {
        var input = InputReader.Get(".\\input\\twelve.txt");
        var numberCounts = new Dictionary<int, int>();

        foreach (var line in input)
        {
            var unknownCount = line.Count(a => a == UNKNOWN);
            if (numberCounts.TryGetValue(unknownCount, out var count))
            {
                numberCounts[unknownCount]++;
            }
            else
            {
                numberCounts.Add(unknownCount, 1);
            }
        }

        foreach (var n in numberCounts)
        {
            Console.WriteLine(n.Key + " occurrences: " + n.Value);
        }

        return HandleStepOneDifferently();
    }

    public static long HandleStepOneDifferently()
    {
        var input = InputReader.Get(".\\input\\twelve.txt");
        var unknownPermDectionary = new Dictionary<string, List<string>>();

        // Parallel.For(10, 18, i =>
        // {
        //     Console.WriteLine("starting " + i);
        //     var dict = GeneratePreCacheData(i);
        //     foreach (var perm in dict)
        //     {
        //         lock (unknownPermDectionary)
        //         {
        //             unknownPermDectionary.TryAdd(perm.Key, perm.Value);
        //         }
        //     }
        //     Console.WriteLine("done " + i);
        // });

        long totalCount = 0;
        Parallel.For(0, input.Length, i =>
        {
            var line = input[i];
            var lineCount = 0;
            (var validConfigs, var comboToCheck, var localDict) =
            GetValidConfigurations(line, unknownPermDectionary);

            var localDictTest = localDict;
            foreach (var perm in localDictTest)
            {
                lock (unknownPermDectionary)
                {
                    unknownPermDectionary.TryAdd(perm.Key, perm.Value);
                }
            }

            foreach (var config in validConfigs)
            {
                if (DoesLineProduceCorrectCombo(config, comboToCheck))
                {
                    lineCount++;
                }
            }
            Console.WriteLine(line + " " + lineCount);
            totalCount += lineCount;
        });

        return totalCount;
    }

    private static Dictionary<string, IEnumerable<IEnumerable<char>>> GeneratePreCacheData(int numberOfQuestionMarks)
    {
        var unknownPermutations = new HashSet<string>();
        var unknownPermDectionary = new Dictionary<string, IEnumerable<IEnumerable<char>>>();

        for (int i = 0; i <= numberOfQuestionMarks; i++)
        {
            var working = new char[i];
            for (var j = 0; j < working.Length; j++)
            {
                working[j] = WORKING;
            }
            var damaged = new char[numberOfQuestionMarks - i];
            for (var j = 0; j < damaged.Length; j++)
            {
                damaged[j] = DAMAGED;
            }
            unknownPermutations.Add(string.Concat(working) + string.Concat(damaged));
        }

        foreach (var perm in unknownPermutations)
        {
            var permPerms = perm.Permutations();
            unknownPermDectionary.Add(perm, permPerms);
        }

        return unknownPermDectionary;
    }

    // expecting #....#... 1,1
    private static (HashSet<string>, long, Dictionary<string, List<string>>) GetValidConfigurations(string fullInputLine, Dictionary<string, List<string>> unknownPermDectionary)
    {
        var validConfigs = new HashSet<string>();
        var unknownPermutations = new HashSet<string>();
        var data = fullInputLine.Split(" ").First();
        var numberOfUnknowns = data.Count(a => a.Equals(UNKNOWN));
        var result = fullInputLine.Split(" ").Last();
        var numberOfResults = result.Split(",").Length;
        var resultNumber = int.Parse(result.Replace(",", ""));
        var unknownIndices = new List<int>();

        for (int i = 0; i <= numberOfUnknowns; i++)
        {
            var working = new char[i];
            for (var j = 0; j < working.Length; j++)
            {
                working[j] = WORKING;
            }
            var damaged = new char[numberOfUnknowns - i];
            for (var j = 0; j < damaged.Length; j++)
            {
                damaged[j] = DAMAGED;
            }
            unknownPermutations.Add(string.Concat(working) + string.Concat(damaged));
        }

        var checkedPermWords = new HashSet<string>();
        foreach (var perm in unknownPermutations)
        {
            var permPerms = perm.GetDistinctPermutations();
            // if (!unknownPermDectionary.TryGetValue(perm, out var permPerms))
            // {
            //     // permPerms = perm.Permutations();
            //     lock (unknownPermDectionary)
            //     {
            //         unknownPermDectionary.TryAdd(perm, permPerms);
            //     }
            // }

            foreach (var newPerm in permPerms)
            {
                var permWord = newPerm;

                if (checkedPermWords.TryGetValue(permWord, out var _))
                {
                    continue;
                }
                checkedPermWords.Add(permWord);
                var newWord = data;

                for (int i = 0; i < numberOfUnknowns; i++)
                {
                    newWord = UNKNOWN_REGEX.Replace(newWord, permWord[i] + "", 1, i);
                }
                validConfigs.Add(newWord);
            }
        }


        return (validConfigs, resultNumber, unknownPermDectionary);
    }

    private static bool DoesLineProduceCorrectCombo(string lineData, long comboToCheck)
    {
        bool isCorrectCombo;
        var resultInt = 0;
        var brokenStringCount = 0;

        for (int i = 0; i < lineData.Length; i++)
        {
            if (lineData[i] == DAMAGED)
            {
                brokenStringCount++;
            }
            else
            {
                if (brokenStringCount != 0)
                {
                    resultInt = resultInt * 10 + brokenStringCount;
                    brokenStringCount = 0;
                }
            }
        }

        if (brokenStringCount != 0)
        {
            resultInt = resultInt * 10 + brokenStringCount;
        }

        if (comboToCheck == 1)
        {
            Console.WriteLine(comboToCheck);
            Console.WriteLine(lineData + " " + brokenStringCount + " " + comboToCheck);
        }
        isCorrectCombo = resultInt == comboToCheck;
        return isCorrectCombo;
    }


    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\twelve.txt");

        long fullCount = 0;

        var permutationValueDict = new Dictionary<string, string>();

        Parallel.For(0, input.Length, inputIndex =>
        {
            var line = input[inputIndex];
            var data = line.Split(" ").First();
            var numbers = line.Split(" ").Last();
            var brokenExpectedCount = numbers.Split(",").Select(int.Parse).Sum();
            var existingBroken = data.Where(x => x.Equals(DAMAGED)).Count();

            var questionIndices = new List<int>();
            for (int i = 0; i < data.Length; i++)
            {
                var character = data[i];
                if (character == UNKNOWN)
                {
                    questionIndices.Add(i);
                }
            }

            var permutationInserts = new HashSet<string>();
            for (int i = 0; i < questionIndices.Count() + 1; i++)
            {
                var damagedCount = questionIndices.Count() - i;
                if (damagedCount + existingBroken > brokenExpectedCount)
                {
                    continue;
                }

                var newWord = string.Concat(Enumerable.Repeat(WORKING, i)) + string.Concat(Enumerable.Repeat(DAMAGED, damagedCount));

                foreach (var perm in newWord.Permutations())
                {
                    permutationInserts.Add(string.Concat(perm));
                }
            }

            long countOfMatching = 0;
            var dataWord = data.ToList();
            var permutations = new HashSet<string>();
            foreach (var perm in permutationInserts)
            {
                var newWord = dataWord;
                for (int i = 0; i < questionIndices.Count(); i++)
                {
                    newWord[questionIndices[i]] = perm[i];
                }
                var newWordAsString = new string(newWord.ToArray());
                if (!permutations.TryGetValue(newWordAsString, out var wololo))
                {
                    permutations.Add(newWordAsString);
                    if (permutationValueDict.TryGetValue(newWordAsString, out var combo))
                    {
                        if (combo == numbers)
                        {
                            countOfMatching++;
                        }
                    }
                    else
                    {
                        var newCombo = GetCombinationSummary(newWordAsString);
                        lock (permutationValueDict)
                        {
                            permutationValueDict.Add(newWordAsString, newCombo);
                        }
                        if (newCombo == numbers)
                        {
                            countOfMatching++;
                        }
                    }
                }
            }

            Console.WriteLine("Word: " + data + "Count: " + countOfMatching);
            Console.WriteLine("******");

            fullCount += countOfMatching;
        });
        return fullCount;
    }

    private static string GetCombinationSummary(string lineData)
    {
        var previousChar = lineData.First();
        var currentCharCount = 0;
        var result = "";
        foreach (var character in lineData)
        {
            if (character == previousChar)
            {
                currentCharCount++;
            }
            else
            {
                if (previousChar == DAMAGED)
                {
                    result += currentCharCount + ",";
                }
                currentCharCount = 1;
            }

            previousChar = character;
        }

        if (previousChar == WORKING)
        {
            result = result.Remove(result.Length - 1);
        }
        else
        {
            result += currentCharCount;
        }

        return result;
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\day_seven_input.txt");
        return -1;
    }
}