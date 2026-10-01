namespace RockPaperScissors;

internal class Round
{
    public Move PlayerMove { get; }
    public Move ComputerMove { get; }
    public RoundResult Result { get; }

    public Round(Move playerMove, Move computerMove, RoundResult result)
    {
        PlayerMove = playerMove;
        ComputerMove = computerMove;
        Result = result;
    }
}
