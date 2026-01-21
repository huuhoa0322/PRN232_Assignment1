using System.ComponentModel.DataAnnotations;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.DTOs;

// ===== AUTH DTOs =====
public class LoginRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}

public class LoginResponseDto
{
    public short AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? AccountEmail { get; set; }
    public int? AccountRole { get; set; }
    public string RoleName { get; set; } = null!;
    public bool IsAdmin { get; set; }
}
