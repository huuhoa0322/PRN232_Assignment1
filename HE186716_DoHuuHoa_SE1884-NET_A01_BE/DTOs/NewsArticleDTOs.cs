using System.ComponentModel.DataAnnotations;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.DTOs;

// ===== NEWS ARTICLE DTOs =====
public class NewsArticleDto
{
    public string NewsArticleId { get; set; } = null!;
    public string? NewsTitle { get; set; }
    public string Headline { get; set; } = null!;
    public DateTime? CreatedDate { get; set; }
    public string? NewsContent { get; set; }
    public string? NewsSource { get; set; }
    public short? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public bool? NewsStatus { get; set; }
    public short? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public short? UpdatedById { get; set; }
    public string? UpdatedByName { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public List<TagDto> Tags { get; set; } = new();
}

public class CreateNewsArticleDto
{
    [StringLength(400, ErrorMessage = "Title cannot exceed 400 characters")]
    public string? NewsTitle { get; set; }

    [Required(ErrorMessage = "Headline is required")]
    [StringLength(150, ErrorMessage = "Headline cannot exceed 150 characters")]
    public string Headline { get; set; } = null!;

    [StringLength(20000, ErrorMessage = "Content cannot exceed 20000 characters")]
    public string? NewsContent { get; set; }

    [StringLength(400, ErrorMessage = "Source cannot exceed 400 characters")]
    public string? NewsSource { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public short CategoryId { get; set; }

    public bool NewsStatus { get; set; } = true;

    public List<int>? TagIds { get; set; }
}

public class UpdateNewsArticleDto
{
    [StringLength(400, ErrorMessage = "Title cannot exceed 400 characters")]
    public string? NewsTitle { get; set; }

    [Required(ErrorMessage = "Headline is required")]
    [StringLength(150, ErrorMessage = "Headline cannot exceed 150 characters")]
    public string Headline { get; set; } = null!;

    [StringLength(4000, ErrorMessage = "Content cannot exceed 4000 characters")]
    public string? NewsContent { get; set; }

    [StringLength(400, ErrorMessage = "Source cannot exceed 400 characters")]
    public string? NewsSource { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public short CategoryId { get; set; }

    public bool? NewsStatus { get; set; }

    public List<int>? TagIds { get; set; }
}

public class NewsArticleSearchDto
{
    public string? Keyword { get; set; }
    public short? CategoryId { get; set; }
    public bool? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
