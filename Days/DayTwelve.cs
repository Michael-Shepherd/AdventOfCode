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
        return HandleStepOneDifferently();
    }

    public static long HandleStepOneDifferently()
    {
        var input = InputReader.Get(".\\input\\twelve.txt");

        Console.WriteLine(input.Where(line => !line.Contains(UNKNOWN)).Count());

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

        if (numberOfUnknowns == 0)
        {
            return (new HashSet<string>() { data }, resultNumber, unknownPermDectionary);
        }

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

        isCorrectCombo = resultInt == comboToCheck;
        return isCorrectCombo;
    }
}