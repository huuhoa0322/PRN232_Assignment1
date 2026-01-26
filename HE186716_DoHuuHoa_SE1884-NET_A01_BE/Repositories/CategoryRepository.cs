using Microsoft.EntityFrameworkCore;
using HE186716_DoHuuHoa_SE1884_NET_A01_BE.Models;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(FunewsManagementContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> GetAllWithArticleCountAsync()
    {
        return await _dbSet
            .Include(c => c.NewsArticles)
            .Include(c => c.ParentCategory)
            .ToListAsync();
    }

    public async Task<bool> HasArticlesAsync(short categoryId)
    {
        return await _context.NewsArticles.AnyAsync(n => n.CategoryId == categoryId);
    }

    public async Task<IEnumerable<Category>> SearchAsync(string? keyword)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.ToLower();
            query = query.Where(c => 
                c.CategoryName.ToLower().Contains(keyword) ||
                c.CategoryDesciption.ToLower().Contains(keyword));
        }

        return await query.Include(c => c.NewsArticles).ToListAsync();
    }

    public async Task<(IEnumerable<Category> Items, int TotalCount)> SearchPagedAsync(string? keyword, int pageIndex, int pageSize)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.ToLower();
            query = query.Where(c => 
                c.CategoryName.ToLower().Contains(keyword) ||
                c.CategoryDesciption.ToLower().Contains(keyword));
        }

        var totalCount = await query.CountAsync();
        
        var items = await query
            .Include(c => c.NewsArticles)
            .Include(c => c.ParentCategory)
            .OrderBy(c => c.CategoryId)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
    {
        return await _dbSet.Where(c => c.IsActive == true).ToListAsync();
    }
}
