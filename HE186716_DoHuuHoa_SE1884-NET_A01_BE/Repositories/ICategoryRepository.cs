using HE186716_DoHuuHoa_SE1884_NET_A01_BE.Models;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<Category>> GetAllWithArticleCountAsync();
    Task<bool> HasArticlesAsync(short categoryId);
    Task<IEnumerable<Category>> SearchAsync(string? keyword);
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();
}
