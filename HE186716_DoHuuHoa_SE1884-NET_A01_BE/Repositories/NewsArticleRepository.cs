using Microsoft.EntityFrameworkCore;
using HE186716_DoHuuHoa_SE1884_NET_A01_BE.Models;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Repositories;

public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
{
    public NewsArticleRepository(FunewsManagementContext context) : base(context)
    {
    }

    public async Task<IEnumerable<NewsArticle>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
    }

    public async Task<NewsArticle?> GetByIdWithDetailsAsync(string id)
    {
        return await _dbSet
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.NewsArticleId == id);
    }

    public async Task<IEnumerable<NewsArticle>> GetActiveArticlesAsync()
    {
        return await _dbSet
            .Where(n => n.NewsStatus == true)
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<NewsArticle>> SearchAsync(string? keyword, short? categoryId = null, bool? status = null)
    {
        var query = _dbSet.Include(n => n.Category)
                          .Include(n => n.CreatedBy)
                          .Include(n => n.Tags)
                          .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.ToLower();
            query = query.Where(n =>
                (n.NewsTitle != null && n.NewsTitle.ToLower().Contains(keyword)) ||
                n.Headline.ToLower().Contains(keyword) ||
                (n.NewsContent != null && n.NewsContent.ToLower().Contains(keyword)) ||
                (n.CreatedBy != null && n.CreatedBy.AccountName != null && n.CreatedBy.AccountName.ToLower().Contains(keyword)) ||
                (n.Category != null && n.Category.CategoryName.ToLower().Contains(keyword)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(n => n.CategoryId == categoryId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(n => n.NewsStatus == status.Value);
        }

        return await query.OrderByDescending(n => n.CreatedDate).ToListAsync();
    }

    public async Task<IEnumerable<NewsArticle>> FilterByDateRangeAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = _dbSet.Include(n => n.Category)
                          .Include(n => n.CreatedBy)
                          .Include(n => n.Tags)
                          .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(n => n.CreatedDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(n => n.CreatedDate <= endDate.Value);
        }

        return await query.OrderByDescending(n => n.CreatedDate).ToListAsync();
    }

    public async Task<IEnumerable<NewsArticle>> GetByAuthorAsync(short createdById)
    {
        return await _dbSet
            .Where(n => n.CreatedById == createdById)
            .Include(n => n.Category)
            .Include(n => n.Tags)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<NewsArticle>> GetRelatedArticlesAsync(string currentArticleId, short? categoryId, int count = 3)
    {
        var currentArticle = await _dbSet
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.NewsArticleId == currentArticleId);

        if (currentArticle == null)
            return Enumerable.Empty<NewsArticle>();

        var currentTagIds = currentArticle.Tags.Select(t => t.TagId).ToList();

        var relatedQuery = _dbSet
            .Where(n => n.NewsArticleId != currentArticleId && n.NewsStatus == true)
            .Include(n => n.Category)
            .Include(n => n.Tags)
            .AsQueryable();

        // Priority: same category OR same tags
        if (categoryId.HasValue)
        {
            relatedQuery = relatedQuery.Where(n => 
                n.CategoryId == categoryId.Value || 
                n.Tags.Any(t => currentTagIds.Contains(t.TagId)));
        }

        return await relatedQuery
            .OrderByDescending(n => n.CreatedDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<string> GenerateNewIdAsync()
    {
        var maxId = await _dbSet
            .Select(n => n.NewsArticleId)
            .OrderByDescending(id => id)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(maxId) || !int.TryParse(maxId, out int numericId))
        {
            return "1";
        }

        return (numericId + 1).ToString();
    }
}
