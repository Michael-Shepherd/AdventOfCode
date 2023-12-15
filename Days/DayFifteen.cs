using System.Transactions;
using Utilities;

namespace Days;

public static class DayFifteen
{
    public static long Handle()
    {
        return HandleStepTwo();
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\fifteen.txt").Flatten().Trim().Replace("\n", "").Replace(" ", "");
        var inputList = input.Split(',');
        var boxes = new Dictionary<int, List<(string, string)>>();
        for (int i = 0; i < 256; i++)
        {
            boxes.Add(i, new List<(string, string)>());
        }

        foreach (var inputToHash in inputList)
        {

            var split = inputToHash.Split("=");
            var label = "";
            var focalLength = "";
            bool shouldRemove = false;

            if (split.Length != 2)
            {
                label = inputToHash.Split("-").First();
                shouldRemove = true;
            }
            else
            {
                label = split.First();
                focalLength = split.Last();
            }

            var boxIndex = (int)label.HashAlgo();
            var currentLenses = boxes[boxIndex];
            var editedOrRemoved = false;
            foreach (var lense in currentLenses)
            {
                if (lense.Item1 == label)
                {
                    if (shouldRemove)
                    {
                        boxes[boxIndex].Remove(lense);
                        editedOrRemoved = true;
                        break;
                    }
                    else
                    {
                        var index = boxes[boxIndex].IndexOf(lense);
                        boxes[boxIndex][index] = (label, focalLength);
                        editedOrRemoved = true;
                        break;
                    }
                }

            }

            if (!editedOrRemoved && !shouldRemove)
            {
                boxes[boxIndex].Add((label, focalLength));
            }
        }
        VisualiseBoxes(boxes);

        var count = 0;
        for (int i = 0; i < boxes.Count; i++)
        {
            var list = boxes[i];
            if (list.Count == 0)
            {
                continue;
            }

            var boxSum = 0;
            var first = i + 1;
            for (int j = 0; j < list.Count; j++)
            {
                var second = j + 1;
                var localSum = first * second * int.Parse(list[j].Item2);

                boxSum += localSum;
            }
            count += boxSum;
        }

        return count;
    }

    private static void VisualiseBoxes<T, T1, T2>(Dictionary<T, List<(T1, T2)>> boxes)
    {
        foreach (var box in boxes)
        {
            if (box.Value.Count > 0)
            {

                Console.WriteLine(box.Key + " " + string.Join(" ", box.Value));
            }
        }
        Console.WriteLine();
    }

    private static long HashAlgo(this string input)
    {
        long currentValue = 0;
        foreach (var letter in input)
        {
            currentValue += (int)letter;
            currentValue *= 17;
            currentValue = currentValue % 256;
        }
        return currentValue; ;
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\fifteen.txt").Flatten().Trim().Replace("\n", "").Replace(" ", "");
        var inputList = input.Split(',');

        var listOfCurrentValues = new List<long>();
        foreach (var inputToHash in inputList)
        {
            listOfCurrentValues.Add(HashAlgo(inputToHash));
        }

        return listOfCurrentValues.Sum();
    }
}