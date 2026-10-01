# Console Games

Three small C# console games by hongocthai:

- `number-guessing-hongocthai`
- `rock-paper-scissors-hongocthai`
- `hangman-hongocthai`

Each folder is a standalone .NET 8 console project. Open a terminal in one of the folders and run:

```powershell
dotnet run
```

The projects include their core game rules and the stretch goals from the supplied GDDs. They use only the .NET standard library and have no NuGet dependencies.

The `verification` folder contains the acceptance and integration results. The code intentionally keeps the console flow straightforward so it is easy to follow and change.

## Build all three

```powershell
dotnet build .\number-guessing-hongocthai\number-guessing-hongocthai.csproj
dotnet build .\rock-paper-scissors-hongocthai\rock-paper-scissors-hongocthai.csproj
dotnet build .\hangman-hongocthai\hangman-hongocthai.csproj
```
