namespace NumberGuessing;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== NUMBER GUESSING ===");

        do
        {
            int secretNumber = Random.Shared.Next(1, 101);
            Console.WriteLine("I'm thinking of a number between 1 and 100.");

            if (!PlayRound(secretNumber))
            {
                break;
            }
        }
        while (AskToPlayAgain());

        Console.WriteLine("Thanks for playing!");
    }

    private static bool PlayRound(int secretNumber)
    {
        int guessCount = 0;

        while (true)
        {
            Console.Write("Enter your guess: ");
            string? input = Console.ReadLine();

            if (input is null)
            {
                return false;
            }

            if (!int.TryParse(input, out int guess))
            {
                Console.WriteLine("Invalid input. Please enter a whole number.");
                continue;
            }

            if (guess < 1 || guess > 100)
            {
                Console.WriteLine("Out of range. Please enter a number between 1 and 100.");
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
                string guessLabel = guessCount == 1 ? "guess" : "guesses";
                Console.WriteLine($"Correct! The number was {secretNumber}.");
                Console.WriteLine($"You found it in {guessCount} {guessLabel}.");
                return true;
            }
        }
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
