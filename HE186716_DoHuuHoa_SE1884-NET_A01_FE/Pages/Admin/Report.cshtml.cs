using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Models;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Services;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_FE.Pages.Admin;

public class ReportModel : PageModel
{
    private readonly ApiService _apiService;

    public ReportStatisticsDto? Statistics { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? StartDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? EndDate { get; set; }

    public ReportModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "True")
            return RedirectToPage("/Auth/Login");

        Statistics = await _apiService.GetReportStatisticsAsync(StartDate, EndDate);
        return Page();
    }
}
