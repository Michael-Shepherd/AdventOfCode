using Utilities;

namespace Days;

public static class DaySeventeen
{
    public static long Handle()
    {
        return HandleStepOne();
    }

    public static long HandleStepOne()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        var inputMatrix = InputReader.Get(".\\input\\seventeen.txt");
        // (row, col), distance
        var unvisitedSet = new Dictionary<(int, int), NoReverseDNode>();
        for (int row = 0; row < inputMatrix.Length; row++)
        {
            for (int col = 0; col < inputMatrix[row].Length; col++)
            {
                if (row == 0 && col == 0)
                {
                    unvisitedSet.Add((row, col), new NoReverseDNode() { CoOrds = (row, col), distance = 0, Value = (int)char.GetNumericValue(inputMatrix[row][col]), LastThreeDirections = new List<PolarDirection> { } });
                }
                else
                {
                    unvisitedSet.Add((row, col), new NoReverseDNode() { CoOrds = (row, col), distance = int.MaxValue, Value = (int)char.GetNumericValue(inputMatrix[row][col]) });
                }
            }
        }

        return SearchForLightestRouteDjikstra(unvisitedSet, (inputMatrix.Length - 1, inputMatrix[0].Length - 1));
    }

    private static void visualiseWeights(Dictionary<(int, int), NoReverseDNode> inputMatrix, List<DNode> touched)
    {
        Console.WriteLine("***********************");
        Console.WriteLine(string.Join("\n", touched));
        for (int row = 0; row < 13; row++)
        {
            for (int col = 0; col < 13; col++)
            {
                foreach (var t in touched)
                {
                    if (t.CoOrds == (row, col))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        break;
                    }
                }
                Console.Write(inputMatrix[(row, col)].Value + " ");

                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine();
        }
        Console.WriteLine("***********************");
    }

    private static long SearchForLightestRouteDjikstra(Dictionary<(int, int),
                                                        NoReverseDNode> unvisitedNodes,
                                                        (int, int) destination)
    {
        var keepItSecret = new Dictionary<(int, int), NoReverseDNode>();
        foreach (var d in unvisitedNodes)
        {
            keepItSecret.Add(d.Key, d.Value);
        }

        var visitedNodes = new Dictionary<(int, int), NoReverseDNode>();
        var currentNode = unvisitedNodes[(0, 0)];
        var isComplete = false;
        var unvisitedNodesAsAList = unvisitedNodes.Select(u => u.Value).ToList();
        var destinationNode = unvisitedNodes[destination];

        while (unvisitedNodes.Any())
        {
            var unvisitedNeighbours = currentNode.GetUnvistedNeighbours(unvisitedNodes);
            foreach (var neighbour in unvisitedNeighbours)
            {
                var existingNeighbour = unvisitedNodes[neighbour.CoOrds];

                if (existingNeighbour.distance < neighbour.distance)
                {
                    unvisitedNodes[neighbour.CoOrds] = neighbour;
                }
            }

            visitedNodes.Add(currentNode.CoOrds, currentNode);
            unvisitedNodes.Remove(currentNode.CoOrds);
            // Visited the destination, break
            if (visitedNodes.TryGetValue(destination, out destinationNode))
            {
                // Console.WriteLine("Touched Destination");
                // foreach (var node in destinationNode.AllParents)
                // {
                //     Console.WriteLine(node.CoOrds + " " + node.distance + " " + node.Value);
                // }

                Console.WriteLine(string.Join("\n", unvisitedNodesAsAList));
                break;
            }

            if (unvisitedNodes.Any())
            {
                // Could optimise this for smallest tentative distance if needed
                unvisitedNodesAsAList = unvisitedNodes.Select(u => u.Value).ToList();
                unvisitedNodesAsAList.Sort((p, q) => p.distance.CompareTo(q.distance));
                currentNode = unvisitedNodesAsAList.First();
            }


            // Smallest Tentative Distance is infinity, break
            if (currentNode.distance == int.MaxValue)
            {
                Console.WriteLine("All infinity");
                break;
            }
        }

        visualiseWeights(keepItSecret, destinationNode.AllParents);


        return destinationNode?.distance ?? -1;
    }

    private static List<NoReverseDNode> GetUnvistedNeighbours(this NoReverseDNode currentNode, Dictionary<(int, int), NoReverseDNode> unvisitedNodes)
    {
        var validDirections = new List<PolarDirection>() {
            PolarDirection.NORTH,
            PolarDirection.SOUTH,
            PolarDirection.EAST,
            PolarDirection.WEST
        };

        if (currentNode.LastThreeDirections.Count == 0)
        {
            // Can go anywhere, do nothing
        }
        else if (currentNode.LastThreeDirections.Count < 3)
        {
            // Can go anywhere but to the previous. Remove the inverse
            validDirections.Remove(currentNode.LastThreeDirections.Last().GetInverse());
        }
        else if (currentNode.LastThreeDirections.Count == 3)
        {
            validDirections.Remove(currentNode.LastThreeDirections.Last().GetInverse());
            // If all three are the same, remove that direction
            if (currentNode.LastThreeDirections.Distinct().Count() == 1)
            {
                validDirections.Remove(currentNode.LastThreeDirections.Last());
            }
        }
        else
        {
            Console.WriteLine("Dafaq");
        }

        Console.WriteLine();
        Console.WriteLine(string.Join(" ", currentNode.LastThreeDirections));
        Console.WriteLine(string.Join(" ", validDirections));
        Console.WriteLine();

        var unvisitedValidNeighbours = new List<NoReverseDNode>();
        foreach (var direction in validDirections)
        {

            var newCoOrds = currentNode.CoOrds;
            if (direction == PolarDirection.NORTH)
            {
                newCoOrds = (currentNode.CoOrds.Item1 - 1, currentNode.CoOrds.Item2);
            }
            else if (direction == PolarDirection.SOUTH)
            {
                newCoOrds = (currentNode.CoOrds.Item1 + 1, currentNode.CoOrds.Item2);

            }
            else if (direction == PolarDirection.EAST)
            {
                newCoOrds = (currentNode.CoOrds.Item1, currentNode.CoOrds.Item2 - 1);

            }
            else if (direction == PolarDirection.WEST)
            {
                newCoOrds = (currentNode.CoOrds.Item1, currentNode.CoOrds.Item2 + 1);
            }

            if (unvisitedNodes.TryGetValue(newCoOrds, out var neighbour))
            {
                var newDirections = currentNode.LastThreeDirections.Select(s => s).ToList();

                if (newDirections.Count == 3)
                {
                    newDirections.RemoveAt(0);
                }
                newDirections.Add(direction);

                neighbour.LastThreeDirections = newDirections;
                neighbour.Previous = currentNode;
                neighbour.AllParents = currentNode.AllParents.Select(s => s).ToList();
                neighbour.AllParents.Add(currentNode);
                neighbour.distance = currentNode.distance + neighbour.Value;
                unvisitedValidNeighbours.Add(neighbour);
            }
        }

        return unvisitedValidNeighbours;
    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\day_seven_input.txt");
        return -1;
    }
}

class NoReverseDNode : DNode
{
    public DNode? Previous { get; set; }
    public List<DNode> AllParents { get; set; } = new List<DNode>();
    public List<PolarDirection> LastThreeDirections { get; set; } = new List<PolarDirection>();

    public override string ToString()
    {
        return $"COords: {CoOrds} Distance: {distance}  {string.Join(" ", LastThreeDirections)}";
    }
}

class DNode
{
    public (int, int) CoOrds { get; set; }
    public int Value { get; set; }
    public int distance { get; set; }
}