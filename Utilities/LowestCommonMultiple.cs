namespace Utilities;

public static class LowestCommonMultiple
{
    public static long CalculateLCM(List<long> numbers)
    {
        if (numbers.Count == 0)
        {
            throw new ArgumentException("List cannot be empty", nameof(numbers));
        }

        long result = numbers[0];

        for (int i = 1; i < numbers.Count; i++)
        {
            result = CalculateLCM(result, numbers[i]);
        }

        return result;
    }

    private static long CalculateLCM(long a, long b)
    {
        return (a * b) / CalculateGCD(a, b);
    }

    private static long CalculateGCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
}