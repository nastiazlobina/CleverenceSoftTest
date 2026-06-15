using CleverenceSoft.Task_1;


namespace CleverenceSoft.Tests;

public class CompressionAlgorithmTests
{
    [TestCase("aaabbbccca", "a3b3c3a")]
    [TestCase("abc", "abc")]
    [TestCase("aabbcc", "a2b2c2")]
    [TestCase("a", "a")]
    public void Compress_Test(string input, string expected)
    {
        var algo = new CompressionAlgorithm(input);
        string result = algo.Compression();
        Assert.That(result, Is.EqualTo(expected));  // ← универсальный синтаксис
    }
    
    [TestCase("a3b3c3a", "aaabbbccca")]
    [TestCase("abc", "abc")]
    [TestCase("a2b2c2", "aabbcc")]
    public void Decompress_Test(string input, string expected)
    {
        var algo = new CompressionAlgorithm("");
        string result = algo.Decompression(input);
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [TestCase("aaabbbccca")]
    [TestCase("abc")]
    [TestCase("aabbcc")]
    [TestCase("a")]
    public void CompressAndDecompress_ReturnsOriginal(string input)
    {
        var algo = new CompressionAlgorithm(input);
        string compressed = algo.Compression();
        string decompressed = algo.Decompression(compressed);
        Assert.That(decompressed, Is.EqualTo(input));
    }
    
    [Test]
    public void EmptyString_ReturnsErrorMessage()
    {
        var algo = new CompressionAlgorithm("");
        string result = algo.Compression();
        Assert.That(result, Is.EqualTo("Строка пустая"));
    }
    
    [Test]
    public void InvalidChars_ReturnsErrorMessage()
    {
        var algo = new CompressionAlgorithm("abc123");
        string result = algo.Compression();
        Assert.That(result, Is.EqualTo("Строка содержит символы помимо латинского алфавита"));
    }
}