namespace RockPaperScissors;

internal static class Program
{
    private static readonly Move[] ClassicMoves = { Move.Rock, Move.Paper, Move.Scissors };
    private static readonly Move[] ExtendedMoves = { Move.Rock, Move.Paper, Move.Scissors, Move.Lizard, Move.Spock };
    private static readonly Dictionary<(Move Winner, Move Loser), string> Rules = new()
    {
        [(Move.Scissors, Move.Paper)] = "cuts",
        [(Move.Paper, Move.Rock)] = "covers",
        [(Move.Rock, Move.Lizard)] = "crushes",
        [(Move.Lizard, Move.Spock)] = "poisons",
        [(Move.Spock, Move.Scissors)] = "smashes",
        [(Move.Scissors, Move.Lizard)] = "decapitates",
        [(Move.Lizard, Move.Paper)] = "eats",
        [(Move.Paper, Move.Spock)] = "disproves",
        [(Move.Spock, Move.Rock)] = "vaporizes",
        [(Move.Rock, Move.Scissors)] = "crushes"
    };

    private static void Main(string[] args)
    {
        Console.WriteLine("=== ROCK PAPER SCISSORS ===");
        var sessionStats = new SessionStats();

        do
        {
            Console.WriteLine();
            Console.WriteLine("1 - classic");
            Console.WriteLine("2 - lizard spock");
            int? gameMode = ReadChoice("pick a game [1-2] > ", 1, 2);

            if (gameMode == null)
            {
                break;
            }

            int? winningScore = ReadChoice("wins needed [1-9] > ", 1, 9);

            if (winningScore == null)
            {
                break;
            }

            Console.WriteLine();
            Console.WriteLine("1 - random computer");
            Console.WriteLine("2 - adaptive computer");
            int? opponent = ReadChoice("pick an opponent [1-2] > ", 1, 2);

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
            Console.WriteLine($"first to {winningScore.Value} round {winLabel} takes it");
            Console.WriteLine(GetInputHelp(availableMoves));

            if (adaptive)
            {
                Console.WriteLine("this computer watches what you played earlier");
            }

            bool completed = PlayMatch(Random.Shared, availableMoves, winningScore.Value, adaptive, sessionStats);
            sessionStats.PrintComparison();

            if (!completed)
            {
                break;
            }
        }
        while (AskToPlayAgain());

        Console.WriteLine("thanks for playing :)");
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
            Console.Write($"round {rounds.Count + 1} | your move > ");
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
                Console.WriteLine($"didn't get that - {GetInputHelp(availableMoves)}");
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
                    message = "you take this round!";
                    break;
                case RoundResult.Lose:
                    computerScore++;
                    message = "computer takes this one";
                    break;
                default:
                    message = "draw - no point this time";
                    break;
            }

            string rule = GetRuleMessage(playerMove, computerMove, result);
            Console.WriteLine($"you: {playerMove} | computer: {computerMove} -> {rule}{message}");
            Console.WriteLine($"score  you {playerScore} - {computerScore} computer");
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
            return "use r/rock, p/paper, s/scissors or q/quit";
        }

        return "use r/rock, p/paper, s/scissors, l/lizard, k/spock or q/quit";
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

        if (Rules.ContainsKey((player, computer)))
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

        string winnerName = winner.ToString().ToLowerInvariant();
        string loserName = loser.ToString().ToLowerInvariant();
        return $"{winnerName} {Rules[(winner, loser)]} {loserName} - ";
    }

    private static void PrintSummary(List<Round> rounds, int playerScore, int computerScore, int winningScore, bool abandoned)
    {
        Console.WriteLine();
        Console.WriteLine("=== MATCH SUMMARY ===");
        Console.WriteLine($"{"round",-8}{"you",-12}{"computer",-12}result");

        for (int i = 0; i < rounds.Count; i++)
        {
            Round round = rounds[i];
            Console.WriteLine($"{i + 1,-8}{round.PlayerMove,-12}{round.ComputerMove,-12}{round.Result}");
        }

        Console.WriteLine($"final score  you {playerScore} - {computerScore} computer");

        if (abandoned)
        {
            Console.WriteLine("match stopped - no winner");
        }
        else
        {
            if (playerScore == winningScore)
            {
                Console.WriteLine("you win the match!");
            }
            else
            {
                Console.WriteLine("computer wins the match");
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
