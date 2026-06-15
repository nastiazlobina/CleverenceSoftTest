namespace CleverenceSoft.Task_3;

using System.Globalization;
using System.Text.RegularExpressions;

public class LogProcessor
{
    private const string DefaultMethod = "DEFAULT";

    private static readonly Regex Format1 = new(
        @"^(?<date>\d{2}\.\d{2}\.\d{4})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\s+(?<level>[A-Z]+)\s+(?<message>.+)$",
        RegexOptions.Compiled);

    private static readonly Regex Format2 = new(
        @"^(?<date>\d{4}-\d{2}-\d{2})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\|\s*(?<level>[A-Z]+)\|\d+\|(?<method>[^|]+)\|\s*(?<message>.+)$",
        RegexOptions.Compiled);

    /// <summary>
    /// Преобразует уровень логирования к единому виду.
    /// </summary>
    private static string NormalizeLevel(string level)
    {
        return level.Trim().ToUpperInvariant() switch
        {
            "INFORMATION" => "INFO",
            "WARNING" => "WARN",
            "INFO" => "INFO",
            "WARN" => "WARN",
            "ERROR" => "ERROR",
            "DEBUG" => "DEBUG",
            _ => "INFO"
        };
    }

    /// <summary>
    /// Разбор строки лога в структурированную запись
    /// </summary>
    public static bool TryParse(string line, out LogEntry entry)
    {
        entry = new LogEntry();

        Match match = Format1.Match(line);
        if (match.Success)
        {
            var date = DateTime.ParseExact(
                match.Groups["date"].Value,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture);

            entry.Date = date.ToString("dd-MM-yyyy");
            entry.Time = match.Groups["time"].Value;
            entry.Level = NormalizeLevel(match.Groups["level"].Value);
            entry.Method = DefaultMethod;
            entry.Message = match.Groups["message"].Value;

            return true;
        }

        match = Format2.Match(line);
        if (match.Success)
        {
            var date = DateTime.ParseExact(
                match.Groups["date"].Value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture);

            entry.Date = date.ToString("dd-MM-yyyy");
            entry.Time = match.Groups["time"].Value;
            entry.Level = NormalizeLevel(match.Groups["level"].Value);
            entry.Method = match.Groups["method"].Value;
            entry.Message = match.Groups["message"].Value;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Преобразует лог запись в строку выходного формата
    /// </summary>
    public static string ToOutputString(LogEntry entry)
    {
        return string.Join("\t",
            entry.Date,
            entry.Time,
            entry.Level,
            entry.Method,
            entry.Message);
    }

    /// <summary>
    /// Обрабатывает входной лог файл и записывает результат и ошибки в файлы
    /// </summary>
    public static void Process(string inputFile, string outputFile, string problemsFile)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(outputFile)!);
        Directory.CreateDirectory(Path.GetDirectoryName(problemsFile)!);

        using var outputWriter = new StreamWriter(outputFile);
        using var problemWriter = new StreamWriter(problemsFile);

        foreach (string line in File.ReadLines(inputFile))
        {
            if (TryParse(line, out LogEntry entry))
            {
                outputWriter.WriteLine(ToOutputString(entry));
            }
            else
            {
                problemWriter.WriteLine(line);
            }
        }
    }
}