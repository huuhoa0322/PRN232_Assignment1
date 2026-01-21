using System.ComponentModel.DataAnnotations;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.DTOs;

// ===== CATEGORY DTOs =====
public class CategoryDto
{
    public short CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string CategoryDesciption { get; set; } = null!;
    public short? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public bool? IsActive { get; set; }
    public int ArticleCount { get; set; }
}

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    public string CategoryName { get; set; } = null!;

    [Required(ErrorMessage = "Category description is required")]
    [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
    public string CategoryDesciption { get; set; } = null!;

    public short? ParentCategoryId { get; set; }

    public bool IsActive { get; set; } = true;
}

public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    public string CategoryName { get; set; } = null!;

    [Required(ErrorMessage = "Category description is required")]
    [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
    public string CategoryDesciption { get; set; } = null!;

    public short? ParentCategoryId { get; set; }

    public bool? IsActive { get; set; }
}
