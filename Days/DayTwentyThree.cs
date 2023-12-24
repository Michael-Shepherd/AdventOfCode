using Utilities;

namespace Days;

public static class DayTwentyThree
{
    public static long Handle()
    {
        return HandleStepOne();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\twentyThree.txt");
        return -1;
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\day_seven_input.txt");
        return -1;
    }
}

class InputObject
{
    public static int Value { get; set; }

    public InputObject(string inputLine)
    {
        inputLine.Split("").First();
        inputLine.Split("").Last();
    }
}