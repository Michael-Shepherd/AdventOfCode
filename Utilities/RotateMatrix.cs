using System.Diagnostics;

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

    public static List<List<char>> TurnRight(this List<List<char>> inputMatrix)
    {
        var newList = new List<List<char>>();

        for (int col = 0; col < inputMatrix[0].Count; col++)
        {
            var colToRow = new List<char>();
            for (int row = 0; row < inputMatrix.Count; row++)
            {
                colToRow.Insert(0, inputMatrix[row][col]);
            }
            newList.Add(colToRow);
        }
        return newList;
    }

    public static List<List<char>> TurnLeft(this List<List<char>> inputMatrix)
    {
        var newListLeft = new List<List<char>>();

        for (int col = 0; col < inputMatrix[0].Count; col++)
        {
            var colToRow = new List<char>();
            for (int row = 0; row < inputMatrix.Count; row++)
            {
                colToRow.Add(inputMatrix[row][col]);
            }
            newListLeft.Insert(0, colToRow);
        }

        return newListLeft;
    }
}