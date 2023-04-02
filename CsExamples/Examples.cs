using NUnit.Framework;

namespace CsExamples;

public class Examples
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [Ignore("Do not block test with input")]
    public void BuildingThisGivesYouNoWarning()
    {
        var line = Console.ReadLine();
    }
}