namespace Hangman;

internal static class Program
{
    private static readonly string[] Words =
    {
        "APPLE", "BANANA", "ORANGE", "CHERRY", "LEMON",
        "TIGER", "RABBIT", "MONKEY", "DOLPHIN", "ELEPHANT",
        "GARDEN", "WINDOW", "PENCIL", "SCHOOL", "CASTLE",
        "PUZZLE", "BALLOON", "HANGMAN", "RAINBOW", "BICYCLE"
    };

    private static void Main(string[] args)
    {
        Console.WriteLine("=== HANGMAN ===");
        Console.WriteLine("Guess the word one letter at a time. You have 6 lives.");

        do
        {
            string secretWord = Words[Random.Shared.Next(Words.Length)];

            if (!PlayRound(secretWord))
            {
                break;
            }
        }
        while (AskToPlayAgain());

        Console.WriteLine("Thanks for playing!");
    }

    private static bool PlayRound(string secretWord)
    {
        var round = new HangmanRound(secretWord);

        while (round.Lives > 0 && !round.IsWon())
        {
            PrintState(round);
            Console.Write("Your guess: ");
            string? input = Console.ReadLine();

            if (input is null)
            {
                return false;
            }

            if (!TryParseLetter(input, out char letter))
            {
                Console.WriteLine("Please enter exactly one letter (A-Z).");
                continue;
            }

            if (!round.TryGuess(letter))
            {
                Console.WriteLine($"You already tried {letter}. No life lost.");
            }
            else if (secretWord.Contains(letter))
            {
                int count = secretWord.Count(character => character == letter);
                string timeLabel = count == 1 ? "time" : "times";
                Console.WriteLine($"Good guess! {letter} appears {count} {timeLabel}.");
            }
            else
            {
                Console.WriteLine($"Sorry, there is no {letter}. Lives left: {round.Lives}.");
            }
        }

        PrintState(round);
        Console.WriteLine(round.IsWon()
            ? $"You win! The word was {secretWord}."
            : $"You lose! The word was {secretWord}.");
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
        string guessedLetters = round.GuessedLetters.Count == 0
            ? "(none)"
            : string.Join(", ", round.GuessedLetters.OrderBy(letter => letter));

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

            if (input is null)
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
