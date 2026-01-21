using HE186716_DoHuuHoa_SE1884_NET_A01_BE.DTOs;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Services;

public interface INewsArticleService
{
    Task<IEnumerable<NewsArticleDto>> GetAllAsync();
    Task<IEnumerable<NewsArticleDto>> GetActiveArticlesAsync();
    Task<NewsArticleDto?> GetByIdAsync(string id);
    Task<IEnumerable<NewsArticleDto>> SearchAsync(NewsArticleSearchDto searchDto);
    Task<IEnumerable<NewsArticleDto>> FilterByDateRangeAsync(DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<NewsArticleDto>> GetByAuthorAsync(short createdById);
    Task<IEnumerable<NewsArticleDto>> GetRelatedArticlesAsync(string articleId);
    Task<NewsArticleDto> CreateAsync(CreateNewsArticleDto dto, short createdById);
    Task<NewsArticleDto?> UpdateAsync(string id, UpdateNewsArticleDto dto, short updatedById);
    Task<(bool Success, string Message)> DeleteAsync(string id);
    Task<NewsArticleDto?> DuplicateAsync(string id, short createdById);
}
