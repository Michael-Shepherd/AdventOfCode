namespace Utilities;

public static class AreaCalculations
{
    public static double CalculateArea(List<(int, int)> coOrdinates)
    {
        var areaSum = 0;
        for (int i = 1; i < coOrdinates.Count; i++)
        {
            var coords1 = coOrdinates[i - 1];
            var coords2 = coOrdinates[i];
            areaSum += (coords1.Item2 * coords2.Item1 - coords2.Item2 * coords1.Item1);
        }
        areaSum = Math.Abs(areaSum);
        return areaSum * 0.5;
    }
}