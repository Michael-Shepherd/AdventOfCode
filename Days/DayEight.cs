using Utilities;

namespace Days;

public class DayEight
{
    public static long Handle()
    {
        return HandleStepTwoDifferently();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\eight.txt");

        var start = "AAA";
        var end = "ZZZ";

        var directions = input.First();

        Console.WriteLine(directions);

        var nodeDict = new Dictionary<string, Node>();
        for (int i = 2; i < input.Length; i++)
        {
            var line = input[i];
            var node = new Node(line);
            nodeDict.Add(node.Index, node);
        }

        var currentNodeIndex = start;
        var directionIndex = 0;
        var lastDirectionIndex = directions.Length;
        var count = 0;
        while (currentNodeIndex != end)
        {
            // Console.Write(currentNodeIndex + " -> ");
            currentNodeIndex = nodeDict[currentNodeIndex].GetNodeByDirection(directions[directionIndex]);
            // Console.WriteLine(currentNodeIndex);
            count++;
            directionIndex++;
            if (directionIndex == lastDirectionIndex)
            {
                directionIndex = 0;
            }
        }

        return count;
    }

    public static long HandleStepTwoDifferently()
    {
        var input = InputReader.Get(".\\input\\eight.txt");

        var startLineEnd = "A";
        var endLineEnd = "Z";

        var directions = input.First();

        Console.WriteLine(directions);

        var nodeDict = new Dictionary<string, Node>();
        var startNodes = new HashSet<string>() { };
        var endNodes = new HashSet<string>() { };

        for (int i = 2; i < input.Length; i++)
        {
            var line = input[i];
            var node = new Node(line);
            nodeDict.Add(node.Index, node);
            if (node.Index.EndsWith(startLineEnd))
            {
                startNodes.Add(node.Index);
            }
            else if (node.Index.EndsWith(endLineEnd))
            {
                endNodes.Add(node.Index);
            }
        }

        foreach (var node in startNodes)
        {
            Console.Write(node + " ");
        }
        Console.WriteLine("");

        foreach (var node in endNodes)
        {
            Console.Write(node + " ");
        }
        Console.WriteLine("");


        // Index, start, length
        var loopSizes = new List<long>();

        Parallel.ForEach(startNodes, node =>
        {
            var internalCount = 0;
            var indicesOfEndPoints = new List<long>();
            var currentNodeIndex = node;
            var directionIndex = 0;
            var lastDirectionIndex = directions.Length;
            while (true)
            {
                currentNodeIndex = nodeDict[currentNodeIndex].GetNodeByDirection(directions[directionIndex]);

                if (currentNodeIndex.EndsWith("Z"))
                {
                    indicesOfEndPoints.Add(internalCount);
                    (var isLoop, var loopSize) = CheckLoop(indicesOfEndPoints);
                    if (isLoop)
                    {
                        Console.WriteLine($"Index {node} Current Count {internalCount} Count of Start {indicesOfEndPoints[indicesOfEndPoints.Count() - 3]} Loop size {loopSize}");
                        lock (loopSizes)
                        {
                            loopSizes.Add(loopSize);
                        }
                        break;
                    }
                }
                internalCount++;
                directionIndex++;
                if (directionIndex == lastDirectionIndex)
                {
                    directionIndex = 0;
                }
            }
        });

        return LowestCommonMultiple.CalculateLCM(loopSizes);
    }

    private static (bool, long) CheckLoop(List<long> indices)
    {
        if (indices.Count() > 2)
        {
            var last = indices.Last();
            var secondLast = indices[indices.Count() - 2];
            var thirdLast = indices[indices.Count() - 3];
            var firstDiff = last - secondLast;
            if (last - secondLast == secondLast - thirdLast)
            {
                return (true, firstDiff);
            }
        }
        return (false, -1);
    }
}

class Node
{
    public string Index { get; set; }
    public string Left { get; set; }
    public string Right { get; set; }


    public Node(string inputLine)
    {
        this.Index = inputLine.Split(" = ").First();
        var directions = inputLine.Replace("(", "").Replace(")", "").Split(" = ").Last();
        this.Left = directions.Split(", ").First();
        this.Right = directions.Split(", ").Last();
    }

    public bool IsRepeating()
    {
        return false;
    }

    public string GetNodeByDirection(char lastDirectionIndex)
    {
        var result = "";
        if (lastDirectionIndex == 'R')
        {
            result = this.Right;
        }
        else if (lastDirectionIndex == 'L')
        {
            result = this.Left;
        }
        else
        {
            throw new Exception("AHHHHHHHH");
        }

        return result;
    }
}