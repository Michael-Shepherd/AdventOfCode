namespace Utilities;

using System;
using System.IO;

public static class InputReader
{
    public static string[] Get(string inputPath)
    {
        var inputArray = File.ReadAllLines(inputPath);
        return inputArray.ToArray();
    }
}