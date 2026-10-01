namespace Hangman;

internal class WordEntry
{
    public string Text { get; }
    public string Category { get; }

    public WordEntry(string text, string category)
    {
        Text = text;
        Category = category;
    }
}
