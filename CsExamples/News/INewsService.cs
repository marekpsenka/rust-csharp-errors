namespace CsExamples.News;

public interface INewsService
{
    Task<List<string>> GetLatestNews();
}