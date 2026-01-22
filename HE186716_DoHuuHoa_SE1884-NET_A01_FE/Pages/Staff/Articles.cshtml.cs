using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Models;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Services;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_FE.Pages.Staff;

public class ArticlesModel : PageModel
{
    private readonly ApiService _apiService;
    public List<NewsArticleDto> Articles { get; set; } = new();
    public List<CategoryDto> Categories { get; set; } = new();
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }

    public ArticlesModel(ApiService apiService) => _apiService = apiService;

    private short GetUserId() => short.TryParse(HttpContext.Session.GetString("UserId"), out var id) ? id : (short)0;

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("Role") != "1") return RedirectToPage("/Auth/Login");
        Articles = await _apiService.GetAllNewsAsync();
        Categories = await _apiService.GetActiveCategoriesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string handler, string? ArticleId, string? NewsTitle, string Headline, 
        string? NewsContent, string? NewsSource, short? CategoryId, bool NewsStatus)
    {
        if (HttpContext.Session.GetString("Role") != "1") return RedirectToPage("/Auth/Login");

        if (handler == "Create")
        {
            var dto = new CreateNewsArticleDto
            {
                NewsTitle = NewsTitle, Headline = Headline, NewsContent = NewsContent,
                NewsSource = NewsSource, CategoryId = CategoryId, NewsStatus = NewsStatus
            };
            var result = await _apiService.CreateNewsAsync(dto, GetUserId());
            Message = result.Success ? "Tạo thành công" : result.Message;
            IsSuccess = result.Success;
        }
        else if (handler == "Update" && !string.IsNullOrEmpty(ArticleId))
        {
            var dto = new UpdateNewsArticleDto
            {
                NewsTitle = NewsTitle, Headline = Headline, NewsContent = NewsContent,
                NewsSource = NewsSource, CategoryId = CategoryId, NewsStatus = NewsStatus
            };
            var result = await _apiService.UpdateNewsAsync(ArticleId, dto, GetUserId());
            Message = result.Success ? "Cập nhật thành công" : result.Message;
            IsSuccess = result.Success;
        }

        Articles = await _apiService.GetAllNewsAsync();
        Categories = await _apiService.GetActiveCategoriesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string deleteId)
    {
        if (HttpContext.Session.GetString("Role") != "1") return RedirectToPage("/Auth/Login");
        var result = await _apiService.DeleteNewsAsync(deleteId);
        Message = result.Success ? "Xóa thành công" : result.Message;
        IsSuccess = result.Success;
        Articles = await _apiService.GetAllNewsAsync();
        Categories = await _apiService.GetActiveCategoriesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostDuplicateAsync(string duplicateId)
    {
        if (HttpContext.Session.GetString("Role") != "1") return RedirectToPage("/Auth/Login");
        var result = await _apiService.DuplicateNewsAsync(duplicateId, GetUserId());
        Message = result.Success ? "Duplicate thành công" : result.Message;
        IsSuccess = result.Success;
        Articles = await _apiService.GetAllNewsAsync();
        Categories = await _apiService.GetActiveCategoriesAsync();
        return Page();
    }
}
