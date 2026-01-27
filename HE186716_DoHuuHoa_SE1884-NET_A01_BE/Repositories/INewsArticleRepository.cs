using HE186716_DoHuuHoa_SE1884_NET_A01_BE.Models;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Repositories;

public interface INewsArticleRepository : IGenericRepository<NewsArticle>
{
    Task<IEnumerable<NewsArticle>> GetAllWithDetailsAsync();
    Task<NewsArticle?> GetByIdWithDetailsAsync(string id);
    Task<IEnumerable<NewsArticle>> GetActiveArticlesAsync();
    Task<IEnumerable<NewsArticle>> SearchAsync(string? keyword, short? categoryId = null, bool? status = null, short? createdById = null);
    Task<IEnumerable<NewsArticle>> FilterByDateRangeAsync(DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<NewsArticle>> GetByAuthorAsync(short createdById);
    Task<IEnumerable<NewsArticle>> GetRelatedArticlesAsync(string currentArticleId, short? categoryId, int count = 3);
    Task<string> GenerateNewIdAsync();
}
