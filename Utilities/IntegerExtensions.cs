namespace Utilities;

public static class IntegerExtensions
{
    public static int Length(this int i)
    {
        if (i < 0)
            throw new ArgumentOutOfRangeException();
        if (i == 0)
            return 1;
        return (int)Math.Floor(Math.Log10(i)) + 1;
    }
}