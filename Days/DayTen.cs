using System.Linq.Expressions;
using Microsoft.VisualBasic;
using Utilities;

namespace Days;

public static class DayTen
{
    public static long Handle()
    {
        return HandleStepTwo();
    }

    public static long HandleStepOne()
    {
        var inputRead = InputReader.Get(".\\input\\ten.txt");
        var input = new List<List<string>>();

        foreach (var line in inputRead)
        {
            var list = line.ToList().Select(c => c + "");
            input.Add(list.ToList());
        }

        // row, col, dist from start
        var startPoint = "S";
        var startCoordinates = new CoOrd(-1, -1);

        for (int i = 0; i < input.Count(); i++)
        {
            for (int j = 0; j < input[0].Count(); j++)
            {
                if (input[i][j] == startPoint)
                {
                    startCoordinates = new CoOrd(i, j);
                }
            }
        }
        var distanceTable = new Dictionary<CoOrd, long>() { { startCoordinates, 0 } };

        var listOfWorkingCoords = new List<CoOrd>() { startCoordinates };
        var parents = new List<CoOrd>() { startCoordinates };
        var distanceIndex = 0;
        while (listOfWorkingCoords.Count() > 0)
        {
            distanceIndex++;
            var newListOfWorkingCoords = new List<CoOrd>();
            foreach (var workingCoord in listOfWorkingCoords)
            {
                var touchingCoords = findTouchingThatArenParents(input, parents, workingCoord);
                foreach (var touching in touchingCoords)
                {
                    distanceTable.Add(touching, distanceIndex);
                    newListOfWorkingCoords.Add(touching);
                }
                parents.AddRange(newListOfWorkingCoords.ToList());
            }


            listOfWorkingCoords = newListOfWorkingCoords.ToList();
        }

        long maxValue = 0;
        for (int i = 0; i < input.Count(); i++)
        {
            for (int j = 0; j < input[0].Count(); j++)
            {
                var value = ".";
                foreach (var distance in distanceTable)
                {
                    if (distance.Key.row == i && distance.Key.col == j)
                    {
                        if (distance.Value > maxValue)
                        {
                            maxValue = distance.Value;
                        }
                        value = distance.Value + "";
                        break;
                    }
                }
                Console.Write(value + " ");

            }
            Console.WriteLine();
        }

        return distanceIndex - 1;
    }


    private static List<CoOrd> findTouchingThatArenParents(List<List<string>> input, List<CoOrd> parentCoords, CoOrd coOrd)
    {
        var currentTile = input[coOrd.row][coOrd.col];

        string upDown = "|";
        string leftRight = "-";
        string bottomRight = "L";
        string bottomLeft = "J";
        string topRight = "F";
        string topLeft = "7";
        string ground = ".";

        var minRow = 0;
        var maxRow = input.Count() - 1;
        var minCol = 0;
        var maxCol = input[0].Count() - 1;

        var up = new CoOrd(coOrd.row - 1, coOrd.col);
        var down = new CoOrd(coOrd.row + 1, coOrd.col);
        var left = new CoOrd(coOrd.row, coOrd.col - 1);
        var right = new CoOrd(coOrd.row, coOrd.col + 1);

        var tilesToCheck = new List<CoOrd>();
        if (currentTile == upDown)
        {
            tilesToCheck = new List<CoOrd>() { up, down };
        }
        else if (currentTile == leftRight)
        {
            tilesToCheck = new List<CoOrd>() { left, right };
        }
        else if (currentTile == bottomRight)
        {
            tilesToCheck = new List<CoOrd>() { up, right };

        }
        else if (currentTile == bottomLeft)
        {
            tilesToCheck = new List<CoOrd>() { up, left };

        }
        else if (currentTile == topRight)
        {
            tilesToCheck = new List<CoOrd>() { down, right };

        }
        else if (currentTile == topLeft)
        {
            tilesToCheck = new List<CoOrd>() { down, left };
        }
        else if (currentTile == ground)
        {
            return new List<CoOrd>();
        }
        else
        {
            tilesToCheck = new List<CoOrd>() { up, down, left, right };
        }

        var touchingTiles = new List<CoOrd>();
        foreach (var tileCoord in tilesToCheck)
        {
            if (coOrd.row > maxRow
            || coOrd.row < minRow
            || coOrd.col < minCol
            || coOrd.col > maxCol)
            {
                continue;
            }

            string tile;
            try
            {
                tile = input[tileCoord.row][tileCoord.col];
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                // Skip over out of range tiles
                continue;
            }

            if (parentCoords.Contains(tileCoord))
            {
                continue;
            }
            if (tile == ground) continue;

            var validChecks = new List<CoOrd>() { up, down, left, right };

            if (tile == upDown)
            {
                validChecks = new List<CoOrd>() { up, down };
            }
            else if (tile == leftRight)
            {
                validChecks = new List<CoOrd>() { left, right };
            }
            else if (tile == bottomRight)
            {
                validChecks = new List<CoOrd>() { down, left };

            }
            else if (tile == bottomLeft)
            {
                validChecks = new List<CoOrd>() { down, right };

            }
            else if (tile == topRight)
            {
                validChecks = new List<CoOrd>() { up, left };

            }
            else if (tile == topLeft)
            {
                validChecks = new List<CoOrd>() { up, right };
            }


            if (validChecks.Contains(tileCoord))
            {
                touchingTiles.Add(tileCoord);

            }
        }

        return touchingTiles;
    }

