using Utilities;

namespace Days;

public static class DaySix
{
    public static long Handle()
    {

        return HandleStepTwo();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\day_six_input.txt");
        var times = input.First().Split(":").Last().Split(" ").Where(time => !string.IsNullOrEmpty(time)).ToList();
        var distancesToBeat = input.Last().Split(":").Last().Split(" ").Where(time => !string.IsNullOrEmpty(time)).ToList();

        long result = 1;
        for (int i = 0; i < times.Count(); i++)
        {
            long.TryParse(times[i], out var time);
            long.TryParse(distancesToBeat[i], out long distanceToBeat);
            result = result * CalculateNumberOfWinningCombos(time, distanceToBeat);
        }

        return result;
    }

    private static long CalculateNumberOfWinningCombos(int time, int distanceToBeat)
    {
        var combos = 0;

        for (int i = 0; i <= time; i++)
        {
            var speed = i;
            var distance = speed * (time - i);

            if (distance > distanceToBeat)
            {
                combos++;
            }
        }

        return combos;
    }

    private static long CalculateNumberOfWinningCombos(long time, long distanceToBeat)
    {
        var combos = 0;

        for (int i = 0; i <= time; i++)
        {
            var speed = i;
            var distance = speed * (time - i);

            if (distance > distanceToBeat)
            {
                combos++;
            }
        }

        return combos;
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\day_six_input.txt");
        var time = string.Join("", input.First().Split(":").Last().Split(" ").Where(time => !string.IsNullOrEmpty(time)).ToList());
        var distanceToBeat = string.Join("", input.Last().Split(":").Last().Split(" ").Where(time => !string.IsNullOrEmpty(time)).ToList());

        long.TryParse(time, out var _time);
        long.TryParse(distanceToBeat, out var _distanceToBeat);
        var combos = CalculateNumberOfWinningCombos(_time, _distanceToBeat);

        return combos;
    }
}