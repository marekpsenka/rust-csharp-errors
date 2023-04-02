using NUnit.Framework;
using CsExamples.News;

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

    [Test]
    public void NewsReader01()
    {
        // Picture the NewsReader resolving `BadHttpNewsService` via DI
        INewsService newsService = new BadHttpNewsService();

        Assert.ThrowsAsync<HttpRequestException>(
            async () => {
                var latestNews = await newsService.GetLatestNews();
                foreach (var piece in latestNews)
                    Console.WriteLine(piece);
            }
        );
    }
}