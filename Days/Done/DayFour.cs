namespace Days;

using Utilities;

public static class DayFour
{
    public static long Handle()
    {
        return HandlePartTwo();
    }

    private static long HandlePartTwo()
    {
        string[] input = InputReader.Get(".\\input\\day_four_input.txt");
        var originalCards = input.Select(card =>
        {
            var cardNumberText = card.Split(": ")[0].Split(" ").Last();
            int.TryParse(cardNumberText, out int cardNumber);
            var numbers = card.Split(": ")[1];
            var winners = numbers.Split(" | ")[0].Replace("  ", " ").Split(" ");
            var winnersNumbers = new List<int>();
            foreach (var numberTest in winners)
            {
                if (int.TryParse(numberTest, out int number))
                {
                    winnersNumbers.Add(number);
                }
            }

            var scratched = numbers.Split(" | ")[1].Replace("  ", " ").Split(" ");
            var scratchedNumbers = new List<int>();
            foreach (var numberTest in scratched)
            {
                if (int.TryParse(numberTest, out int number))
                {
                    scratchedNumbers.Add(number);
                }
            }

            return new CardEntry
            {
                Number = cardNumber,
                WinningNumbers = winnersNumbers,
                ScratchedNumbers = scratchedNumbers,
                OverlapCount = winnersNumbers.Intersect(scratchedNumbers).Count()
            };
        });

        var orginalCardsDictionary = new Dictionary<int, CardEntry>() { };
        foreach (var card in originalCards)
        {
            orginalCardsDictionary.Add(card.Number, card);
        }

        var currentSet = originalCards;
        var finalCardsCount = 0;
        while (currentSet.Count() > 0)
        {
            finalCardsCount += currentSet.Count();
            var newSet = new List<CardEntry>();
            Console.WriteLine("Iterationnnnnnn");
            foreach (var card in currentSet)
            {
                if (card.OverlapCount > 0)
                {
                    var startIndex = card.Number + 1;
                    var endIndex = card.Number + card.OverlapCount;
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        newSet.Add(orginalCardsDictionary[i]);
                    }
                    // var wonCards = originalCards.Where(c => c.Number >= startIndex && c.Number <= endIndex);
                    // newSet.AddRange(wonCards);
                }
            }
            currentSet = newSet;
        }

        return finalCardsCount;
    }

    private static long HandlePartOne()
    {
        string[] input = InputReader.Get(".\\input\\day_four_input.txt");

        var sumOfOverlapPoints = 0;
        foreach (var card in input)
        {
            var numbers = card.Split(": ")[1];
            var winners = numbers.Split(" | ")[0].Replace("  ", " ").Split(" ");
            var winnersNumbers = new List<int>();
            foreach (var numberTest in winners)
            {
                if (int.TryParse(numberTest, out int number))
                {
                    winnersNumbers.Add(number);
                }
            }

            var scratched = numbers.Split(" | ")[1].Replace("  ", " ").Split(" ");
            var scratchedNumbers = new List<int>();
            foreach (var numberTest in scratched)
            {
                if (int.TryParse(numberTest, out int number))
                {
                    scratchedNumbers.Add(number);
                }
            }

            var overlapCount = winnersNumbers.Intersect(scratchedNumbers).Count();
            sumOfOverlapPoints += CalculateValue(overlapCount);
        }

        return sumOfOverlapPoints;
    }

    private static int CalculateValue(int count)
    {
        if (count > 0)
        {
            var value = 1;
            for (int i = 1; i < count; i++)
            {
                value = value * 2;
            }
            return value;
        }
        else
        {
            return 0;
        }
    }
}

public sealed class CardEntry
{
    public int Number { get; set; }
    public List<int> WinningNumbers { get; set; } = new List<int>();
    public List<int> ScratchedNumbers { get; set; } = new List<int>();

    public int OverlapCount { get; set; }
}