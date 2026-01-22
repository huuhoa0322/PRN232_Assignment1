using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_FE.Pages.Staff;

public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("Role") != "1")
            return RedirectToPage("/Auth/Login");
        return Page();
    }
}