    private class CoOrd
    {
        public int row { get; set; }
        public int col { get; set; }

        public CoOrd(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public override bool Equals(Object? obj)
        {
            var obje = obj as CoOrd;
            if (obje == null)
            {
                return false;
            }
            return this.row.Equals(obje.row) && this.col.Equals(obje.col);
        }

        public override string ToString()
        {
            return $"row: {this.row} col: {this.col}";
        }
    }

    public static long HandleStepTwo()
    {
        var inputRead = InputReader.Get(".\\input\\ten.txt");
        var input = new List<List<string>>();

        foreach (var line in inputRead)
        {
            var list = line.ToList().Select(c => c + "");
            input.Add(list.ToList());
        }

        // row, col, dist from start
        var startPoint = "S";
        var startCoordinates = new CoOrd(-1, -1);

        for (int i = 0; i < input.Count(); i++)
        {
            for (int j = 0; j < input[0].Count(); j++)
            {
                if (input[i][j] == startPoint)
                {
                    startCoordinates = new CoOrd(i, j);
                }
            }
        }
        var distanceTable = new Dictionary<CoOrd, long>() { { startCoordinates, 0 } };

        var listOfWorkingCoords = new List<CoOrd>() { startCoordinates };
        var parents = new List<CoOrd>() { startCoordinates };
        var distanceIndex = 0;
        while (listOfWorkingCoords.Count() > 0)
        {
            distanceIndex++;
            var newListOfWorkingCoords = new List<CoOrd>();
            foreach (var workingCoord in listOfWorkingCoords)
            {
                var touchingCoords = findTouchingThatArenParents(input, parents, workingCoord);
                foreach (var touching in touchingCoords)
                {
                    distanceTable.Add(touching, distanceIndex);
                    newListOfWorkingCoords.Add(touching);
                }
                parents.AddRange(newListOfWorkingCoords.ToList());
            }


            listOfWorkingCoords = newListOfWorkingCoords.ToList();
        }

        var laidPipe = new List<List<string>>();
        for (int i = 0; i < input.Count(); i++)
        {
            var row = new List<string>();
            for (int j = 0; j < input[0].Count(); j++)
            {
                var value = ".";
                foreach (var distance in distanceTable)
                {
                    if (distance.Key.row == i && distance.Key.col == j)
                    {
                        value = distance.Value + "";
                        break;
                    }
                }
                row.Add(value == "." ? "." : input[i][j]);
            }
            laidPipe.Add(row);
        }

        foreach (var line in laidPipe)
        {
            Console.WriteLine(string.Join(" ", line));
        }

        var enclosedCoords = new List<CoOrd>();
        var openCoords = new List<CoOrd>();
        var listOfChecked = new List<CoOrd>();
        for (int i = 0; i < laidPipe.Count(); i++)
        {
            for (int j = 0; j < laidPipe[0].Count(); j++)
            {
                var currentCoord = new CoOrd(i, j);
                if (enclosedCoords.Contains(currentCoord) || openCoords.Contains(currentCoord))
                {
                    continue;
                }

                if (laidPipe[i][j] == "X")
                {
                    continue;
                }

                if (listOfChecked.Contains(currentCoord))
                {
                    continue;
                }

                else
                {
                    (var hasPathToWall, var group, listOfChecked) = HasPathToWall(laidPipe, currentCoord, listOfChecked);
                    // Console.WriteLine($"{currentCoord} has path: {hasPathToWall}");
                    // Console.WriteLine(string.Join(" ", group));
                    // Console.WriteLine(string.Join("*", listOfChecked));
                    foreach (var c in group)
                    {
                        if (hasPathToWall)
                        {
                            openCoords.Add(c);
                        }
                        else
                        {
                            enclosedCoords.Add(c);
                        }
                    }
                    foreach (var c in enclosedCoords)
                    {
                        laidPipe[c.row][c.col] = "I";
                    }

                    foreach (var c in openCoords)
                    {
                        laidPipe[c.row][c.col] = ".";
                    }

                    // Console.WriteLine();

                    // foreach (var line in laidPipe)
                    // {
                    //     Console.WriteLine(string.Join(" ", line));
                    // }
                }
            }
        }


        Console.WriteLine("\n\nFinal\n");
        foreach (var line in laidPipe)
        {
            foreach (var letter in line)
            {
                if (letter == "X")
                {
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                else if (letter == ".")
                {
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                else if (letter == "I")
                {
                    // Console.BackgroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.Write(letter + " ");
            }
            Console.WriteLine();
        }

        // Console.WriteLine(string.Join("\n", enclosedCoords));

        return enclosedCoords.Select(x => x.ToString()).Distinct().Count();
    }

    private static (bool, List<CoOrd>, List<CoOrd>) HasPathToWall(List<List<string>> input, CoOrd coOrd, List<CoOrd> listOfParents)
    {
        var minRow = 0;
        var maxRow = input.Count() - 1;
        var minCol = 0;
        var maxCol = input[0].Count() - 1;

        var listOfCoordsToCheck = new List<CoOrd>() { coOrd };
        var listOfCoordsChecked = new List<CoOrd>();
        var groupList = new List<CoOrd>();
        bool hasPathToWall = false;

        // Console.WriteLine("Sample Size: " + Math.Pow(input.Count(), 2));

        while (listOfCoordsToCheck.Count() > 0)
        {
            // Console.WriteLine("Checked: " + listOfCoordsChecked.Count());
            // Console.WriteLine("Parents: " + listOfParents.Count());
            var newListOfCoords = new List<CoOrd>();

            foreach (var c in listOfCoordsToCheck)
            {
                if (listOfParents.Contains(c))
                {
                    continue;
                }
                // Console.WriteLine("Coord to check:");
                // Console.WriteLine(c);
                if (input[c.row][c.col] != ".")
                {
                    listOfParents.Add(c);
                    continue;
                }
                else
                {
                    listOfCoordsChecked.Add(c);
                    listOfParents.Add(c);
                    groupList.Add(c);
                }
                // Console.WriteLine(string.Join(" | ", listOfParents));

                if (c.row == minRow || c.row == maxRow || c.col == minCol || c.col == maxCol)
                {
                    hasPathToWall = true;
                }

                var up = new CoOrd(c.row - 1, c.col);
                var down = new CoOrd(c.row + 1, c.col);
                var left = new CoOrd(c.row, c.col - 1);
                var right = new CoOrd(c.row, c.col + 1);
                var validChecks = new List<CoOrd>() { up, down, left, right };

                // foreach (var check in validChecks.Where(v => !listOfParents.Contains(v)))
                foreach (var check in validChecks)
                {
                    if (listOfParents.Contains(check))
                    {
                        // Console.WriteLine(check);
                        // Console.WriteLine(string.Join("|", listOfParents));
                        continue;
                    }
                    string tile;
                    try
                    {
                        tile = input[check.row][check.col];
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        // Skip over out of range tiles
                        hasPathToWall = true;
                        continue;
                    }

                    if (tile == ".")
                    {
                        newListOfCoords.Add(check);
                    }
                }
            }
            listOfCoordsToCheck = newListOfCoords.Where(nc => !listOfParents.Contains(nc)).ToList();
        }



        return (hasPathToWall, groupList, listOfParents);
    }

    private bool IsValidRunningAlongPipeOrIntoEmptySpace(List<List<string>> input, CoOrd current, CoOrd next)
    {
        string upDown = "|";
        string leftRight = "-";
        string bottomRight = "L";
        string bottomLeft = "J";
        string topRight = "F";
        string topLeft = "7";
        string ground = ".";
        string start = "S";

        var up = new CoOrd(current.row - 1, current.col);
        var down = new CoOrd(current.row + 1, current.col);
        var left = new CoOrd(current.row, current.col - 1);
        var right = new CoOrd(current.row, current.col + 1);

        var isUp = next == up;
        var isDown = next == up;
        var isLeft = next == up;
        var isRight = next == up;

        var currentTile = input[current.row][current.col];
        var nextTile = ground;

        try
        {
            nextTile = input[current.row][current.col];
        }
        catch (ArgumentNullException e)
        {
            // false if next tile is out of bounds
            return false;
        }


        if (currentTile == ground)
        {
            if (nextTile == ground)
            {
                return true;
            }
            else if (new List<string>() { bottomRight, bottomLeft, topRight, topLeft, start }.Contains(nextTile))
            {
                return true;
            }

            return false;
        }

        if (currentTile == upDown)
        {
            if (nextTile == ground || nextTile == leftRight)
            {
                return false;
            }

            return true;
        }

        if (currentTile == leftRight)
        {
            if (nextTile == ground || nextTile == upDown)
            {
                return false;
            }

            return true;
        }

        if (currentTile == bottomRight)
        {
            if (isUp)
            {
                if (nextTile == upDown || nextTile == topRight || nextTile == topLeft || nextTile == bottomRight)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (isDown)
            {
                if (nextTile == leftRight || nextTile == upDown || nextTile == bottomLeft || nextTile == bottomRight)
                {
                    return false;
                }
                return true;
            }
            else if (isLeft)
            {
                if (nextTile == leftRight || nextTile == upDown || nextTile == bottomRight || nextTile == topRight)
                {
                    return false;
                }
                return true;
            }
            else if (isRight)
            {
                if (nextTile == upDown || nextTile == bottomRight || nextTile == ground)
                {
                    return false;
                }
                return true;
            }
        }

        if (currentTile == bottomLeft)
        {
            if (isUp)
            {
                if (nextTile == upDown || nextTile == topRight || nextTile == topLeft)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (isDown)
            {
                if (nextTile == leftRight || nextTile == upDown || nextTile == bottomLeft || nextTile == bottomRight)
                {
                    return false;
                }
                return true;
            }
            else if (isLeft)
            {
                if (nextTile == upDown || nextTile == ground || nextTile == bottomLeft)
                {
                    return false;
                }
                return true;
            }
            else if (isRight)
            {
                if (nextTile == leftRight || nextTile == upDown || nextTile == bottomLeft || nextTile == topLeft)
                {
                    return false;
                }
                return true;
            }
        }

        if (currentTile == topRight)
        {
            if (isUp)
            {
                if (nextTile == topRight || nextTile == upDown || nextTile == topLeft || nextTile == leftRight)
                {
                    return false;
                }
                return true;
            }
            else if (isDown)
            {
                if (nextTile == leftRight || nextTile == topRight || nextTile == topLeft || nextTile == ground)
                {
                    return false;
                }
                return true;
            }
            else if (isLeft)
            {
                if (nextTile == upDown || nextTile == leftRight || nextTile == topRight || nextTile == bottomRight)
                {
                    return false;
                }
                return true;
            }
            else if (isRight)
            {
                if (nextTile == upDown || nextTile == ground || nextTile == topRight || nextTile == bottomRight)
                {
                    return false;
                }
                return true;
            }
        }
    }

}