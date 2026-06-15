namespace CleverenceSoft.Tests;

public class ServerTests
{
    [SetUp]
    public void Setup()
    {
        Server.Reset();
    }

    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
    public void AddToCount_Test(int value)
    {
        Server.AddToCount(value);
        Assert.That(Server.GetCount(), Is.EqualTo(value));
    }

    [Test]
    public void MultipleWrites_Test()
    {
        Parallel.For(0, 1000, _ =>
        {
            Server.AddToCount(1);
        });
        Assert.That(Server.GetCount(), Is.EqualTo(1000));
    }

    [Test]
    public void MultipleReads_Test()
    {
        Server.AddToCount(42);
        Parallel.For(0, 1000, _ =>
        {
            Assert.That(Server.GetCount(), Is.EqualTo(42));
        });
    }

    [Test]
    public void ReadAndWrite_Test()
    {
        var tasks = new List<Task>();

        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                Server.AddToCount(1);
            }));
        }

        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                _ = Server.GetCount();
            }));
        }

        Task.WaitAll(tasks.ToArray());

        Assert.That(Server.GetCount(), Is.EqualTo(100));
    }
    
    [Test]
    public void AddNegativeValue_Test()
    {
        Server.AddToCount(10);
        Server.AddToCount(-3);

        Assert.That(Server.GetCount(), Is.EqualTo(7));
    }
}