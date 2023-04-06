using NUnit.Framework;
using CsExamples.News;

namespace CsExamples;

public class Examples
{
    [Test]
    [Ignore("Do not block test with input")]
    public void BuildingThisGivesYouNoWarning()
    {
        var line = Console.ReadLine();
    }

    [Test]
    public void SadNewsReader()
    {
        // Picture the NewsReader resolving `BadHttpNewsService` via DI
        INewsService newsService = new BadNewsService();

        Assert.ThrowsAsync<HttpRequestException>(
            async () => {
                var latestNews = await newsService.GetLatestNews();
                foreach (var piece in latestNews)
                    Console.WriteLine(piece);
            }
        );
    }

    [Test]
    public void HappyNewsReader()
    {
        // Picture the NewsReader resolving `GoodHttpNewsService` via DI
        INewsService newsService = new GoodNewsService();

        try
        {
            var latestNews = newsService.GetLatestNews();
        }
        catch (NewsServiceException)
        {
            // Here the news reader would write to a log or notify the user
            Assert.Pass();
        }
        // catch (HttpRequestException) !!!
        // {
        //      This is where encapsulation would be broken. The role of the interface is to
        //      hide implementation details!
        // }
    }
}