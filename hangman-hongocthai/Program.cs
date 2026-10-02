namespace Hangman;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== HANGMAN ===");
        Console.WriteLine("guess one letter at a time - you have 6 lives");
        Console.WriteLine("or try the whole word, but a miss costs 2 lives");
        List<WordEntry> words = WordList.Load(Path.Combine(AppContext.BaseDirectory, "words.txt"));

        do
        {
            WordEntry word = words[Random.Shared.Next(words.Count)];

            if (!PlayRound(word.Text, word.Category))
            {
                break;
            }
        }
        while (AskToPlayAgain());

        Console.WriteLine("thanks for playing :)");
    }

    private static bool PlayRound(string secretWord, string category)
    {
        var round = new HangmanRound(secretWord, category);

        while (round.Lives > 0 && !round.IsWon())
        {
            PrintState(round);
            Console.Write("letter or whole word > ");
            string? input = Console.ReadLine();

            if (input == null)
            {
                return false;
            }

            input = input.Trim();

            if (input.Length > 1)
            {
                if (!WordList.IsValidWord(input))
                {
                    Console.WriteLine("use one letter, or a 4-10 letter word (a-z)");
                    continue;
                }

                int previousLives = round.Lives;

                if (round.GuessWord(input.ToUpperInvariant()))
                {
                    Console.WriteLine("that's the word!");
                }
                else
                {
                    int lostLives = previousLives - round.Lives;
                    string lifeLabel;

                    if (lostLives == 1)
                    {
                        lifeLabel = "life";
                    }
                    else
                    {
                        lifeLabel = "lives";
                    }

                    Console.WriteLine($"nope - lost {lostLives} {lifeLabel}, {round.Lives} left");
                }

                continue;
            }

            if (!TryParseLetter(input, out char letter))
            {
                Console.WriteLine("use one letter, or a 4-10 letter word (a-z)");
                continue;
            }

            if (!round.TryGuess(letter))
            {
                Console.WriteLine($"you already tried {letter} - no life lost");
            }
            else if (secretWord.Contains(letter))
            {
                int count = 0;

                foreach (char character in secretWord)
                {
                    if (character == letter)
                    {
                        count++;
                    }
                }

                string timeLabel;

                if (count == 1)
                {
                    timeLabel = "time";
                }
                else
                {
                    timeLabel = "times";
                }

                Console.WriteLine($"good one - {letter} shows up {count} {timeLabel}");
            }
            else
            {
                Console.WriteLine($"no {letter} this time, {round.Lives} lives left");
            }
        }

        PrintState(round);
        if (round.IsWon())
        {
            Console.WriteLine($"you got it! the word was {secretWord}");
        }
        else
        {
            Console.WriteLine($"game over - the word was {secretWord}");
        }

        return true;
    }

    private static bool TryParseLetter(string input, out char letter)
    {
        input = input.Trim();
        letter = default;

        if (input.Length != 1)
        {
            return false;
        }

        char character = input[0];

        if ((character < 'A' || character > 'Z') && (character < 'a' || character > 'z'))
        {
            return false;
        }

        letter = char.ToUpperInvariant(character);
        return true;
    }

    private static void PrintState(HangmanRound round)
    {
        string guessedLetters;

        if (round.GuessedLetters.Count == 0)
        {
            guessedLetters = "(none)";
        }
        else
        {
            var sortedLetters = new List<char>(round.GuessedLetters);
            sortedLetters.Sort();
            guessedLetters = string.Join(", ", sortedLetters);
        }

        Console.WriteLine(Gallows.Stages[6 - round.Lives]);
        Console.WriteLine($"category  {round.Category}");
        Console.WriteLine($"word      {round.GetMaskedWord()}");
        Console.WriteLine($"lives     {round.Lives}");
        Console.WriteLine($"tried     {guessedLetters}");
    }

    private static bool AskToPlayAgain()
    {
        while (true)
        {
            Console.Write("play again? [y/n] > ");
            string? input = Console.ReadLine();

            if (input == null)
            {
                return false;
            }

            switch (input.Trim().ToLowerInvariant())
            {
                case "y":
                case "yes":
                    return true;
                case "n":
                case "no":
                    return false;
                default:
                    Console.WriteLine("just y/yes or n/no here");
                    break;
            }
        }
    }
}
