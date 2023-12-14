using System.Text;
using Utilities;

namespace Days;

public static class DayTwelve
{
    private static readonly char WORKING = '.';
    private static readonly char DAMAGED = '#';
    private static readonly char UNKNOWN = '?';


    public static long Handle()
    {
        return HandleStepOne();
    }

    // public static long HandleStepOneDifferently()
    // {

    // }

    // // if false, returns the failed prefix to filter out
    // private static (bool, string) DoesLineProduceCorrectCombo(string lineData, int comboToCheck)
    // {
    //     var previousChar = lineData.First();
    //     var currentCharCount = 0;
    //     int? resultInt = null;
    //     var resultString = "";
    //     var isCorrectCombo = false;

    //     for (int i = 0; i < lineData.Length; i++)
    //     {
    //         var character = lineData[i];
    //         resultString += character;
    //         if (character == previousChar)
    //         {
    //             currentCharCount++;
    //         }
    //         else
    //         {
    //             if (previousChar == DAMAGED)
    //             {
    //                 resultInt = int.Parse((resultInt is null ? "" : resultInt.ToString()) + currentCharCount.ToString());
    //                 var diffInComboLenght = comboToCheck.Length() - resultInt?.Length() ?? 0;
    //                 if (diffInComboLenght == 0)
    //                 {
    //                     break;
    //                 }
    //                 else if ((comboToCheck / (10 * diffInComboLenght)) != resultInt)
    //                 {

    //                 }
    //             }
    //             currentCharCount = 1;
    //         }


    //         previousChar = character;
    //     }

    //     if (previousChar != WORKING)
    //     {
    //         resultInt = int.Parse((resultInt is null ? "" : resultInt.ToString()) + currentCharCount.ToString());
    //     }

    //     return resultInt ?? 0;
    // }



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