using HE186716_DoHuuHoa_SE1884_NET_A01_BE.Models;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Repositories;

public interface IAccountRepository : IGenericRepository<SystemAccount>
{
    Task<SystemAccount?> GetByEmailAsync(string email);
    Task<SystemAccount?> AuthenticateAsync(string email, string password);
    Task<bool> HasCreatedArticlesAsync(short accountId);
    Task<IEnumerable<SystemAccount>> SearchAsync(string? keyword, int? role = null);
}
