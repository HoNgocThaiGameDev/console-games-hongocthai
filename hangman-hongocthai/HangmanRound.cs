namespace Hangman;

internal class HangmanRound
{
    public string SecretWord { get; }
    public HashSet<char> GuessedLetters { get; } = new();
    public int Lives { get; private set; } = 6;

    public HangmanRound(string secretWord)
    {
        SecretWord = secretWord;
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

    public string GetMaskedWord()
    {
        return string.Join(" ", SecretWord.Select(letter => GuessedLetters.Contains(letter) ? letter : '_'));
    }

    public bool IsWon()
    {
        return SecretWord.All(letter => GuessedLetters.Contains(letter));
    }
}
