using System.Buffers;
using System.Dynamic;
using System.Text;
using Utilities;

namespace Days;

public static class DayTwenty
{
    public static long Handle()
    {
        return HandleStepOne();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\twenty.txt");
        var moduleDict = new Dictionary<string, Module>();
        var conjunctions = new HashSet<string>();

        foreach (var line in input)
        {
            var key = line.Replace(" ", "").Split("->").First().Replace("%", "").Replace("&", "");

            var module = new Module(line);
            if (module.IsConjunction)
            {
                conjunctions.Add(key);
            }

            moduleDict.Add(key, module);
        }

        foreach (var m in moduleDict)
        {
            foreach (var d in m.Value.Destinations)
            {
                if (conjunctions.Contains(d))
                {
                    moduleDict[d].PulseInputIsHighHistory.Add(m.Key, false);
                }
            }
        }

        foreach (var m in moduleDict)
        {
            Console.WriteLine(m.Key + "\n" + m.ToString());
        }

        long lowPulses = 0;
        long highPulses = 0;
        long i = 0;
        while (true)
        {
            i++;
            if (i % 1000000 == 0)
            {
                Console.WriteLine(i);
            }
            lowPulses++;
            // (source, dest, isHigh)
            var stepsToTake = moduleDict["broadcaster"].Destinations.Select(d => ("broadcaster", d, false));
            while (stepsToTake.Any())
            {
                var newStepsToTake = new List<(string, string, bool)>();
                foreach (var pulse in stepsToTake)
                {
                    // Console.WriteLine(pulse.Item1 + "->" + pulse.Item2 + " " + (pulse.Item3 ? "High" : "Low"));
                    if (pulse.Item3 == true)
                    {
                        highPulses++;
                    }
                    else
                    {
                        lowPulses++;
                    }
                    try
                    {

                        var pulseResult = moduleDict[pulse.Item2].Pulse(pulse.Item3, pulse.Item1);
                        if (pulseResult.Item2 is not null)
                        {
                            newStepsToTake.AddRange(pulseResult.Item1.Select(p => (pulse.Item2, p, pulseResult.Item2 ?? false)));
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        if (pulse.Item3 == false)
                        {
                            Console.WriteLine("WOLOLO " + i);
                            return i;
                        }
                    }


                    // Console.WriteLine("vvv New Steps vv");
                    // foreach (var step in newStepsToTake)
                    // {
                    //     Console.WriteLine(step.Item1 + "->" + step.Item2 + " " + (step.Item3 ? "High" : "Low"));
                    // }
                }
                stepsToTake = newStepsToTake;
            }
        }

        Console.WriteLine(lowPulses + " " + highPulses);

        return lowPulses * highPulses;
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\day_seven_input.txt");
        return -1;
    }
}


class Module
{
    public bool IsOn { get; private set; } = false;

    public bool IsConjunction { get; private set; } = false;

    public Dictionary<string, bool> PulseInputIsHighHistory { get; set; } = new Dictionary<string, bool>();

    public List<string> Destinations { get; private set; } = new List<string>();

    public Module(string moduleLine)
    {
        IsConjunction = moduleLine[0] == '&';
        Destinations = moduleLine.Replace(" ", "").Split("->").Last().Split(",").ToList();
    }



    // returns List of destinations and isHighOutput
    public (List<string>, bool?) Pulse(bool isHigh, string input)
    {
        if (IsConjunction)
        {
            PulseInputIsHighHistory[input] = isHigh;

            if (PulseInputIsHighHistory.Values.Any(h => !h))
            {
                return (Destinations, true);
            }
            else
            {
                return (Destinations, false);
            }
        }

        if (isHigh)
        {
            return (new List<string>(), null);
        }
        else
        {
            if (IsOn)
            {
                IsOn = !IsOn;
                return (Destinations, false);
            }
            else
            {
                IsOn = !IsOn;
                return (Destinations, true);
            }
        }
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"IsOn: {IsOn}");
        sb.AppendLine($"IsConjunction: {IsConjunction}");

        sb.AppendLine("PulseInputIsHighHistory:");
        foreach (var kvp in PulseInputIsHighHistory)
        {
            sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
        }

        sb.AppendLine("Destinations:");
        foreach (var destination in Destinations)
        {
            sb.AppendLine($"  {destination}");
        }

        return sb.ToString();
    }
}