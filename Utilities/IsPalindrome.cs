namespace Utilities;

public static class StringExtensions
{
    public static bool IsPalindrome(this string text)
    {
        if (text.Length <= 1)
            return true;
        else
        {
            if (text[0] != text[text.Length - 1])
                return false;
            else
                return IsPalindrome(text.Substring(1, text.Length - 2));
        }
    }

    public static bool IsPalindromeEvenOnly(this List<List<string>> lists)
    {
        if (lists.Count() % 2 != 0)
        {
            return false;
        }
        var isPalindrome = true;

        for (int i = 0; i < lists.Count() / 2; i++)
        {
            if (!lists[i].SequenceEqual(lists[lists.Count() - 1 - i]))
            {
                isPalindrome = false;
                break;
            }
        }

        return isPalindrome;
    }
}