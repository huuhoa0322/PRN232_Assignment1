using Microsoft.AspNetCore.Mvc.RazorPages;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Models;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Services;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_FE.Pages;

public class IndexModel : PageModel
{
    private readonly ApiService _apiService;

    public List<NewsArticleDto> Articles { get; set; } = new();
    public bool IsAuthenticated { get; set; }

    public IndexModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync()
    {
        // Ki?m tra n?u user ?ã ??ng nh?p (Admin, Staff, ho?c Lecturer)
        var role = HttpContext.Session.GetString("Role");
        IsAuthenticated = !string.IsNullOrEmpty(role);

        if (IsAuthenticated)
        {
            // Admin, Staff, Lecturer có th? xem t?t c? bài vi?t (bao g?m c? inactive)
            Articles = await _apiService.GetNewsForLecturerAsync();
        }
        else
        {
            // User ch?a ??ng nh?p ch? xem bài active
            Articles = await _apiService.GetActiveNewsAsync();
        }
    }
}
