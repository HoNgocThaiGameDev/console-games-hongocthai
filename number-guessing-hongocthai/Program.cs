namespace NumberGuessing;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== NUMBER GUESSING ===");
        int? sessionBest = null;

        do
        {
            Console.WriteLine();
            Console.WriteLine("1. Standard game");
            Console.WriteLine("2. Seven guesses");
            Console.WriteLine("3. Computer guesses your number (1-100)");
            int? mode = ReadChoice("Choose a mode (1-3): ", 1, 3);

            if (mode == null)
            {
                break;
            }

            if (mode == 3)
            {
                if (!PlayReverseRound(ref sessionBest))
                {
                    break;
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("1. Easy (1-10)");
                Console.WriteLine("2. Normal (1-100)");
                Console.WriteLine("3. Hard (1-1000)");
                int? difficulty = ReadChoice("Choose a difficulty (1-3): ", 1, 3);

                if (difficulty == null)
                {
                    break;
                }

                int maximum;

                if (difficulty == 1)
                {
                    maximum = 10;
                }
                else if (difficulty == 2)
                {
                    maximum = 100;
                }
                else
                {
                    maximum = 1000;
                }

                int secretNumber = Random.Shared.Next(1, maximum + 1);
                int? guessLimit = null;

                if (mode == 2)
                {
                    guessLimit = 7;
                }

                if (!PlayRound(secretNumber, maximum, guessLimit, ref sessionBest))
                {
                    break;
                }
            }
        }
        while (AskToPlayAgain());

        Console.WriteLine("Thanks for playing!");
    }

    private static bool PlayRound(int secretNumber, int maximum, int? guessLimit, ref int? sessionBest)
    {
        int guessCount = 0;
        Console.WriteLine();
        Console.WriteLine($"I'm thinking of a number between 1 and {maximum}.");

        if (guessLimit.HasValue)
        {
            Console.WriteLine($"You have {guessLimit.Value} guesses.");
        }

        while (true)
        {
            Console.Write("Enter your guess: ");
            string? input = Console.ReadLine();

            if (input == null)
            {
                return false;
            }

            if (!int.TryParse(input, out int guess))
            {
                Console.WriteLine("Invalid input. Please enter a whole number.");
                continue;
            }

            if (guess < 1 || guess > maximum)
            {
                Console.WriteLine($"Out of range. Please enter a number between 1 and {maximum}.");
                continue;
            }

            guessCount++;

            if (guess > secretNumber)
            {
                Console.WriteLine($"Too high! The number is lower than {guess}.");
            }
            else if (guess < secretNumber)
            {
                Console.WriteLine($"Too low! The number is higher than {guess}.");
            }
            else
            {
                string guessLabel;

                if (guessCount == 1)
                {
                    guessLabel = "guess";
                }
                else
                {
                    guessLabel = "guesses";
                }

                Console.WriteLine($"Correct! The number was {secretNumber}.");
                Console.WriteLine($"You found it in {guessCount} {guessLabel}.");
                ShowSessionBest(guessCount, ref sessionBest);
                return true;
            }

            if (guessLimit.HasValue)
            {
                int remaining = guessLimit.Value - guessCount;
                Console.WriteLine($"Guesses left: {remaining}.");

                if (remaining == 0)
                {
                    Console.WriteLine($"Out of guesses. The number was {secretNumber}.");
                    return true;
                }
            }
        }
    }

    private static bool PlayReverseRound(ref int? sessionBest)
    {
        Console.WriteLine();
        Console.WriteLine("Think of a whole number between 1 and 100.");
        Console.WriteLine("Answer h if my guess is too high, l if it is too low, or c if it is correct.");

        int lower = 1;
        int upper = 100;
        int guessCount = 0;

        while (lower <= upper)
        {
            int guess = lower + (upper - lower) / 2;
            guessCount++;
            string answer;

            while (true)
            {
                Console.Write($"My guess #{guessCount}: {guess} (h/l/c): ");
                string? input = Console.ReadLine();

                if (input == null)
                {
                    return false;
                }

                answer = input.Trim().ToLowerInvariant();

                if (answer == "h" || answer == "l" || answer == "c")
                {
                    break;
                }

                Console.WriteLine("Please enter h, l or c.");
            }

            if (answer == "c")
            {
                string guessLabel;

                if (guessCount == 1)
                {
                    guessLabel = "guess";
                }
                else
                {
                    guessLabel = "guesses";
                }

                Console.WriteLine($"Correct! I found your number, {guess}, in {guessCount} {guessLabel}.");
                ShowSessionBest(guessCount, ref sessionBest);
                return true;
            }

            if (answer == "h")
            {
                upper = guess - 1;
            }
            else
            {
                lower = guess + 1;
            }
        }

        Console.WriteLine("Your answers do not match any number from 1 to 100.");
        return true;
    }

    private static void ShowSessionBest(int guessCount, ref int? sessionBest)
    {
        if (!sessionBest.HasValue || guessCount < sessionBest.Value)
        {
            sessionBest = guessCount;
        }

        string guessLabel;

        if (sessionBest.Value == 1)
        {
            guessLabel = "guess";
        }
        else
        {
            guessLabel = "guesses";
        }

        Console.WriteLine($"Best this session: {sessionBest.Value} {guessLabel}.");
    }

    private static int? ReadChoice(string prompt, int minimum, int maximum)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (input == null)
            {
                return null;
            }

            if (int.TryParse(input, out int choice) && choice >= minimum && choice <= maximum)
            {
                return choice;
            }

            Console.WriteLine($"Please enter a whole number from {minimum} to {maximum}.");
        }
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
