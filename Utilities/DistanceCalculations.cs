namespace Utilities;

public static class DistanceCalculations
{
    public static long ManhattanDistance(this (long y, long x) pointOne, (long y, long x) pointTwo)
    {
        return Math.Abs(pointOne.x - pointTwo.x) + Math.Abs(pointOne.y - pointTwo.y);
    }
}