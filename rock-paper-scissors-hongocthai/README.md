# Rock Paper Scissors

Author: hongocthai

A C# console game based on GDD_Game2_Rock_Paper_Scissors.pdf.

## Run

Use the .NET 8 SDK, or a newer SDK with the .NET 8 runtime installed. Open a terminal in this folder and run:

```powershell
dotnet run
```

## How to play

- Choose the classic three-move game or the five-move Lizard Spock game.
- Choose a match length from 1 to 9 round wins and a Random or Adaptive opponent.
- Enter r/rock, p/paper or s/scissors. In the five-move game, l/lizard and k/spock are also accepted. Inputs ignore case and surrounding spaces.
- The first side to reach the selected number of wins takes the match. Draws do not change the score.
- Enter q/quit to abandon the match and see its summary.
- The summary lists every played round, including draws.
- Answer y/yes to start a new match or n/no to quit.
- End of input abandons the current match and exits normally.

All Must and Should requirements RPS-01 through RPS-12 are preserved on main. All three stretch goals are implemented on stretch-goals, which is the currently checked-out branch.

## Completed stretch goals

| ID | Feature | Behaviour |
| --- | --- | --- |
| S1 | Lizard Spock | All five moves, all ten winning relationships, and the matching rule verbs appear in round messages. Classic mode still accepts only the original three moves. |
| S2 | Match length | Choose 1-9 wins before every match. Invalid input is rejected and a match stops immediately at its target score. |
| S3 | Adaptive computer | Predicts the most frequent player move from earlier valid rounds in the current match, then chooses a move that beats it. With no history it chooses randomly; frequency ties and multiple counter-moves are resolved randomly. |

After each match, the game compares your round win rates against Random and Adaptive opponents during the session. Win rate is player round wins divided by all played rounds, including draws. Played rounds in abandoned matches are included. Prediction history resets for each match; session statistics remain until the program exits.

## Rule design

Moves and results use enums; round history uses a List. One dictionary stores the ten winning pairs and their verbs. DetermineResult contains all result decisions, and both printed rule messages and adaptive choices use the same rule data.

Three moves have 9 ordered pairings; five moves have 25. Storing winning pairs once keeps the rules readable without writing separate branches for every pairing. The computer chooses its move before reading the current player input.

## Branches

Run `git switch main` for the original game, or `git switch stretch-goals` for all extensions. Both branches are local; nothing has been pushed.

The project uses only the standard library. There are no NuGet packages or code comments.
