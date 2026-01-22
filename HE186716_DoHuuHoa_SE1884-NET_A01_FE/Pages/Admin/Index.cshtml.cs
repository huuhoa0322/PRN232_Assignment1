using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_FE.Pages.Admin;

public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        // Check if user is admin
        if (HttpContext.Session.GetString("IsAdmin") != "True")
        {
            return RedirectToPage("/Auth/Login");
        }
        return Page();
    }
}
