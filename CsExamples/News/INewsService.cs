namespace CsExamples.News;

public class NewsServiceException : Exception
{
    public NewsServiceException(string message, Exception inner) : base(message, inner)
    {
    }
}

public interface INewsService
{
    Task<List<string>> GetLatestNews();
}