using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Models;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Services;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_FE.Pages.News;

public class SearchModel : PageModel
{
    private readonly ApiService _apiService;

    [BindProperty(SupportsGet = true)]
    public string? Keyword { get; set; }

    [BindProperty(SupportsGet = true)]
    public short? CategoryId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? TagId { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? StartDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? EndDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SortBy { get; set; } = "date"; // "date" or "title"

    public List<NewsArticleDto> Articles { get; set; } = new();
    public List<CategoryDto> Categories { get; set; } = new();
    public List<TagDto> Tags { get; set; } = new();
    public int TotalCount { get; set; }

    public SearchModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync()
    {
        Categories = await _apiService.GetActiveCategoriesAsync();
        Tags = await _apiService.GetAllTagsAsync();

        // Get articles with all filters
        if (!string.IsNullOrEmpty(Keyword) || CategoryId.HasValue || TagId.HasValue || StartDate.HasValue || EndDate.HasValue)
        {
            var result = await _apiService.SearchNewsAdvancedAsync(
                keyword: Keyword,
                categoryId: CategoryId,
                tagId: TagId,
                startDate: StartDate,
                endDate: EndDate,
                sortBy: SortBy
            );
            Articles = result;
        }
        else
        {
            Articles = await _apiService.GetActiveNewsAsync();
        }

        // Apply client-side sorting if needed (fallback)
        Articles = ApplySorting(Articles, SortBy);
        TotalCount = Articles.Count;
    }

    private List<NewsArticleDto> ApplySorting(List<NewsArticleDto> articles, string? sortBy)
    {
        return sortBy?.ToLower() switch
        {
            "title" => articles.OrderBy(a => a.NewsTitle ?? a.Headline).ToList(),
            _ => articles.OrderByDescending(a => a.CreatedDate).ToList()
        };
    }
}
