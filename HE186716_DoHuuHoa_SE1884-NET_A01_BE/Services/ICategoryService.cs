using HE186716_DoHuuHoa_SE1884_NET_A01_BE.DTOs;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();
    Task<CategoryDto?> GetByIdAsync(short id);
    Task<IEnumerable<CategoryDto>> SearchAsync(string? keyword);
    Task<PagedResultDto<CategoryDto>> SearchPagedAsync(string? keyword, int pageIndex, int pageSize);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    Task<CategoryDto?> UpdateAsync(short id, UpdateCategoryDto dto);
    Task<(bool Success, string Message)> DeleteAsync(short id);
}
