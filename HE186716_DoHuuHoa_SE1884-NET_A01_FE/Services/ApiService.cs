using System.Net.Http.Json;
using System.Text.Json;
using HE186716_DoHuuHoa_SE1884_NET_A01_FE.Models;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_FE.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("NewsAPI");
        _httpContextAccessor = httpContextAccessor;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    // ===== AUTH =====
    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        var request = new LoginRequest { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<LoginResponse>(_jsonOptions);
        }
        return null;
    }

    // ===== NEWS ARTICLES =====
    public async Task<List<NewsArticleDto>> GetActiveNewsAsync()
    {
        var response = await _httpClient.GetAsync("api/news");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<NewsArticleDto>>(_jsonOptions) ?? new();
        }
        return new();
    }

    public async Task<NewsArticleDto?> GetNewsDetailAsync(string id)
    {
        var response = await _httpClient.GetAsync($"api/news/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<NewsArticleDto>(_jsonOptions);
        }
        return null;
    }

    public async Task<List<NewsArticleDto>> GetRelatedNewsAsync(string id)
    {
        var response = await _httpClient.GetAsync($"api/news/{id}/related");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<NewsArticleDto>>(_jsonOptions) ?? new();
        }
        return new();
    }

    public async Task<PagedResultDto<NewsArticleDto>?> SearchNewsPagedAsync(string? keyword, short? categoryId, int pageIndex, int pageSize)
    {
        var url = $"api/news/search?pageIndex={pageIndex}&pageSize={pageSize}&";
        if (!string.IsNullOrEmpty(keyword))
            url += $"keyword={Uri.EscapeDataString(keyword)}&";
        if (categoryId.HasValue)
            url += $"categoryId={categoryId}&";
        
        var response = await _httpClient.GetAsync(url.TrimEnd('&', '?'));
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PagedResultDto<NewsArticleDto>>(_jsonOptions);
        }
        return null;
    }

    public async Task<List<NewsArticleDto>> SearchNewsAsync(string? keyword, short? categoryId)
    {
        // Adapter for legacy calls - API now returns PagedResult
        var url = "api/news/search?";
        if (!string.IsNullOrEmpty(keyword))
            url += $"keyword={Uri.EscapeDataString(keyword)}&";
        if (categoryId.HasValue)
            url += $"categoryId={categoryId}&";
        
        var response = await _httpClient.GetAsync(url.TrimEnd('&', '?'));
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PagedResultDto<NewsArticleDto>>(_jsonOptions);
            return result?.Items ?? new List<NewsArticleDto>();
        }
        return new();
    }

    // ===== CATEGORIES =====
    public async Task<List<CategoryDto>> GetActiveCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("api/category/active");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>(_jsonOptions) ?? new();
        }
        return new();
    }

    // ===== ACCOUNTS (Admin) =====
    public async Task<List<AccountDto>> GetAllAccountsAsync()
    {
        var response = await _httpClient.GetAsync("api/account");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<AccountDto>>(_jsonOptions) ?? new();
        }
        return new();
    }

    public async Task<PagedResultDto<AccountDto>?> SearchAccountsPagedAsync(string? keyword, int? role, int pageIndex, int pageSize)
    {
        var url = $"api/account/search?pageIndex={pageIndex}&pageSize={pageSize}&";
        if (!string.IsNullOrEmpty(keyword))
            url += $"keyword={Uri.EscapeDataString(keyword)}&";
        if (role.HasValue)
            url += $"role={role}&";
        
        var response = await _httpClient.GetAsync(url.TrimEnd('&', '?'));
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PagedResultDto<AccountDto>>(_jsonOptions);
        }
        return null;
    }

    public async Task<List<AccountDto>> SearchAccountsAsync(string? keyword, int? role)
    {
        // Fallback or deprecated
        var url = "api/account/search?";
        if (!string.IsNullOrEmpty(keyword))
            url += $"keyword={Uri.EscapeDataString(keyword)}&";
        if (role.HasValue)
            url += $"role={role}&";
        
        var response = await _httpClient.GetAsync(url.TrimEnd('&', '?'));
         if (response.IsSuccessStatusCode)
        {
             // Note: The API might now return PagedResult by default for search endpoint if we changed it completely.
             // But in AccountController we changed Search to return PagedResult.
             // So this legacy method might break if we calling the same endpoint.
             // Let's assume we use the new method primarily.
             // If we changed the endpoint signature JSON return type, we need to adapt here.
             // Since we changed AccountController.Search to return PagedResult, we should parse it as such and return Items.
            var result = await response.Content.ReadFromJsonAsync<PagedResultDto<AccountDto>>(_jsonOptions);
            return result?.Items ?? new List<AccountDto>();
        }
        return new();
    }

    public async Task<(bool Success, string Message)> CreateAccountAsync(CreateAccountDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/account", dto);
        if (response.IsSuccessStatusCode)
            return (true, "Account created successfully");
        
        var error = await response.Content.ReadAsStringAsync();
        return (false, error);
    }

    public async Task<(bool Success, string Message)> UpdateAccountAsync(short id, UpdateAccountDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/account/{id}", dto);
        if (response.IsSuccessStatusCode)
            return (true, "Account updated successfully");
        
        var error = await response.Content.ReadAsStringAsync();
        return (false, error);
    }

    public async Task<(bool Success, string Message)> DeleteAccountAsync(short id)
    {
        var response = await _httpClient.DeleteAsync($"api/account/{id}");
        if (response.IsSuccessStatusCode)
            return (true, "Account deleted successfully");
        
        var error = await response.Content.ReadAsStringAsync();
        return (false, error);
    }

    // ===== REPORTS (Admin) =====
    public async Task<ReportStatisticsDto?> GetReportStatisticsAsync(DateTime? startDate, DateTime? endDate)
    {
        var url = "api/report/statistics?";
        if (startDate.HasValue)
            url += $"startDate={startDate.Value:yyyy-MM-dd}&";
        if (endDate.HasValue)
            url += $"endDate={endDate.Value:yyyy-MM-dd}&";
        
        var response = await _httpClient.GetAsync(url.TrimEnd('&', '?'));
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ReportStatisticsDto>(_jsonOptions);
        }
        return null;
    }

    // ===== CATEGORIES (Staff) =====
    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("api/category");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>(_jsonOptions) ?? new();
        return new();
    }

    public async Task<(bool Success, string Message)> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/category", dto);
        if (response.IsSuccessStatusCode) return (true, "Success");
        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool Success, string Message)> UpdateCategoryAsync(short id, UpdateCategoryDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/category/{id}", dto);
        if (response.IsSuccessStatusCode) return (true, "Success");
        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool Success, string Message)> DeleteCategoryAsync(short id)
    {
        var response = await _httpClient.DeleteAsync($"api/category/{id}");
        if (response.IsSuccessStatusCode) return (true, "Success");
        return (false, await response.Content.ReadAsStringAsync());
    }

    // ===== NEWS ARTICLES (Staff) =====
    public async Task<List<NewsArticleDto>> GetAllNewsAsync()
    {
        var response = await _httpClient.GetAsync("api/news/all");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<NewsArticleDto>>(_jsonOptions) ?? new();
        return new();
    }

    public async Task<List<NewsArticleDto>> GetNewsByAuthorAsync(short authorId)
    {
        var response = await _httpClient.GetAsync($"api/news/author/{authorId}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<NewsArticleDto>>(_jsonOptions) ?? new();
        return new();
    }

    public async Task<(bool Success, string Message)> CreateNewsAsync(CreateNewsArticleDto dto, short createdById)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/news?createdById={createdById}", dto);
        if (response.IsSuccessStatusCode) return (true, "Success");
        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool Success, string Message)> UpdateNewsAsync(string id, UpdateNewsArticleDto dto, short updatedById)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/news/{id}?updatedById={updatedById}", dto);
        if (response.IsSuccessStatusCode) return (true, "Success");
        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool Success, string Message)> DeleteNewsAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"api/news/{id}");
        if (response.IsSuccessStatusCode) return (true, "Success");
        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool Success, string Message)> DuplicateNewsAsync(string id, short createdById)
    {
        var response = await _httpClient.PostAsync($"api/news/{id}/duplicate?createdById={createdById}", null);
        if (response.IsSuccessStatusCode) return (true, "Success");
        return (false, await response.Content.ReadAsStringAsync());
    }

    // ===== TAGS (Staff) =====
    public async Task<List<TagDto>> GetAllTagsAsync()
    {
        var response = await _httpClient.GetAsync("api/tag");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<TagDto>>(_jsonOptions) ?? new();
        return new();
    }

    public async Task<(bool Success, string Message)> CreateTagAsync(CreateTagDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/tag", dto);
        if (response.IsSuccessStatusCode) return (true, "Success");
        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool Success, string Message)> UpdateTagAsync(int id, UpdateTagDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/tag/{id}", dto);
        if (response.IsSuccessStatusCode) return (true, "Success");
        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool Success, string Message)> DeleteTagAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/tag/{id}");
        if (response.IsSuccessStatusCode) return (true, "Success");
        return (false, await response.Content.ReadAsStringAsync());
    }
}


