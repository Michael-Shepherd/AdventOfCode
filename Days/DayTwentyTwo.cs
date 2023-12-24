using System.Dynamic;
using Microsoft.VisualBasic;
using Utilities;

namespace Days;

public static class DayTwentyTwo
{
    public static long Handle()
    {
        return HandleStepOne();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\twentyTwo.txt");
        var bricks = new List<Brick>();
        int maxX = 0;
        int maxY = 0;
        int maxZ = 0;

        foreach (var line in input)
        {
            var brick = new Brick(line);
            bricks.Add(brick);
        }

        bricks.Sort((a, b) => a.LowestPoint.CompareTo(b.LowestPoint));

        Console.WriteLine(string.Join("\n", bricks));

        return -1;
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\twentyTwo.txt");
        return -1;
    }
}

class Brick
{
    public (int x, int y, int z) Start { get; set; }
    public (int x, int y, int z) End { get; set; }
    public int LowestPoint { get; set; }
    public int HighestPoint { get; set; }

    public bool IsUnderBrick(Brick brick)
    {
        // if (brick.LowestPoint - this.HighestPoint != 1)
        // {
        //     return false;
        // }
        // if ((this.Start.x, this.Start.y, this.End.x, this.End.y).FindIntersection((brick.Start.x, brick.Start.y, brick.End.x, brick.End.y)) is not null)
        // {
        //     return true;
        // }

        return false;
    }

    public Brick(string inputLine)
    {
        var start = inputLine.Split("~").First();
        var end = inputLine.Split("~").Last();

        var startSplit = start.Split(",").Select(s => int.Parse(s)).ToList();
        Start = (startSplit[0], startSplit[1], startSplit[2]);
        LowestPoint = Start.z;
        HighestPoint = Start.z;

        var endSplit = end.Split(",").Select(s => int.Parse(s)).ToList();
        End = (endSplit[0], endSplit[1], endSplit[2]);
        if (End.z < LowestPoint)
        {
            LowestPoint = End.z;
        }
        else
        {
            HighestPoint = End.z;
        }
    }
    public override string ToString() => $"Start: ({Start.x}, {Start.y}, {Start.z}), End: ({End.x}, {End.y}, {End.z}), LowestPoint: {LowestPoint}, HighestPoint: {HighestPoint}";
}