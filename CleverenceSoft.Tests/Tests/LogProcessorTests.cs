using CleverenceSoft.Task_3;

namespace CleverenceSoft.Tests;

public class LogProcessorTests
{
    [Test]
    public void Parse_Format1_Test()
    {
        string line = "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'";

        bool result = LogProcessor.TryParse(line, out var entry);

        Assert.That(result, Is.True);
        Assert.That(entry.Date, Is.EqualTo("10-03-2025"));
        Assert.That(entry.Time, Is.EqualTo("15:14:49.523"));
        Assert.That(entry.Level, Is.EqualTo("INFO"));
        Assert.That(entry.Method, Is.EqualTo("DEFAULT"));
        Assert.That(entry.Message, Is.EqualTo("Версия программы: '3.4.0.48729'"));
    }

    [Test]
    public void Parse_Format2_Test()
    {
        string line = "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO'";

        bool result = LogProcessor.TryParse(line, out var entry);

        Assert.That(result, Is.True);
        Assert.That(entry.Date, Is.EqualTo("10-03-2025"));
        Assert.That(entry.Time, Is.EqualTo("15:14:51.5882"));
        Assert.That(entry.Level, Is.EqualTo("INFO"));
        Assert.That(entry.Method, Is.EqualTo("MobileComputer.GetDeviceId"));
        Assert.That(entry.Message, Is.EqualTo("Код устройства: '@MINDEO'"));
    }

    [TestCase("INFORMATION", "INFO")]
    [TestCase("WARNING", "WARN")]
    [TestCase("INFO", "INFO")]
    [TestCase("ERROR", "ERROR")]
    [TestCase("DEBUG", "DEBUG")]
    public void Normalize_Level_Test(string input, string expected)
    {
        string line = $"10.03.2025 10:00:00.000 {input} Test message";

        bool result = LogProcessor.TryParse(line, out var entry);

        Assert.That(result, Is.True);
        Assert.That(entry.Level, Is.EqualTo(expected));
    }

    [Test]
    public void Missing_Method_Should_Be_Default()
    {
        string line = "10.03.2025 10:00:00.000 INFORMATION Some message";

        LogProcessor.TryParse(line, out var entry);

        Assert.That(entry.Method, Is.EqualTo("DEFAULT"));
    }

    [Test]
    public void Invalid_Line_Should_Return_False()
    {
        string line = "this is not a log line";

        bool result = LogProcessor.TryParse(line, out var entry);

        Assert.That(result, Is.False);
    }

    [Test]
    public void ToOutputString_Test()
    {
        var entry = new LogEntry
        {
            Date = "10-03-2025",
            Time = "15:14:49.523",
            Level = "INFO",
            Method = "DEFAULT",
            Message = "test message"
        };

        string result = LogProcessor.ToOutputString(entry);

        Assert.That(
            result,
            Is.EqualTo("10-03-2025\t15:14:49.523\tINFO\tDEFAULT\ttest message"));
    }

    [Test]
    public void Process_File_EndToEnd_Test()
    {
        string input = Path.GetTempFileName();
        string output = Path.GetTempFileName();
        string problems = Path.GetTempFileName();

        File.WriteAllLines(input, new[]
        {
            "10.03.2025 15:14:49.523 INFORMATION Test message",
            "invalid line"
        });

        LogProcessor.Process(input, output, problems);

        var outLines = File.ReadAllLines(output);
        var problemLines = File.ReadAllLines(problems);

        Assert.That(outLines.Length, Is.EqualTo(1));
        Assert.That(problemLines.Length, Is.EqualTo(1));

        Assert.That(outLines[0].Contains("INFO"), Is.True);
    }
}