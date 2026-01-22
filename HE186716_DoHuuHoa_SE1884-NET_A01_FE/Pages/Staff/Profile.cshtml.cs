using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_FE.Pages.Staff;

public class ProfileModel : PageModel
{
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("Role") != "1") return RedirectToPage("/Auth/Login");
        return Page();
    }

    public IActionResult OnPost(string CurrentPassword, string NewPassword, string ConfirmPassword)
    {
        if (HttpContext.Session.GetString("Role") != "1") return RedirectToPage("/Auth/Login");

        if (NewPassword != ConfirmPassword)
        {
            Message = "Mật khẩu mới không khớp";
            IsSuccess = false;
            return Page();
        }

        // Note: Would need to call API to change password
        // For now, just show a message
        Message = "Chức năng đổi mật khẩu cần gọi API /api/account/{id}/change-password";
        IsSuccess = false;
        return Page();
    }
}
