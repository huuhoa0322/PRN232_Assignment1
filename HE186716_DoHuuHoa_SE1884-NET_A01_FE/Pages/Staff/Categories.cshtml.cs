using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Models;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Services;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_FE.Pages.Staff;

public class CategoriesModel : PageModel
{ 
    private readonly ApiService _apiService;
    public List<CategoryDto> Categories { get; set; } = new();
    public List<CategoryDto> AllCategories { get; set; } = new(); // For parent category dropdown
    
    [TempData]
    public string? Message { get; set; }
    
    [TempData]
    public bool IsSuccess { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Keyword { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageIndex { get; set; } = 1;

    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public CategoriesModel(ApiService apiService) => _apiService = apiService;

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("Role") != "1") 
            return RedirectToPage("/Auth/Login");
        
        // Load paged categories for display
        var pagedResult = await _apiService.SearchCategoriesPagedAsync(Keyword, PageIndex, PageSize);
        if (pagedResult != null)
        {
            Categories = pagedResult.Items;
            TotalPages = pagedResult.TotalPages;
            TotalCount = pagedResult.TotalCount;
        }
        
        // Load all categories for parent category dropdown
        AllCategories = await _apiService.GetAllCategoriesAsync(); 
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string handler, short? CategoryId, string CategoryName, string CategoryDesciption, short? ParentCategoryId, bool IsActive)
    {
        if (HttpContext.Session.GetString("Role") != "1") 
            return RedirectToPage("/Auth/Login");
         
        if (handler == "Create")
        {
            var dto = new CreateCategoryDto 
            { 
                CategoryName = CategoryName, 
                CategoryDesciption = CategoryDesciption, 
                ParentCategoryId = ParentCategoryId,
                IsActive = IsActive 
            };
            var result = await _apiService.CreateCategoryAsync(dto);
            Message = result.Message;
            IsSuccess = result.Success;
        }
        else if (handler == "Update" && CategoryId.HasValue)
        {
            var dto = new UpdateCategoryDto 
            { 
                CategoryName = CategoryName, 
                CategoryDesciption = CategoryDesciption, 
                ParentCategoryId = ParentCategoryId,
                IsActive = IsActive 
            };
            var result = await _apiService.UpdateCategoryAsync(CategoryId.Value, dto);
            Message = result.Message;
            IsSuccess = result.Success;
        }

        // PRG Pattern: Redirect to GET to prevent resubmission
        return RedirectToPage(new { Keyword, PageIndex });
    }

    public async Task<IActionResult> OnPostDeleteAsync(short deleteId)
    {
        if (HttpContext.Session.GetString("Role") != "1") 
            return RedirectToPage("/Auth/Login");
        
        var result = await _apiService.DeleteCategoryAsync(deleteId);
        Message = result.Message;
        IsSuccess = result.Success;
        
        // PRG Pattern: Redirect to GET to prevent resubmission
        return RedirectToPage(new { Keyword, PageIndex });
    }
}
