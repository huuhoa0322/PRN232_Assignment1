using System.ComponentModel.DataAnnotations;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.DTOs;

// ===== TAG DTOs =====
public class TagDto
{
    public int TagId { get; set; }
    public string? TagName { get; set; }
    public string? Note { get; set; }
    public int ArticleCount { get; set; }
}

public class CreateTagDto
{
    [Required(ErrorMessage = "Tag name is required")]
    [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters")]
    public string TagName { get; set; } = null!;

    [StringLength(400, ErrorMessage = "Note cannot exceed 400 characters")]
    public string? Note { get; set; }
}

public class UpdateTagDto
{
    [Required(ErrorMessage = "Tag name is required")]
    [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters")]
    public string TagName { get; set; } = null!;

    [StringLength(400, ErrorMessage = "Note cannot exceed 400 characters")]
    public string? Note { get; set; }
}
