namespace Hangman;

internal class HangmanRound
{
    public string SecretWord { get; }
    public string Category { get; }
    public HashSet<char> GuessedLetters { get; } = new();
    public int Lives { get; private set; } = 6;

    public HangmanRound(string secretWord, string category)
    {
        SecretWord = secretWord;
        Category = category;
    }

    public bool TryGuess(char letter)
    {
        if (!GuessedLetters.Add(letter))
        {
            return false;
        }

        if (!SecretWord.Contains(letter))
        {
            Lives--;
        }

        return true;
    }

    public bool GuessWord(string guess)
    {
        if (string.Equals(guess, SecretWord, StringComparison.OrdinalIgnoreCase))
        {
            foreach (char letter in SecretWord)
            {
                GuessedLetters.Add(letter);
            }

            return true;
        }

        Lives = Math.Max(0, Lives - 2);
        return false;
    }

    public string GetMaskedWord()
    {
        var maskedLetters = new List<char>();

        foreach (char letter in SecretWord)
        {
            if (GuessedLetters.Contains(letter))
            {
                maskedLetters.Add(letter);
            }
            else
            {
                maskedLetters.Add('_');
            }
        }

        return string.Join(" ", maskedLetters);
    }

    public bool IsWon()
    {
        foreach (char letter in SecretWord)
        {
            if (!GuessedLetters.Contains(letter))
            {
                return false;
            }
        }

        return true;
    }
}
