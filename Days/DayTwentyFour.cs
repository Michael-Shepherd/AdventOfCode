using Microsoft.VisualBasic;
using Utilities;

namespace Days;

public static class DayTwentyFour
{
    private static long MAX = 400000000000000;
    private static long MIN = 200000000000000;
    public static long Handle()
    {
        return HandleStepOne();
    }
    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\twentyFour.txt");
        var hailstones = new List<HailstoneLine>();
        long inCount = 0;

        foreach (var line in input)
        {
            var hailstone = new HailstoneLine(line);
            foreach (var stone in hailstones)
            {
                var intersection = stone.LineAsXandYPoints.FindIntersection(hailstone.LineAsXandYPoints);
                if (intersection is not null)
                {
                    if (intersection?.x >= MIN && intersection?.x <= MAX
                    && intersection?.y >= MIN && intersection?.y <= MAX)
                    {
                        inCount++;
                    }
                }
            }
            hailstones.Add(hailstone);
        }
        return inCount;
    }

    public static long HandleStepOne()
    {

        var input = InputReader.Get(".\\input\\twentyFour.txt");
        var hailstones = new List<HailstoneLine>();
        long inCount = 0;

        foreach (var line in input)
        {
            var hailstone = new HailstoneLine(line);
            foreach (var stone in hailstones)
            {
                var intersection = stone.LineAsXandYPoints.FindIntersection(hailstone.LineAsXandYPoints);
                if (intersection is not null)
                {
                    if (intersection?.x >= MIN && intersection?.x <= MAX
                    && intersection?.y >= MIN && intersection?.y <= MAX)
                    {
                        inCount++;
                    }
                }
            }
            hailstones.Add(hailstone);
        }
        return inCount;
    }
}

class HailstoneLine
{
    private long MAX = 400000000000000;
    private long MIN = 200000000000000;

    public (long x, long y, long z) Point { get; set; }
    public (long x, long y, long z) StartPoint { get; set; }
    public (long x, long y, long z) EndPoint { get; set; }
    public (long x, long y, long z) Velocity { get; set; }

    public (long x1, long y1, long x2, long y2) LineAsXandYPoints => (StartPoint.x, StartPoint.y, EndPoint.x, EndPoint.y);

    public HailstoneLine(string inputLine)
    {

        inputLine = inputLine.Replace(" ", "");
        var start = inputLine.Split("@").First();
        var end = inputLine.Split("@").Last();

        var startSplit = start.Split(",").Select(s => long.Parse(s)).ToList();
        Point = (startSplit[0], startSplit[1], startSplit[2]);

        var endSplit = end.Split(",").Select(s => long.Parse(s)).ToList();
        Velocity = (endSplit[0], endSplit[1], endSplit[2]);

        PopulateMaxAndMinPoints();
    }

    private void PopulateMaxAndMinPoints()
    {

        StartPoint = Point;

        // Aiming up right
        if (Velocity.x > 0 && Velocity.y > 0)
        {
            var xDist = MAX - Point.x;
            var yDist = MAX - Point.y;

            // If x will reach the edge first
            if (xDist / Velocity.x < yDist / Velocity.y)
            {
                while (xDist % Velocity.x != 0)
                {
                    xDist++;
                }
                var steps = xDist / Velocity.x;
                EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
                return;
            }
            else
            {
                while (yDist % Velocity.y != 0)
                {
                    yDist++;
                }
                var steps = yDist / Velocity.y;
                EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
                return;
            }
        }

        // Aiming down left
        if (Velocity.x < 0 && Velocity.y < 0)
        {
            var xDist = Point.x - MIN;
            var yDist = Point.y - MIN;

            // If x will reach the edge first
            if (xDist / Velocity.x < yDist / Velocity.y)
            {
                while (xDist % Velocity.x != 0)
                {
                    xDist++;
                }
                var steps = Math.Abs(xDist / Velocity.x);
                EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
                return;
            }
            else
            {
                while (yDist % Velocity.y != 0)
                {
                    yDist++;
                }
                var steps = Math.Abs(yDist / Velocity.y);
                EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
                return;
            }
        }

        // Aiming Down right
        if (Velocity.x > 0 && Velocity.y < 0)
        {
            var xDist = MAX - Point.x;
            var yDist = Point.y - MIN;

            // If x will reach the edge first
            if (xDist / Velocity.x < yDist / Velocity.y)
            {
                while (xDist % Velocity.x != 0)
                {
                    xDist++;
                }
                var steps = Math.Abs(xDist / Velocity.x);
                EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
                return;
            }
            else
            {
                while (yDist % Velocity.y != 0)
                {
                    yDist++;
                }
                var steps = Math.Abs(yDist / Velocity.y);
                EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
                return;
            }
        }

        // Aiming up Left
        if (Velocity.x < 0 && Velocity.y > 0)
        {
            var xDist = Point.x - MIN;
            var yDist = MAX - Point.y;

            // If x will reach the edge first
            if (xDist / Velocity.x < yDist / Velocity.y)
            {
                while (xDist % Velocity.x != 0)
                {
                    xDist++;
                }
                var steps = Math.Abs(xDist / Velocity.x);
                EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
                return;
            }
            else
            {
                while (yDist % Velocity.y != 0)
                {
                    yDist++;
                }
                var steps = Math.Abs(yDist / Velocity.y);
                EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
                return;
            }
        }


        // Aiming up
        if (Velocity.x == 0 && Velocity.y > 0)
        {
            var yDist = MAX - Point.y;
            while (yDist % Velocity.y != 0)
            {
                yDist++;
            }
            var steps = Math.Abs(yDist / Velocity.y);
            EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
            return;
        }

        // Aiming up down
        if (Velocity.x == 0 && Velocity.y < 0)
        {
            var yDist = Point.y - MIN;
            while (yDist % Velocity.y != 0)
            {
                yDist++;
            }
            var steps = Math.Abs(yDist / Velocity.y);
            EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
            return;
        }

        // Aiming right
        if (Velocity.x > 0 && Velocity.y == 0)
        {
            var xDist = MAX - Point.x;
            while (xDist % Velocity.x != 0)
            {
                xDist++;
            }
            var steps = Math.Abs(xDist / Velocity.x);
            EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
            return;
        }

        // Aiming left
        if (Velocity.x < 0 && Velocity.y == 0)
        {
            var xDist = Point.x - MIN;
            while (xDist % Velocity.x != 0)
            {
                xDist++;
            }
            var steps = Math.Abs(xDist / Velocity.x);
            EndPoint = (Point.x + steps * Velocity.x, Point.y + steps * Velocity.y, Point.z + steps * Velocity.z);
            return;
        }
    }

    public override string ToString() => $"Start: ({StartPoint.x}, {StartPoint.y}, {StartPoint.z}), Velocity: ({Velocity.x}, {Velocity.y}, {Velocity.z}), End: ({EndPoint.x}, {EndPoint.y}, {EndPoint.z}), ";
}