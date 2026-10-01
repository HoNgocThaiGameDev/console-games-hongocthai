# Hangman

Author: hongocthai

A C# console game based on GDD_Game3_Hangman.pdf.

## Run

Use the .NET 8 SDK, or a newer SDK with the .NET 8 runtime installed. Open a terminal in this folder and run:

```powershell
dotnet run
```

## How to play

- Guess one letter A-Z at a time. Lowercase letters and surrounding spaces are accepted.
- You may also guess an entire word of 4-10 letters A-Z. A wrong word costs two lives, or the last remaining life.
- You start with 6 lives. Each new wrong letter costs one life.
- A correct guess reveals every occurrence of the letter.
- Repeated guesses and invalid input cost no lives.
- Reveal the whole word to win. The word is shown if you run out of lives.
- Answer y/yes to start a new round or n/no to quit.
- End of input exits the game normally.

All Must and Should requirements HM-01 through HM-14 are preserved on main. All four stretch goals are implemented on stretch-goals, which is the currently checked-out branch.

## Completed stretch goals

| ID | Feature | Behaviour |
| --- | --- | --- |
| S1 | Word file | Loads words.txt next to the running program. Trims lines, normalizes valid ASCII letters to uppercase, ignores blank or invalid lines and removes duplicates. Missing, empty, invalid, or unreadable files fall back to the built-in list with a message. |
| S2 | ASCII gallows | Seven stages for 0-6 mistakes. The drawing is shown before every guess and at the end of the round. Wrong whole-word guesses advance it by the number of lives lost. |
| S3 | Categories | Built-in words are grouped into Animals, Countries, Games, Objects, and Food. Every state shows the category as a hint. |
| S4 | Whole-word guess | A correct word wins immediately and reveals every position. A wrong word costs two lives without making lives negative. |

## Word file

Edit words.txt in this project, with one word per line. Words must contain 4-10 plain English letters. The build copies this file beside the executable, and published builds include it too.

The supplied file and fallback list each contain 25 words. File words matching the built-in list inherit their category; other valid file words use the Custom words category.

HangmanRound keeps the secret word, category, HashSet of guessed letters and remaining lives together.

## Branches

Run `git switch main` for the original game, or `git switch stretch-goals` for all extensions. Both branches are local; nothing has been pushed.

The project uses only the standard library. There are no NuGet packages or code comments.
