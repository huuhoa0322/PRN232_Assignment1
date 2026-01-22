using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Models;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Services;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_FE.Pages.Staff;

public class CategoriesModel : PageModel
{
    private readonly ApiService _apiService;
    public List<CategoryDto> Categories { get; set; } = new();
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }

    public CategoriesModel(ApiService apiService) => _apiService = apiService;

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("Role") != "1") return RedirectToPage("/Auth/Login");
        Categories = await _apiService.GetAllCategoriesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string handler, short? CategoryId, string CategoryName, string CategoryDesciption, bool IsActive)
    {
        if (HttpContext.Session.GetString("Role") != "1") return RedirectToPage("/Auth/Login");

        if (handler == "Create")
        {
            var dto = new CreateCategoryDto { CategoryName = CategoryName, CategoryDesciption = CategoryDesciption, IsActive = IsActive };
            var result = await _apiService.CreateCategoryAsync(dto);
            Message = result.Success ? "Tạo thành công" : result.Message;
            IsSuccess = result.Success;
        }
        else if (handler == "Update" && CategoryId.HasValue)
        {
            var dto = new UpdateCategoryDto { CategoryName = CategoryName, CategoryDesciption = CategoryDesciption, IsActive = IsActive };
            var result = await _apiService.UpdateCategoryAsync(CategoryId.Value, dto);
            Message = result.Success ? "Cập nhật thành công" : result.Message;
            IsSuccess = result.Success;
        }

        Categories = await _apiService.GetAllCategoriesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(short deleteId)
    {
        if (HttpContext.Session.GetString("Role") != "1") return RedirectToPage("/Auth/Login");
        var result = await _apiService.DeleteCategoryAsync(deleteId);
        Message = result.Success ? "Xóa thành công" : result.Message;
        IsSuccess = result.Success;
        Categories = await _apiService.GetAllCategoriesAsync();
        return Page();
    }
}
