namespace RockPaperScissors;

internal class SessionStats
{
    private int randomRounds;
    private int randomWins;
    private int adaptiveRounds;
    private int adaptiveWins;

    public void RecordRound(bool adaptive, RoundResult result)
    {
        if (adaptive)
        {
            adaptiveRounds++;

            if (result == RoundResult.Win)
            {
                adaptiveWins++;
            }
        }
        else
        {
            randomRounds++;

            if (result == RoundResult.Win)
            {
                randomWins++;
            }
        }
    }

    public void PrintComparison()
    {
        Console.WriteLine("=== SESSION WIN RATES ===");
        Console.WriteLine("draws are included in the round count");
        PrintWinRate("Random", randomWins, randomRounds);
        PrintWinRate("Adaptive", adaptiveWins, adaptiveRounds);
    }

    private static void PrintWinRate(string opponent, int wins, int rounds)
    {
        if (rounds == 0)
        {
            Console.WriteLine($"{opponent.ToLowerInvariant()}: no rounds yet");
            return;
        }

        double winRate = wins * 100.0 / rounds;
        Console.WriteLine($"{opponent.ToLowerInvariant()}: {wins}/{rounds} rounds won ({winRate:F1}%)");
    }
}
