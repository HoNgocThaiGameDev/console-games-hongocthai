# Number Guessing

Author: hongocthai

A C# console game based on GDD_Game1_Number_Guessing.pdf.

## Run

Use the .NET 8 SDK, or a newer SDK with the .NET 8 runtime installed. Open a terminal in this folder and run:

```powershell
dotnet run
```

## How to play

- Choose Standard, Seven guesses, or Computer guesses your number.
- For Standard and Seven guesses, choose Easy (1-10), Normal (1-100), or Hard (1-1000).
- Use the higher or lower hints until you find it.
- Only valid numbers in the range count as guesses.
- Answer y/yes to play again or n/no to quit. Answers ignore case and surrounding spaces.
- End of input exits the game normally.

All Must and Should requirements NG-01 through NG-11 are preserved on main. All four stretch goals are implemented on stretch-goals, which is the currently checked-out branch.

## Completed stretch goals

| ID | Feature | Behaviour |
| --- | --- | --- |
| S1 | Limited attempts | Seven valid guesses; every wrong guess shows the remaining count. The seventh wrong guess ends the round and reveals the number. |
| S2 | Reverse roles | Think of a number from 1 to 100 and answer h/l/c. The computer uses binary search, finds every consistent answer within seven guesses, and reports contradictory feedback when no number remains possible. |
| S3 | Difficulty | Easy 1-10, Normal 1-100, Hard 1-1000. Prompts and validation use the selected range. Reverse play keeps its specified 1-100 range. |
| S4 | Session best | Every win shows the lowest guess count so far in this session, including wins in reverse mode. Losses do not change it. |

Binary search halves the possible numbers after every wrong guess. Seven guesses suffice for 100 possibilities because 2^7 = 128.

## Branches

Run `git switch main` for the original game, or `git switch stretch-goals` for all extensions. Both branches are local; nothing has been pushed.

The project uses only the standard library. There are no NuGet packages or code comments.
