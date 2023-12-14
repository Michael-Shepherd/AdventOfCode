namespace Utilities;
public static class EnumerableExtensions
{
    // Source: http://stackoverflow.com/questions/774457/combination-generator-in-linq#12012418
    private static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource item)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        yield return item;

        foreach (var element in source)
            yield return element;
    }

    public static IEnumerable<IEnumerable<TSource>> Permutations<TSource>(this IEnumerable<TSource> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        var list = source.ToList();

        if (list.Count > 1)
            return from s in list
                   from p in Permutations(list.Take(list.IndexOf(s)).Concat(list.Skip(list.IndexOf(s) + 1)))
                   select p.Prepend(s);

        return new[] { list };
    }

    public static List<string> GetDistinctPermutations(this string input)
    {
        List<string> result = new List<string>();
        GetDistinctPermutationsHelper(input.ToCharArray(), 0, result);
        return result;
    }

    static void GetDistinctPermutationsHelper(char[] arr, int index, List<string> result)
    {
        if (index == arr.Length - 1)
        {
            result.Add(new string(arr));
            return;
        }

        HashSet<char> swapped = new HashSet<char>();

        for (int i = index; i < arr.Length; i++)
        {
            if (!swapped.Contains(arr[i]))
            {
                swapped.Add(arr[i]);

                Swap(arr, index, i);
                GetDistinctPermutationsHelper(arr, index + 1, result);
                Swap(arr, index, i); // Backtrack
            }
        }
    }

    static void Swap(char[] arr, int i, int j)
    {
        char temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}