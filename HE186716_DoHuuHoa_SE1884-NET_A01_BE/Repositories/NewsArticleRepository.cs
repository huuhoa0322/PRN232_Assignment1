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
            .Include(n => n.UpdatedBy)
            .Include(n => n.Tags)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
    }

    public async Task<NewsArticle?> GetByIdWithDetailsAsync(string id)
    {
        return await _dbSet
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.UpdatedBy)
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.NewsArticleId == id);
    }

    public async Task<IEnumerable<NewsArticle>> GetActiveArticlesAsync()
    {
        return await _dbSet
            .Where(n => n.NewsStatus == true)
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.UpdatedBy)
            .Include(n => n.Tags)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<NewsArticle>> SearchAsync(string? keyword, short? categoryId = null, bool? status = null, short? createdById = null, int? tagId = null)
    {
        var query = _dbSet.Include(n => n.Category)
                          .Include(n => n.CreatedBy)
                          .Include(n => n.UpdatedBy)
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

        // Filter by author (for Staff - only see their own articles)
        if (createdById.HasValue)
        {
            query = query.Where(n => n.CreatedById == createdById.Value);
        }

        // Filter by tag
        if (tagId.HasValue)
        {
            query = query.Where(n => n.Tags.Any(t => t.TagId == tagId.Value));
        }

        return await query.OrderByDescending(n => n.CreatedDate).ToListAsync();
    }

    public async Task<IEnumerable<NewsArticle>> FilterByDateRangeAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = _dbSet.Include(n => n.Category)
                          .Include(n => n.CreatedBy)
                          .Include(n => n.UpdatedBy)
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
            .Include(n => n.UpdatedBy)
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

        // Base query: exclude current article and only active articles
        var relatedQuery = _dbSet
            .Where(n => n.NewsArticleId != currentArticleId && n.NewsStatus == true)
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .AsQueryable();

        // Filter: same category OR share at least one tag
        relatedQuery = relatedQuery.Where(n => 
            (categoryId.HasValue && n.CategoryId == categoryId.Value) || 
            (currentTagIds.Any() && n.Tags.Any(t => currentTagIds.Contains(t.TagId))));

        return await relatedQuery
            .OrderByDescending(n => n.CreatedDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<string> GenerateNewIdAsync()
    {
        var allIds = await _dbSet
            .Select(n => n.NewsArticleId)
            .ToListAsync();

        if (!allIds.Any())
        {
            return "1";
        }

        // Parse all IDs to integers
        var numericIds = allIds
            .Where(id => int.TryParse(id, out _))
            .Select(id => int.Parse(id))
            .ToList();

        if (!numericIds.Any())
        {
            return "1";
        }

        // Get max and increment
        var maxId = numericIds.Max();
        return (maxId + 1).ToString();
    }
}
