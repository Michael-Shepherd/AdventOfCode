using Utilities;

namespace Days;

public static class DayNineteen
{
    public static long Handle()
    {
        return HandleStepOne();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\nineteen.txt");
        var parts = new List<Part>();
        var instructions = new Dictionary<string, List<Instruction>>();
        var accepted = new List<Part>();
        var startKey = "in";


        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            else if (line.StartsWith('{'))
            {
                // Add to parts
                parts.Add(new Part(line));
            }
            else
            {
                // Parse instructions
                var key = line.Split('{').First();
                var lineInstructions = line.Remove(line.Length - 1).Split('{').Last().Split(',');
                var newInstructions = lineInstructions.Select(l => new Instruction(l)).ToList();
                instructions.Add(key, newInstructions);
            }
        }

        var startInstruction = instructions[startKey];
        foreach (var part in parts)
        {
            var currentInstruction = startInstruction;
            var stillGoing = true;
            while (stillGoing)
            {
                foreach (var instruction in currentInstruction)
                {
                    if (instruction.IsOnlyDestination)
                    {
                        if (instruction.Destination == "R" || instruction.Destination == "A")
                        {
                            if (instruction.Destination == "A")
                            {
                                accepted.Add(part);
                            }
                            stillGoing = false;
                            break;
                        }
                        else
                        {
                            currentInstruction = instructions[instruction.Destination];
                            break;
                        }
                    }
                    else if (instruction.GreaterThan)
                    {
                        if (part.GetValue(instruction.PartRating ?? 'x') > instruction.Number)
                        {
                            if (instruction.Destination == "R" || instruction.Destination == "A")
                            {
                                if (instruction.Destination == "A")
                                {
                                    accepted.Add(part);
                                }
                                stillGoing = false;
                                break;
                            }
                            else
                            {
                                currentInstruction = instructions[instruction.Destination];
                                break;
                            }
                        }
                    }
                    else if (instruction.LessThan)
                    {
                        if (part.GetValue(instruction.PartRating ?? 'x') < instruction.Number)
                        {
                            if (instruction.Destination == "R" || instruction.Destination == "A")
                            {
                                if (instruction.Destination == "A")
                                {
                                    accepted.Add(part);
                                }
                                stillGoing = false;
                                break;
                            }
                            else
                            {
                                currentInstruction = instructions[instruction.Destination];
                                break;
                            }
                        }
                    }
                }
            }
        }

        Console.WriteLine(string.Join("\n", accepted));
        return accepted.Select(a => a.Value).Sum();
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\day_seven_input.txt");
        return -1;
    }
}

class Instruction
{
    // X, M, A, S
    public char? PartRating { get; set; }
    public bool GreaterThan { get; set; } = false;
    public bool LessThan { get; set; } = false;
    public int? Number { get; set; }
    public bool IsOnlyDestination { get; set; } = false;
    public string Destination { get; set; }


    // TAKES In x>0:abc OR x<23:R OR A
    public Instruction(string inputPart)
    {
        var split = inputPart.Split(":");
        if (split.Length == 1)
        {
            Destination = inputPart;
            IsOnlyDestination = true;
            return;
        }

        Destination = split.Last();
        var greaterThanSplit = split.First().Split(">");
        if (greaterThanSplit.Length == 2)
        {
            GreaterThan = true;
            Number = int.Parse(greaterThanSplit.Last());
            PartRating = greaterThanSplit.First().First();
        }

        var lessThanSplit = split.First().Split("<");
        if (lessThanSplit.Length == 2)
        {
            LessThan = true;
            Number = int.Parse(lessThanSplit.Last());
            PartRating = lessThanSplit.First().First();
        }
    }

    public override string ToString()
    {
        if (IsOnlyDestination)
        {
            return "Destintion: " + Destination;
        }
        else
        {
            var op = GreaterThan ? ">" : LessThan ? "<" : "?";
            return $"{PartRating} {op} {Number} => {Destination}";
        }
    }
}

class Part
{
    public int X { get; set; }
    public int M { get; set; }
    public int A { get; set; }
    public int S { get; set; }

    public int GetValue(char key)
    {
        switch (key)
        {
            case 'x':
                return X;
            case 'm':
                return M;
            case 'a':
                return A;
            case 's':
                return S;
            default:
                throw new Exception("AHHHHHHH");
        }
    }

    public int Value => X + M + A + S;

    // TAKES IN {x=787,m=2655,a=1222,s=2876}
    public Part(string inputLine)
    {
        inputLine = inputLine.Remove(0, 1);
        inputLine = inputLine.Remove(inputLine.Length - 1, 1);
        var splits = inputLine.Split(",");

        if (splits.Length != 4)
        {
            throw new Exception("AHHHHHH");
        }

        X = int.Parse(splits[0].Split("=").Last());
        M = int.Parse(splits[1].Split("=").Last());
        A = int.Parse(splits[2].Split("=").Last());
        S = int.Parse(splits[3].Split("=").Last());
    }

    public override string ToString()
    {
        return $"x {X} m {M} a {A} s {S}";
    }

}