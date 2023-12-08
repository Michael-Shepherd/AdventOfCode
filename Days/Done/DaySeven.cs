using System.Dynamic;
using Utilities;

namespace Days;

public class DaySeven
{
    private static Dictionary<char, int> cardValues = new Dictionary<char, int>()
    {
        {'A', 12},
        {'K', 11},
        {'Q', 10},
        {'J', -1},
        {'T', 8},
        {'9', 7},
        {'8', 6},
        {'7', 5},
        {'6', 4},
        {'5', 3},
        {'4', 2},
        {'3', 1},
        {'2', 0}
    };

    private static Dictionary<string, int> handValues = new Dictionary<string, int>()
    {
        {"Five", 6},
        {"Four", 5},
        {"FullHouse", 4},
        {"ThreeOfAKind", 3},
        {"TwoPair", 2},
        {"Pair", 1},
        {"High", 0},
    };

    public static long Handle()
    {
        return HandleStepTwo();
    }

    public static long HandleStepOne()
    {
        var input = InputReader.Get(".\\input\\day_seven_input.txt");

        // card, card value, card bet
        var cards = new List<(string, int, long)>() { };
        foreach (var line in input)
        {
            var hand = line.Split(" ").First().ToUpperInvariant();
            var value = GetHandValue(hand);
            long.TryParse(line.Split(" ").Last(), out var bet);

            cards.Add((hand, value, bet));
        }

        cards.Sort((x, y) =>
        {
            int result = y.Item2.CompareTo(x.Item2);
            if (result == 0)
            {
                Console.WriteLine($"{x.Item1} {x.Item2} {x.Item3}");
                Console.WriteLine($"{y.Item1} {y.Item2} {y.Item3}");
                var isFirstBetter = IsFirstHandStronger(x.Item1, y.Item1);
                Console.WriteLine($"isfirstbetter: {isFirstBetter}");

                return isFirstBetter;
            }
            else
            {
                return result;
            }
        });

        var result = (long)0;
        for (int i = 1; i <= cards.Count(); i++)
        {
            var winnings = (cards.Count() - i + 1) * cards[i - 1].Item3;
            result += winnings;
            Console.WriteLine($"{cards[i - 1].Item1} {cards[i - 1].Item2} {cards[i - 1].Item3} {winnings}");
        }

        return result;
    }

    private static int IsFirstHandStronger(string firstHand, string secondHand)
    {
        for (int i = 0; i < firstHand.Length; i++)
        {
            if (cardValues[firstHand[i]] > cardValues[secondHand[i]])
            {
                return -1;
            }
            else if (cardValues[firstHand[i]] < cardValues[secondHand[i]])
            {
                return 1;
            }
        }
        return 0;
    }

    private static int GetMaxHandValue(string hand)
    {
        var baseValue = GetHandValue(hand);


        if (hand.Contains('J'))
        {
            foreach (char c in hand)
            {
                foreach (var card in cardValues.Keys)
                {
                    if (card != 'J')
                    {
                        var newValue = GetHandValue(hand.Replace('J', card));
                        baseValue = newValue > baseValue ? newValue : baseValue;
                    }
                }
            }
        }

        return baseValue;
    }

    private static int GetHandValue(string hand)
    {
        var distinct = hand.Distinct().Count();

        // Five
        if (distinct == 1)
        {
            return handValues["Five"];
        }

        // Four of a kind or full house
        if (distinct == 2)
        {
            hand.Replace(hand[0] + "", "");
            // Four
            if (hand.Replace(hand[0] + "", "").Length == 1 || hand.Replace(hand[0] + "", "").Length == 4)
            {
                return handValues["Four"];
            }

            // Full House
            return handValues["FullHouse"];
        }

        // Three of a kind or two pairs
        if (distinct == 3)
        {
            // Three
            foreach (char c in hand)
            {
                if (hand.Replace(c + "", "").Length == 2)
                {
                    return handValues["ThreeOfAKind"];
                }
            }

            // Two Pair
            return handValues["TwoPair"];
        }

        // Pair
        if (distinct == 4)
        {
            return handValues["Pair"];
        }

        return handValues["High"];

    }

    public static long HandleStepTwo()
    {
        var input = InputReader.Get(".\\input\\day_seven_input.txt");

        // card, card value, card bet
        var cards = new List<(string, int, long)>() { };
        foreach (var line in input)
        {
            var hand = line.Split(" ").First().ToUpperInvariant();
            var value = GetMaxHandValue(hand);
            long.TryParse(line.Split(" ").Last(), out var bet);

            cards.Add((hand, value, bet));
        }

        cards.Sort((x, y) =>
        {
            int result = y.Item2.CompareTo(x.Item2);
            if (result == 0)
            {
                var isFirstBetter = IsFirstHandStronger(x.Item1, y.Item1);

                return isFirstBetter;
            }
            else
            {
                return result;
            }
        });

        var result = (long)0;
        for (int i = 1; i <= cards.Count(); i++)
        {
            var winnings = (cards.Count() - i + 1) * cards[i - 1].Item3;
            result += winnings;
        }

        return result;
    }
}