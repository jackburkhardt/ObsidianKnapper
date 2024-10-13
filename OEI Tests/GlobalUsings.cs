global using NUnit.Framework;


public static class TestUtils
{
    public static int CountOccurrences(string text, string searchItem)
    {
        return text.Split(searchItem).Length - 1;
    }
}