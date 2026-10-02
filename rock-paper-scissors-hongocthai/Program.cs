namespace RockPaperScissors;

internal static class Program
{
    private static readonly Move[] ClassicMoves = { Move.Rock, Move.Paper, Move.Scissors };
    private static readonly Move[] ExtendedMoves = { Move.Rock, Move.Paper, Move.Scissors, Move.Lizard, Move.Spock };
    private static readonly Move[] RuleOrder =
    {
        Move.Rock,
        Move.Scissors,
        Move.Lizard,
        Move.Paper,
        Move.Spock
    };
    private static readonly string[,] RuleVerbs =
    {
        { "crushes", "crushes" },
        { "decapitates", "cuts" },
        { "eats", "poisons" },
        { "disproves", "covers" },
        { "vaporizes", "smashes" }
    };

    private static void Main(string[] args)
    {
        Console.WriteLine("=== ROCK PAPER SCISSORS ===");
        var sessionStats = new SessionStats();

        do
        {
            Console.WriteLine();
            Console.WriteLine("1. Classic game");
            Console.WriteLine("2. Lizard-Spock game");
            int? gameMode = ReadChoice("Choose a game (1-2): ", 1, 2);

            if (gameMode == null)
            {
                break;
            }

            int? winningScore = ReadChoice("How many wins are needed (1-9): ", 1, 9);

            if (winningScore == null)
            {
                break;
            }

            Console.WriteLine();
            Console.WriteLine("1. Random computer");
            Console.WriteLine("2. Adaptive computer");
            int? opponent = ReadChoice("Choose an opponent (1-2): ", 1, 2);

            if (opponent == null)
            {
                break;
            }

            Move[] availableMoves;

            if (gameMode == 1)
            {
                availableMoves = ClassicMoves;
            }
            else
            {
                availableMoves = ExtendedMoves;
            }

            bool adaptive = opponent == 2;
            string winLabel;

            if (winningScore.Value == 1)
            {
                winLabel = "win";
            }
            else
            {
                winLabel = "wins";
            }

            Console.WriteLine();
            Console.WriteLine($"First to {winningScore.Value} round {winLabel} takes the match.");
            Console.WriteLine(GetInputHelp(availableMoves));

            if (adaptive)
            {
                Console.WriteLine("This computer watches the moves you played earlier.");
            }

            bool completed = PlayMatch(Random.Shared, availableMoves, winningScore.Value, adaptive, sessionStats);
            sessionStats.PrintComparison();

            if (!completed)
            {
                break;
            }
        }
        while (AskToPlayAgain());

        Console.WriteLine("Thanks for playing!");
    }

    private static bool PlayMatch(Random random, Move[] availableMoves, int winningScore, bool adaptive, SessionStats sessionStats)
    {
        int playerScore = 0;
        int computerScore = 0;
        var rounds = new List<Round>();
        var moveCounts = new Dictionary<Move, int>();

        foreach (Move move in availableMoves)
        {
            moveCounts.Add(move, 0);
        }

        while (playerScore < winningScore && computerScore < winningScore)
        {
            Move computerMove = ChooseComputerMove(random, availableMoves, adaptive, moveCounts);
            Console.Write($"Round {rounds.Count + 1} - your move: ");
            string? input = Console.ReadLine();

            if (input == null)
            {
                PrintSummary(rounds, playerScore, computerScore, winningScore, true);
                return false;
            }

            input = input.Trim().ToLowerInvariant();

            if (input == "q" || input == "quit")
            {
                PrintSummary(rounds, playerScore, computerScore, winningScore, true);
                return true;
            }

            if (!TryParseMove(input, out Move playerMove) || !availableMoves.Contains(playerMove))
            {
                Console.WriteLine($"Unrecognised move. {GetInputHelp(availableMoves)}");
                continue;
            }

            RoundResult result = DetermineResult(playerMove, computerMove);
            rounds.Add(new Round(playerMove, computerMove, result));
            moveCounts[playerMove]++;
            sessionStats.RecordRound(adaptive, result);

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
                    message = "Draw. This round does not count.";
                    break;
            }

            string rule = GetRuleMessage(playerMove, computerMove, result);
            Console.WriteLine($"You: {playerMove}  |  Computer: {computerMove}  ->  {rule}{message}");
            Console.WriteLine($"Score: You {playerScore} - {computerScore} Computer");
        }

