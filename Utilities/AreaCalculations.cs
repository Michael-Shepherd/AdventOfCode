namespace Utilities;

public static class AreaCalculations
{
    // This is wrong
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

    public static double PolygonArea(this List<(int, int)> coOrdinates)
    {

        var n = coOrdinates.Count();
        // Initialize area
        double area = 0.0;

        // Calculate value of shoelace formula
        int j = n - 1;

        for (int i = 0; i < n; i++)
        {
            var jNode = coOrdinates[j];
            var iNode = coOrdinates[i];

            area += (jNode.Item2 + iNode.Item2) * (jNode.Item1 - iNode.Item1);

            // j is previous vertex to i
            j = i;
        }

        // Return absolute value
        return Math.Abs(area / 2.0);
    }
}