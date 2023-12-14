namespace Utilities;

public static class ListExtensions
{
    public static string Flatten(this List<List<char>> list)
    {
        return new string(list.SelectMany(l => l).ToArray());
    }
}