        PrintSummary(rounds, playerScore, computerScore, winningScore, false);
        return true;
    }

    private static Move ChooseComputerMove(Random random, Move[] availableMoves, bool adaptive, Dictionary<Move, int> moveCounts)
    {
        if (!adaptive)
        {
            return availableMoves[random.Next(availableMoves.Length)];
        }

        bool hasHistory = false;

        foreach (int count in moveCounts.Values)
        {
            if (count > 0)
            {
                hasHistory = true;
                break;
            }
        }

        if (!hasHistory)
        {
            return availableMoves[random.Next(availableMoves.Length)];
        }

        int highestCount = 0;

        foreach (int count in moveCounts.Values)
        {
            if (count > highestCount)
            {
                highestCount = count;
            }
        }

        var likelyMoves = new List<Move>();

        foreach (KeyValuePair<Move, int> moveCount in moveCounts)
        {
            if (moveCount.Value == highestCount)
            {
                likelyMoves.Add(moveCount.Key);
            }
        }

        Move prediction = likelyMoves[random.Next(likelyMoves.Count)];
        var counters = new List<Move>();

        foreach (Move move in availableMoves)
        {
            if (DetermineResult(move, prediction) == RoundResult.Win)
            {
                counters.Add(move);
            }
        }

        return counters[random.Next(counters.Count)];
    }

    private static string GetInputHelp(Move[] availableMoves)
    {
        if (availableMoves.Length == 3)
        {
            return "Enter R (rock), P (paper), S (scissors) or Q to quit the match.";
        }

        return "Enter R (rock), P (paper), S (scissors), L (lizard), K (spock) or Q to quit the match.";
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
            case "l":
            case "lizard":
                move = Move.Lizard;
                return true;
            case "k":
            case "spock":
                move = Move.Spock;
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

        int playerIndex = GetRuleIndex(player);
        int computerIndex = GetRuleIndex(computer);
        int distance = (computerIndex - playerIndex + RuleOrder.Length) % RuleOrder.Length;

        if (distance == 1 || distance == 2)
        {
            return RoundResult.Win;
        }

        return RoundResult.Lose;
    }

    private static string GetRuleMessage(Move player, Move computer, RoundResult result)
    {
        if (result == RoundResult.Draw)
        {
            return string.Empty;
        }

        Move winner;
        Move loser;

        if (result == RoundResult.Win)
        {
            winner = player;
            loser = computer;
        }
        else
        {
            winner = computer;
            loser = player;
        }

        int winnerIndex = GetRuleIndex(winner);
        int loserIndex = GetRuleIndex(loser);
        int distance = (loserIndex - winnerIndex + RuleOrder.Length) % RuleOrder.Length;
        string verb = RuleVerbs[winnerIndex, distance - 1];
        string winnerName = winner.ToString();
        string loserName = loser.ToString();
        return $"{winnerName} {verb} {loserName}. ";
    }

    private static int GetRuleIndex(Move move)
    {
        for (int index = 0; index < RuleOrder.Length; index++)
        {
            if (RuleOrder[index] == move)
            {
                return index;
            }
        }

        return -1;
    }

    private static void PrintSummary(List<Round> rounds, int playerScore, int computerScore, int winningScore, bool abandoned)
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
            if (playerScore == winningScore)
            {
                Console.WriteLine("You win the match!");
            }
            else
            {
                Console.WriteLine("Computer wins the match.");
            }
        }
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
