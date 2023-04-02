namespace CsExamples.News;

public class BadHttpNewsService : INewsService
{
    static readonly HttpClient _httpClient = new HttpClient(); 

    public async Task<List<string>> GetLatestNews()
    {
        var response = await _httpClient.GetAsync("http://someWrongAdress831");

        // Here the implementation would convert response into a list
        // of the latest news. We return a made up list for the sake of 
        // the example.

        return new List<string> { "A chicken crossed the road", 
            "Science says turtles are friendly" };
    }
}