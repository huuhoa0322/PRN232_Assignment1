using HE186716_DoHuuHoa_SE1884_NET_A01_BE.Models;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Repositories;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<IEnumerable<Tag>> SearchAsync(string? keyword);
    Task<bool> IsUsedInArticlesAsync(int tagId);
    Task<IEnumerable<Tag>> GetTagsByArticleIdAsync(string articleId);
    Task<int> GenerateNewIdAsync();
}
