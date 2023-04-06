namespace CsExamples.News;

public class GoodNewsService : INewsService
{
    static readonly HttpClient _httpClient = new HttpClient(); 

    public async Task<List<string>> GetLatestNews()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://someWrongAdress831");
        }
        catch (HttpRequestException e)
        {
            throw new NewsServiceException("Http client failed to retrieve the news", e);
        }

        // Here the implementation would convert response into a list
        // of the latest news. We return a made up list for the sake of 
        // the example.

        return new List<string> { "A chicken crossed the road", 
            "Science says turtles are friendly" };
    }
}