namespace Hangman;

internal static class WordList
{
    private static readonly Dictionary<string, string[]> Categories = new()
    {
        ["Animals"] = new[] { "TIGER", "RABBIT", "MONKEY", "DOLPHIN", "ELEPHANT" },
        ["Countries"] = new[] { "VIETNAM", "CANADA", "BRAZIL", "FRANCE", "JAPAN" },
        ["Games"] = new[] { "CHESS", "SUDOKU", "TETRIS", "HANGMAN", "PUZZLE" },
        ["Objects"] = new[] { "BALLOON", "WINDOW", "PENCIL", "CASTLE", "BICYCLE" },
        ["Food"] = new[] { "APPLE", "BANANA", "ORANGE", "CHERRY", "LEMON" }
    };

    public static List<WordEntry> Load(string path)
    {
        var builtInWords = new List<WordEntry>();

        foreach (KeyValuePair<string, string[]> category in Categories)
        {
            foreach (string word in category.Value)
            {
                builtInWords.Add(new WordEntry(word, category.Key));
            }
        }

        if (!File.Exists(path))
        {
            Console.WriteLine("Could not find words.txt. Using the built-in words.");
            return builtInWords;
        }

        try
        {
            var words = new List<WordEntry>();
            var seen = new HashSet<string>();

            foreach (string line in File.ReadLines(path))
            {
                string text = line.Trim();

                if (!IsValidWord(text))
                {
                    continue;
                }

                text = text.ToUpperInvariant();

                if (!seen.Add(text))
                {
                    continue;
                }

                string category = "Custom words";

                foreach (WordEntry builtInWord in builtInWords)
                {
                    if (builtInWord.Text == text)
                    {
                        category = builtInWord.Category;
                        break;
                    }
                }

                words.Add(new WordEntry(text, category));
            }

            if (words.Count > 0)
            {
                string wordLabel;

                if (words.Count == 1)
                {
                    wordLabel = "word";
                }
                else
                {
                    wordLabel = "words";
                }

                Console.WriteLine($"Loaded {words.Count} {wordLabel} from words.txt.");
                return words;
            }

            Console.WriteLine("No usable words in words.txt. Using the built-in words.");
        }
        catch (IOException)
        {
            Console.WriteLine("Could not read words.txt. Using the built-in words.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Could not read words.txt. Using the built-in words.");
        }

        return builtInWords;
    }

    public static bool IsValidWord(string text)
    {
        if (text.Length < 4 || text.Length > 10)
        {
            return false;
        }

        foreach (char character in text)
        {
            bool isUppercaseLetter = character >= 'A' && character <= 'Z';
            bool isLowercaseLetter = character >= 'a' && character <= 'z';

            if (!isUppercaseLetter && !isLowercaseLetter)
            {
                return false;
            }
        }

        return true;
    }
}
