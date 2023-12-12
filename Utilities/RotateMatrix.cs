namespace Utilities;

public static class RotateMatrix
{
    public static List<List<int>> Rotate(List<List<int>> matrix)
    {
        var newMatrix = new int[matrix[0].Count()][];
        for (int i = 0; i < matrix[0].Count(); i++)
        {
            newMatrix[i] = new int[matrix.Count()];
        }

        Console.WriteLine(newMatrix[0].Length - matrix.Count());

        for (int i = 0; i < matrix.Count(); i++)
        {
            for (int j = 0; j < matrix[0].Count(); j++)
            {
                newMatrix[j][i] = matrix[i][j];
            }
        }

        for (int i = 0; i < matrix[0].Count(); i++)
        {

        }

        var returnList = new List<List<int>>();

        foreach (var row in newMatrix)
        {
            returnList.Add(row.ToList());
        }

        return returnList;
    }
}