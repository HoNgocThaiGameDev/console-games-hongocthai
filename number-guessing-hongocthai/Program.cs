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
            Console.WriteLine("1 - normal game");
            Console.WriteLine("2 - seven guesses");
            Console.WriteLine("3 - let the computer guess (1-100)");
            int? mode = ReadChoice("pick a mode [1-3] > ", 1, 3);

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
                Console.WriteLine("1 - easy   (1-10)");
                Console.WriteLine("2 - normal (1-100)");
                Console.WriteLine("3 - hard   (1-1000)");
                int? difficulty = ReadChoice("pick a difficulty [1-3] > ", 1, 3);

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

        Console.WriteLine("thanks for playing :)");
    }

    private static bool PlayRound(int secretNumber, int maximum, int? guessLimit, ref int? sessionBest)
    {
        int guessCount = 0;
        Console.WriteLine();
        Console.WriteLine($"i picked a number from 1 to {maximum}");

        if (guessLimit.HasValue)
        {
            Console.WriteLine($"you get {guessLimit.Value} guesses");
        }

        while (true)
        {
            Console.Write("your guess > ");
            string? input = Console.ReadLine();

            if (input == null)
            {
                return false;
            }

            if (!int.TryParse(input, out int guess))
            {
                Console.WriteLine("hmm, enter a whole number");
                continue;
            }

            if (guess < 1 || guess > maximum)
            {
                Console.WriteLine($"that one is outside 1-{maximum}");
                continue;
            }

            guessCount++;

            if (guess > secretNumber)
            {
                Console.WriteLine($"too high - go lower than {guess}");
            }
            else if (guess < secretNumber)
            {
                Console.WriteLine($"too low - go higher than {guess}");
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

                Console.WriteLine($"nice, the number was {secretNumber}!");
                Console.WriteLine($"you found it in {guessCount} {guessLabel}");
                ShowSessionBest(guessCount, ref sessionBest);
                return true;
            }

            if (guessLimit.HasValue)
            {
                int remaining = guessLimit.Value - guessCount;
                Console.WriteLine($"guesses left: {remaining}");

                if (remaining == 0)
                {
                    Console.WriteLine($"out of guesses - the number was {secretNumber}");
                    return true;
                }
            }
        }
    }

    private static bool PlayReverseRound(ref int? sessionBest)
    {
        Console.WriteLine();
        Console.WriteLine("think of a whole number from 1 to 100");
        Console.WriteLine("use h for too high, l for too low, or c when i get it");

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
                Console.Write($"guess #{guessCount}: {guess}  h/l/c? ");
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

                Console.WriteLine("use h, l or c");
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

                Console.WriteLine($"got it - {guess} in {guessCount} {guessLabel}!");
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

        Console.WriteLine("those answers don't fit any number from 1 to 100");
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

        Console.WriteLine($"best this session: {sessionBest.Value} {guessLabel}");
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

            Console.WriteLine($"enter a whole number from {minimum} to {maximum}");
        }
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
