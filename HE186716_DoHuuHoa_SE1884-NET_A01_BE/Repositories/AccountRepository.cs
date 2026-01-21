using Microsoft.EntityFrameworkCore;
using HE186716_DoHuuHoa_SE1884_NET_A01_BE.Models;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Repositories;

public class AccountRepository : GenericRepository<SystemAccount>, IAccountRepository
{
    public AccountRepository(FunewsManagementContext context) : base(context)
    {
    }

    public async Task<SystemAccount?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.AccountEmail == email);
    }

    public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
    {
        return await _dbSet.FirstOrDefaultAsync(a => 
            a.AccountEmail == email && a.AccountPassword == password);
    }

    public async Task<bool> HasCreatedArticlesAsync(short accountId)
    {
        return await _context.NewsArticles.AnyAsync(n => n.CreatedById == accountId);
    }

    public async Task<IEnumerable<SystemAccount>> SearchAsync(string? keyword, int? role = null)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.ToLower();
            query = query.Where(a => 
                (a.AccountName != null && a.AccountName.ToLower().Contains(keyword)) ||
                (a.AccountEmail != null && a.AccountEmail.ToLower().Contains(keyword)));
        }

        if (role.HasValue)
        {
            query = query.Where(a => a.AccountRole == role.Value);
        }

        return await query.ToListAsync();
    }
}
