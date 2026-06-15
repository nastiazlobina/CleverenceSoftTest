namespace CleverenceSoft.Task_1;

public class CompressionAlgorithm
{
    private string originString;

    public CompressionAlgorithm(string originString)
    {
        this.originString = originString;
    }

    public string Compression()
    {
        if (string.IsNullOrEmpty(originString))
        {
            return "Строка пустая";
        }

        bool isAllLetters = originString.All(c => Char.IsLetter(c) && (c >= 'a' && c <= 'z'));
        if (!isAllLetters)
        {
            return "Строка содержит символы помимо латинского алфавита";
        }

        return CompressionString(originString);
    }

    public string Decompression(string compression)
    {
        string output = "";
        for (int i = 0; i < compression.Length; i++)
        {
            char currentChar = compression[i];
            if (i + 1 < compression.Length && char.IsDigit(compression[i + 1]))
            {
                output += new string(currentChar, int.Parse(compression[i + 1].ToString()));
                i++;
            }
            else output += currentChar;
        }

        return output;
    }

    public string CompressionString(string input)
    {
        int countLetter = 1;
        string output = "";
        for (int i = 0; i < input.Length; i += countLetter)
        {
            countLetter = 1;
            for (int j = i + 1; j < input.Length; j++)
            {
                if (input[j] == input[i]) countLetter++;
                else break;
            }

            if (countLetter == 1) output += input[i];
            else output += $"{input[i]}{countLetter}";
        }

        return output;
    }
}