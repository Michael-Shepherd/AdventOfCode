using Days;
using System.Diagnostics;

public class Program
{
    static void Main()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        Console.WriteLine(DayTen.Handle());

        stopwatch.Stop();
        Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
    }
}