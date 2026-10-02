namespace Hangman;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== HANGMAN ===");
        Console.WriteLine("Guess the word one letter at a time. You have 6 lives.");
        Console.WriteLine("You can also guess the whole word; a wrong word costs 2 lives.");
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

        Console.WriteLine("Thanks for playing!");
    }

    private static bool PlayRound(string secretWord, string category)
    {
        var round = new HangmanRound(secretWord, category);

        while (round.Lives > 0 && !round.IsWon())
        {
            PrintState(round);
            Console.Write("Your guess: ");
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
                    Console.WriteLine("Please enter exactly one letter (A-Z) or a whole word (A-Z).");
                    continue;
                }

                int previousLives = round.Lives;

                if (round.GuessWord(input.ToUpperInvariant()))
                {
                    Console.WriteLine("Correct whole-word guess!");
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

                    Console.WriteLine($"Wrong word. You lost {lostLives} {lifeLabel}. Lives left: {round.Lives}.");
                }

                continue;
            }

            if (!TryParseLetter(input, out char letter))
            {
                Console.WriteLine("Please enter exactly one letter (A-Z) or a whole word (A-Z).");
                continue;
            }

            if (!round.TryGuess(letter))
            {
                Console.WriteLine($"You already tried {letter}. No life lost.");
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

                Console.WriteLine($"Good guess! {letter} appears {count} {timeLabel}.");
            }
            else
            {
                Console.WriteLine($"Sorry, there is no {letter}. Lives left: {round.Lives}.");
            }
        }

        PrintState(round);
        if (round.IsWon())
        {
            Console.WriteLine($"You win! The word was {secretWord}.");
        }
        else
        {
            Console.WriteLine($"You lose! The word was {secretWord}.");
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
        Console.WriteLine($"Category: {round.Category}");
        Console.WriteLine($"Word:    {round.GetMaskedWord()}");
        Console.WriteLine($"Lives:   {round.Lives}");
        Console.WriteLine($"Guessed: {guessedLetters}");
    }

    private static bool AskToPlayAgain()
    {
        while (true)
        {
            Console.Write("Play again? (y/n): ");
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
                    Console.WriteLine("Please answer y or n.");
                    break;
            }
        }
    }
}
