namespace Days;

using Utilities;
using System.ComponentModel;

public static class DayTwo
{
    public static long Handle()
    {
        return HandlePartTwo();
    }

    private static long HandlePartTwo()
    {
        var maxBallsNeeded = new Dictionary<ColoursEnum, int>()
        {
            {ColoursEnum.BLUE, 0},
            {ColoursEnum.RED, 0},
            {ColoursEnum.GREEN, 0}
        };

        var sumOfPowers = 0;
        string[] input = InputReader.Get(".\\input\\day_two_input.txt");
        foreach (var game in input)
        {
            // Console.WriteLine(game);
            var gameNumber = GetGameNumber(game);
            var gameRuns = game.Split(": ")[1].Split("; ");

            foreach (var gameRun in gameRuns)
            {
                var gameRunObject = new GameRun(gameRun, gameNumber);

                if (gameRunObject.Green > maxBallsNeeded[ColoursEnum.GREEN])
                {
                    maxBallsNeeded[ColoursEnum.GREEN] = gameRunObject.Green;
                }

                if (gameRunObject.Blue > maxBallsNeeded[ColoursEnum.BLUE])
                {
                    maxBallsNeeded[ColoursEnum.BLUE] = gameRunObject.Blue;
                }

                if (gameRunObject.Red > maxBallsNeeded[ColoursEnum.RED])
                {
                    maxBallsNeeded[ColoursEnum.RED] = gameRunObject.Red;
                }
            }

            // Console.WriteLine($"Game {gameNumber}: Blue - {maxBallsNeeded[ColoursEnum.BLUE]}, Green - {maxBallsNeeded[ColoursEnum.GREEN]}, Red - {maxBallsNeeded[ColoursEnum.RED]}");
            var power = maxBallsNeeded[ColoursEnum.BLUE] * maxBallsNeeded[ColoursEnum.GREEN] * maxBallsNeeded[ColoursEnum.RED];
            // Console.WriteLine($"Game {gameNumber}: Power - {maxBallsNeeded[ColoursEnum.BLUE] * maxBallsNeeded[ColoursEnum.GREEN] * maxBallsNeeded[ColoursEnum.RED]}");

            maxBallsNeeded = new Dictionary<ColoursEnum, int>()
            {
                {ColoursEnum.BLUE, 0},
                {ColoursEnum.RED, 0},
                {ColoursEnum.GREEN, 0}
            };

            sumOfPowers += power;
            // Console.WriteLine("****************");
        }

        return sumOfPowers;
    }


    private static long HandlePartOne()
    {
        var numbeOfBallsAvailable = new Dictionary<ColoursEnum, int>()
        {
            {ColoursEnum.BLUE, 14},
            {ColoursEnum.RED, 12},
            {ColoursEnum.GREEN, 13}
        };

        var sumOfIds = 0;
        string[] input = InputReader.Get(".\\input\\day_two_input.txt");
        foreach (var game in input)
        {
            var isPossible = true;
            // Console.WriteLine(game);
            var gameNumber = GetGameNumber(game);
            var gameRuns = game.Split(": ")[1].Split("; ");

            foreach (var gameRun in gameRuns)
            {
                var gameRunObject = new GameRun(gameRun, gameNumber);
                if (gameRunObject.Green > numbeOfBallsAvailable[ColoursEnum.GREEN])
                {
                    isPossible = false;
                    break;
                }
                if (gameRunObject.Blue > numbeOfBallsAvailable[ColoursEnum.BLUE])
                {
                    isPossible = false;
                    break;
                }
                if (gameRunObject.Red > numbeOfBallsAvailable[ColoursEnum.RED])
                {

                    isPossible = false;
                    break;
                }
                // Console.WriteLine($"Game {gameNumber}: Blue - {gameRunObject.Blue}, Green - {gameRunObject.Green}, Red - {gameRunObject.Red}");
            }

            if (isPossible)
            {
                sumOfIds += gameNumber;
            }
            else
            {
                isPossible = true;
            }
            // Console.WriteLine("****************");
        }

        return sumOfIds;
    }

    private static int GetGameNumber(string game)
    {
        int.TryParse(game.Split(": ")[0].Split(' ')[1], out int gameNumber);
        return gameNumber;
    }

}

enum ColoursEnum
{
    [Description("blue")]
    BLUE,
    [Description("red")]
    RED,
    [Description("green")]
    GREEN,
}

class GameRun
{
    public int GameId { get; set; }
    public int Green { get; set; }
    public int Red { get; set; }
    public int Blue { get; set; }

    public GameRun(string gameRun, int gameId)
    {
        this.GameId = gameId;
        // Expecting in the format: 3 blue, 4 red
        var sections = gameRun.Split(", ");
        foreach (var section in sections)
        {
            if (section.Contains("blue"))
            {
                var numberString = section.Split(" ").First();
                if (int.TryParse(numberString, out int number))
                {
                    this.Blue = number;
                }
            }

            if (section.Contains("green"))
            {
                var numberString = section.Split(" ").First();
                if (int.TryParse(numberString, out int number))
                {
                    this.Green = number;
                }
            }

            if (section.Contains("red"))
            {
                var numberString = section.Split(" ").First();
                if (int.TryParse(numberString, out int number))
                {
                    this.Red = number;
                }
            }
        }
    }
}