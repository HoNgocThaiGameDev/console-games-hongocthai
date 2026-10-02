namespace RockPaperScissors;

internal static class Program
{
    private const int WinningScore = 3;

    private static void Main()
    {
        Console.WriteLine("=== ROCK PAPER SCISSORS ===");
        Console.WriteLine("First to 3 round wins takes the match.");
        Console.WriteLine("Enter R (rock), P (paper) or S (scissors). Enter Q (quit) to quit the match.");

        do
        {
            if (!PlayMatch(Random.Shared))
            {
                break;
            }
        }
        while (AskToPlayAgain());

        Console.WriteLine("Thanks for playing!");
    }

    private static bool PlayMatch(Random random)
    {
        int playerScore = 0;
        int computerScore = 0;
        var rounds = new List<Round>();

        while (playerScore < WinningScore && computerScore < WinningScore)
        {
            Move computerMove = (Move)random.Next(3);
            Console.Write($"Round {rounds.Count + 1} - your move: ");
            string? input = Console.ReadLine();

            if (input is null)
            {
                PrintSummary(rounds, playerScore, computerScore, true);
                return false;
            }

            input = input.Trim().ToLowerInvariant();

            if (input == "q" || input == "quit")
            {
                PrintSummary(rounds, playerScore, computerScore, true);
                return true;
            }

            if (!TryParseMove(input, out Move playerMove))
            {
                Console.WriteLine("Unrecognised move. Please enter R, P, S or Q.");
                continue;
            }

            RoundResult result = DetermineResult(playerMove, computerMove);
            rounds.Add(new Round(playerMove, computerMove, result));

            string message;

            switch (result)
            {
                case RoundResult.Win:
                    playerScore++;
                    message = "You win this round!";
                    break;
                case RoundResult.Lose:
                    computerScore++;
                    message = "Computer wins this round.";
                    break;
                default:
                    message = "Draw. This round does not count towards the score.";
                    break;
            }

            Console.WriteLine($"You: {playerMove} | Computer: {computerMove} -> {message}");
            Console.WriteLine($"Score: You {playerScore} - {computerScore} Computer");
        }

        PrintSummary(rounds, playerScore, computerScore, false);
        return true;
    }

    private static bool TryParseMove(string input, out Move move)
    {
        switch (input.Trim().ToLowerInvariant())
        {
            case "r":
            case "rock":
                move = Move.Rock;
                return true;
            case "p":
            case "paper":
                move = Move.Paper;
                return true;
            case "s":
            case "scissors":
                move = Move.Scissors;
                return true;
            default:
                move = default;
                return false;
        }
    }

    private static RoundResult DetermineResult(Move player, Move computer)
    {
        if (player == computer)
        {
            return RoundResult.Draw;
        }

        return (player, computer) switch
        {
            (Move.Rock, Move.Scissors) => RoundResult.Win,
            (Move.Paper, Move.Rock) => RoundResult.Win,
            (Move.Scissors, Move.Paper) => RoundResult.Win,
            _ => RoundResult.Lose
        };
    }

    private static void PrintSummary(List<Round> rounds, int playerScore, int computerScore, bool abandoned)
    {
        Console.WriteLine();
        Console.WriteLine("=== MATCH SUMMARY ===");
        Console.WriteLine($"{"Round",-8}{"You",-12}{"Computer",-12}Result");

        for (int i = 0; i < rounds.Count; i++)
        {
            Round round = rounds[i];
            Console.WriteLine($"{i + 1,-8}{round.PlayerMove,-12}{round.ComputerMove,-12}{round.Result}");
        }

        Console.WriteLine($"Final score: You {playerScore} - {computerScore} Computer");

        if (abandoned)
        {
            Console.WriteLine("Match abandoned. No winner.");
        }
        else
        {
            Console.WriteLine(playerScore == WinningScore ? "You win the match!" : "Computer wins the match.");
        }
    }

    private static bool AskToPlayAgain()
    {
        while (true)
        {
            Console.Write("Play again? (y/yes or n/no): ");
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
                    Console.WriteLine("Please answer y/yes or n/no.");
                    break;
            }
        }
    }
}
